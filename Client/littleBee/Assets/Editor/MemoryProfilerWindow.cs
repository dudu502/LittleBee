using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Editor.Treemap;
using Treemap;
using UnityEditor;
using UnityEngine;
using System;
using System.Net;
using NUnit.Framework.Constraints;
using UnityEditor.MemoryProfiler;
using Object = UnityEngine.Object;
using System.IO;

namespace MemoryProfilerWindow
{
    public class MemoryProfilerWindow : EditorWindow
    {
        [NonSerialized]
        UnityEditor.MemoryProfiler.PackedMemorySnapshot _snapshot;

        [SerializeField]
        PackedCrawlerData _packedCrawled;

        [NonSerialized]
        CrawledMemorySnapshot _unpackedCrawl;

        Vector2 _scrollPosition;

        [NonSerialized]
        private bool _registered = false;
        public Inspector _inspector;
        TreeMapView _treeMapView;

        [MenuItem("Window/MemoryProfiler")]
        static void Create()
        {
            EditorWindow.GetWindow<MemoryProfilerWindow>();
        }

        [MenuItem("Window/MemoryProfilerInspect")]
        static void Inspect()
        {
        }

        public void OnDestroy()
        {
            UnityEditor.MemoryProfiler.MemorySnapshot.OnSnapshotReceived -= IncomingSnapshot;

            if (_treeMapView != null)
                _treeMapView.CleanupMeshes ();
        }

        public void Initialize()
        {
            if (_treeMapView == null)
                _treeMapView = new TreeMapView ();
            
            if (!_registered)
            {
                UnityEditor.MemoryProfiler.MemorySnapshot.OnSnapshotReceived += IncomingSnapshot;
                _registered = true;
            }

            if (_unpackedCrawl == null && _packedCrawled != null && _packedCrawled.valid)
                Unpack();


        }

        void OnGUI()
        {
            Initialize();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Take Snapshot"))
            {
                UnityEditor.EditorUtility.DisplayProgressBar("Take Snapshot", "Downloading Snapshot...", 0.0f);
                try
                {
                    UnityEditor.MemoryProfiler.MemorySnapshot.RequestNewSnapshot();
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }

            EditorGUI.BeginDisabledGroup(_snapshot == null);
            if (GUILayout.Button("Save Snapshot..."))
            {
                PackedMemorySnapshotUtility.SaveToFile(_snapshot);
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Load Snapshot..."))
            {
                PackedMemorySnapshot packedSnapshot = PackedMemorySnapshotUtility.LoadFromFile();
                if(packedSnapshot != null)
                    IncomingSnapshot(packedSnapshot);
            }

            if (_unpackedCrawl != null)
            {
                GUILayout.Label(string.Format("Total memory: {0}", EditorUtility.FormatBytes(_unpackedCrawl.totalSize)));
            }
            GUILayout.EndHorizontal();
            if (_treeMapView != null)
                _treeMapView.Draw();
            if (_inspector != null)
                _inspector.Draw();

            //RenderDebugList();
        }

        public void SelectThing(ThingInMemory thing)
        {
            _inspector.SelectThing(thing);
            _treeMapView.SelectThing(thing);
        }

        public void SelectGroup(Group group)
        {
            _treeMapView.SelectGroup(group);
        }

        private void RenderDebugList()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            foreach (var thing in _unpackedCrawl.allObjects)
            {
                var mo = thing as ManagedObject;
                if (mo != null)
                    GUILayout.Label("MO: " + mo.typeDescription.name);

                var gch = thing as GCHandle;
                if (gch != null)
                    GUILayout.Label("GCH: " + gch.caption);

                var sf = thing as StaticFields;
                if (sf != null)
                    GUILayout.Label("SF: " + sf.typeDescription.name);
            }

            GUILayout.EndScrollView();
        }

        void Unpack()
        {
            _unpackedCrawl = CrawlDataUnpacker.Unpack(_packedCrawled);
            _inspector = new Inspector(this, _unpackedCrawl, _snapshot);

            if(_treeMapView != null)
                _treeMapView.Setup(this, _unpackedCrawl);
        }

        void IncomingSnapshot(PackedMemorySnapshot snapshot)
        {
            _snapshot = snapshot;

            UnityEditor.EditorUtility.DisplayProgressBar("Take Snapshot", "Crawling Snapshot...", 0.33f);
            try
            {
                _packedCrawled = new Crawler().Crawl(_snapshot);

                UnityEditor.EditorUtility.DisplayProgressBar("Take Snapshot", "Unpacking Snapshot...", 0.67f);

                Unpack();
            }
            finally
            {
                UnityEditor.EditorUtility.ClearProgressBar();
            }
        }
    }
}
