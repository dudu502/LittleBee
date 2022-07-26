using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

#if UNITY_5_5_OR_NEWER
using Profiler = UnityEngine.Profiling.Profiler;
#else
using Profiler = UnityEngine.Profiler;
#endif

public static class PackedMemorySnapshotUtility
{

    private static string previousDirectory = null;

    public static void SaveToFile(PackedMemorySnapshot snapshot)
    {
        var filePath = EditorUtility.SaveFilePanel("Save Snapshot", previousDirectory, "MemorySnapshot", "memsnap3");
        if(string.IsNullOrEmpty(filePath))
            return;

        previousDirectory = Path.GetDirectoryName(filePath);
        SaveToFile(filePath, snapshot);
    }

    static void SaveToFile(string filePath, PackedMemorySnapshot snapshot)
    {
        // Saving snapshots using JsonUtility, instead of BinaryFormatter, is significantly faster.
        // I cancelled saving a memory snapshot that is saving using BinaryFormatter after 24 hours.
        // Saving the same memory snapshot using JsonUtility.ToJson took 20 seconds only.

        Debug.LogFormat("Saving...");
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        Profiler.BeginSample("PackedMemorySnapshotUtility.SaveToFile");
        stopwatch.Start();

        string fileExtension = Path.GetExtension(filePath);
        if (string.Equals(fileExtension, ".memsnap", System.StringComparison.OrdinalIgnoreCase)) {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (Stream stream = File.Open(filePath, FileMode.Create)) {
                bf.Serialize(stream, snapshot);
            }
        } else if (string.Equals(fileExtension, ".memsnap2", System.StringComparison.OrdinalIgnoreCase)) {
            var json = JsonUtility.ToJson(snapshot);
            File.WriteAllText(filePath, json);
        } else { // memsnap3 + default
            // Stream writing -- will not to exhaust memory (for large snapshots)
            using (TextWriter writer = File.CreateText(filePath)) {
                var errors = new List<string>();
                var serializer = getSerializer(errors);
                serializer.Serialize(writer, snapshot);
                logErrors(errors);
            }
        }

        stopwatch.Stop();
        Profiler.EndSample();
        Debug.LogFormat("Saving took {0}ms", stopwatch.ElapsedMilliseconds);
    }

    public static PackedMemorySnapshot LoadFromFile()
    {
        var filePath = EditorUtility.OpenFilePanelWithFilters("Load Snapshot", previousDirectory, new[] { "Snapshots", "memsnap3,memsnap2,memsnap" });
        if(string.IsNullOrEmpty(filePath))
            return null;

        previousDirectory = Path.GetDirectoryName(filePath);
        Debug.LogFormat("Loading \"{0}\"", filePath);
        var packedSnapshot = LoadFromFile(filePath);
        Debug.LogFormat("Completed loading \"{0}\"", filePath);
        return packedSnapshot;
    }

    static PackedMemorySnapshot LoadFromFile(string filePath)
    {
        Debug.LogFormat("Loading...");
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        PackedMemorySnapshot result;
        string fileExtension = Path.GetExtension(filePath);

        if (string.Equals(fileExtension, ".memsnap3", System.StringComparison.OrdinalIgnoreCase)) {
            Profiler.BeginSample("PackedMemorySnapshotUtility.LoadFromFile(litjson)");
            stopwatch.Start();

            using (TextReader reader = File.OpenText(filePath)) {
                var errors = new List<string>();
                var serializer = getSerializer(errors);
                result = (PackedMemorySnapshot) serializer.Deserialize(reader, typeof(PackedMemorySnapshot));
                logErrors(errors);
            }

            stopwatch.Stop();
            Profiler.EndSample();
        }
        else if(string.Equals(fileExtension, ".memsnap2", System.StringComparison.OrdinalIgnoreCase))
        {
            Profiler.BeginSample("PackedMemorySnapshotUtility.LoadFromFile(json)");
            stopwatch.Start();

            var json = File.ReadAllText(filePath);
            result = JsonUtility.FromJson<PackedMemorySnapshot>(json);

            stopwatch.Stop();
            Profiler.EndSample();
        }
        else if(string.Equals(fileExtension, ".memsnap", System.StringComparison.OrdinalIgnoreCase))
        {
            Profiler.BeginSample("PackedMemorySnapshotUtility.LoadFromFile(binary)");
            stopwatch.Start();

            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using(Stream stream = File.Open(filePath, FileMode.Open))
            {
                result = binaryFormatter.Deserialize(stream) as PackedMemorySnapshot;
            }

            stopwatch.Stop();
            Profiler.EndSample();
        }
        else
        {
            Debug.LogErrorFormat("MemoryProfiler: Unrecognized memory snapshot format '{0}'.", filePath);
            result = null;
        }

        Debug.LogFormat("Loading took {0}ms", stopwatch.ElapsedMilliseconds);
        return result;
    }

    private static JsonSerializer getSerializer(List<string> errors) {
        JsonSerializer serializer = new JsonSerializer();
        serializer.ContractResolver = new MyContractResolver();
        serializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        serializer.MissingMemberHandling = MissingMemberHandling.Error;
        serializer.Error += (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) => {
            errors.Add(string.Format("'{1}' from '{0}'", sender, args.ErrorContext.Error.Message));
        };
        // Enable this to get detailed logging
        //serializer.TraceWriter = new FilteringTraceLogWriter("typeDescriptions");
        return serializer;
    }

    private static void logErrors(List<string> errors) {
        if (0 < errors.Count) {
            var last = Mathf.Min(20, errors.Count); // might be very large, just do the first 20
            var sb = new StringBuilder();
            for (int i = 0; i < last; i++) {
                sb.AppendLine(errors[i]);
            }
            UnityEngine.Debug.Log(sb.ToString());
        }
    }

    private sealed class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {

        public MyContractResolver() : base() {
            IgnoreSerializableAttribute = false; // Use SerializableAttribute to determine what to serialize!
        }

    }

    private class FilteringTraceLogWriter : Newtonsoft.Json.Serialization.ITraceWriter {

        private readonly string pattern;

        public FilteringTraceLogWriter() : this(null) {
        }

        public FilteringTraceLogWriter(string pattern) {
            this.pattern = pattern;
        }

        public System.Diagnostics.TraceLevel LevelFilter {
            get { return System.Diagnostics.TraceLevel.Verbose; }
        }

        private static readonly LogType[] logTypeFromTraceLevel = new LogType[] {
            LogType.Log, // off -- it's handled elsewhere
            LogType.Error, // error
            LogType.Warning, // warning
            LogType.Log, // info
            LogType.Log, // verbose
        };

        public void Trace(System.Diagnostics.TraceLevel level, string message, Exception ex) {
            if (System.Diagnostics.TraceLevel.Off == level || null != pattern) {
                if (!message.Contains(pattern)) { // Skip if not pattern
                    return;
                }
            }

            UnityEngine.Debug.unityLogger.Log(logTypeFromTraceLevel[(int) level], message);
            if (null != ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }
    }

}

