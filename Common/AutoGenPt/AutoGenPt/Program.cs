using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJson;
namespace AutoGenPt
{  
    class Program
    {
        const string Template =
@"//Template auto generator:[AutoGenPt] v1.0
//Creation time:[TIME]
using System;
using System.Collections;
using System.Collections.Generic;
namespace [NAMESPACE]
{
public class [CLASSNAME]
{
    [FIELDS]   
    [SETTER]
    [HASFIELDS]
    public static byte[] Write([CLASSNAME] data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            [WRITE_FIELDS]
            return buffer.Getbuffer();
        }
    }

    public static [CLASSNAME] Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            [CLASSNAME] data = new [CLASSNAME]();
            [READ_FIELDS]
            return data;
        }       
    }
}
}
";


        static void Main(string[] args)
        {
            if(args.Length != 4 || Array.IndexOf(args, "-s") == -1 || Array.IndexOf(args, "-o") == -1)
            {
                Console.WriteLine(string.Join(" " ,args));
                Console.WriteLine("AutoGenPt -s [PtConfig file's full path] -o [generate codes folder's full path]");
                return;
            }
            string srcPath = args[1 + Array.IndexOf(args, "-s")];
            string genPath = args[1 + Array.IndexOf(args, "-o")];

            GenCode(srcPath,genPath);
            Console.ReadKey();
        }
        static async void GenCode(string srcPath,string genPath)
        {
            var result = await ReadFile(srcPath);
            Console.WriteLine(result);
            CollectionData data;
            try
            {
                data = LitJson.JsonMapper.ToObject<CollectionData>(result);
                Console.WriteLine(data.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if (!Directory.Exists(genPath))
                Directory.CreateDirectory(genPath);

            foreach(var pt in data.pts)
            {
                string fileContent = Template;
                fileContent = fileContent.Replace("[TIME]",DateTime.Now.ToString());
                fileContent = fileContent.Replace("[CLASSNAME]", pt.className);
                fileContent = fileContent.Replace("[NAMESPACE]", data.ns);
                var markTypeName = pt.GetMarkTypeName();
                var fields = $"public {Field.FieldTypeValue[markTypeName]} __tag__ "+"{ get;private set;}\n\n\t";
                var setter = "";
                var hasFields = "";

                for (int i=0;i<pt.fields.Count;++i)
                {
                    fields += pt.fields[i].ToString() + "\n\t";
                    setter += pt.fields[i].ToSetter(pt.className,i) + "\n\t";
                    hasFields += pt.fields[i].ToHasField(i) + "\n\t";
                }
                fileContent = fileContent.Replace("[FIELDS]", fields);
                fileContent = fileContent.Replace("[SETTER]", setter);
                fileContent = fileContent.Replace("[HASFIELDS]",hasFields);
                fileContent = fileContent.Replace("[WRITE_FIELDS]", pt.ToWriteString());
                fileContent = fileContent.Replace("[READ_FIELDS]", pt.ToReadString());
                //await WriteFile(genPath+ "\\" + pt.className + ".cs",fileContent);
                await Task.Run(()=> {
                    File.WriteAllText(genPath + "\\" + pt.className+".cs", fileContent);
                    Console.WriteLine(" File writing is completed " + genPath + "\\" + pt.className);
                });

                
            }

            Console.WriteLine("Auto Gen Pt Complete.");
        }
        
        static async Task<string> ReadFile(string file)
        {
            string result = "";

            Console.WriteLine(" File reading is stating");
            using (StreamReader reader = new StreamReader(file))
            {
                result = await reader.ReadToEndAsync();
            }
            Console.WriteLine(" File reading is completed");
            return result;
        }
    }

   
    class CollectionData
    {
        public string ns = "DefaultName";
        public List<Pt> pts=new List<Pt>();
    }

    class Pt
    {
        public string className="";
        public List<Field> fields=new List<Field>();
        public static Dictionary<string, string> ReadFieldValue = new Dictionary<string, string>()
        {
            { "int32","buffer.ReadInt32()" },
            { "int16","buffer.ReadInt16()" },
            { "int64","buffer.ReadInt64()" },
            { "uint32","buffer.ReadUInt32()" },
            { "uint16","buffer.ReadUInt16()" },
            { "uint64","buffer.ReadUInt64()" },
            { "string","buffer.ReadString()" },
            { "float","buffer.ReadFloat()" },
            { "double","buffer.ReadDouble()" },
            { "byte","buffer.ReadByte()" },
            { "bool","buffer.ReadBool()" },
            { "bytes","buffer.ReadBytes()" },      
        };

        public static Dictionary<string, string> WriteFieldValue = new Dictionary<string, string>()
        {
            { "int32","buffer.WriteInt32({0})" },
            { "int16","buffer.WriteInt16({0})" },
            { "int64","buffer.WriteInt64({0})" },
            { "uint32","buffer.WriteUInt32({0})" },
            { "uint16","buffer.WriteUInt16({0})" },
            { "uint64","buffer.WriteUInt64({0})" },
            { "string","buffer.WriteString({0})" },
            { "float","buffer.WriteFloat({0})" },
            { "double","buffer.WriteDouble({0})" },
            { "byte","buffer.WriteByte({0})" },
            { "bool","buffer.WriteBool({0})" },
            { "bytes","buffer.WriteBytes({0})" },
            
        };

        public string GetMarkTypeName()
        {
            int count = fields.Count;
            if(count<=8)
            {
                return "byte";
            }
            if(count<=16)
            {
                return "uint16";
            }
            if(count<=32)
            {
                return "uint32";
            }
            if(count<=64)
            {
                return "uint64";
            }
            throw new Exception("结构体最大字段数64.");
        }
        public string ToReadString()
        {
            var mark = GetMarkTypeName();
            string result = string.Format("data.__tag__ = "+ReadFieldValue[mark]) + ";\n\t\t\t";
            for (int i=0;i<fields.Count;++i) 
            {
                var f = fields[i];
                if(f.isArray == 0)
                {
                    if(Field.IsSystemStructType(f.type))
                        result += $"if(data.Has{f.name}())"+string.Format("data.{0} = "+ReadFieldValue[f.type]+";",f.name);
                    else
                        result += $"if(data.Has{f.name}())"+string.Format("data.{0} = {1}.Read(buffer.ReadBytes());", f.name,f.type);
                }
                else
                {
                    if(Field.IsSystemStructType(f.type))
                        result += $"if(data.Has{f.name}())"+string.Format("data.{0} = buffer.ReadCollection(()=>{1});", f.name, ReadFieldValue[f.type]);
                    else
                        result += $"if(data.Has{f.name}())"+string.Format("data.{0} = buffer.ReadCollection(retbytes=>{1}.Read(retbytes));", f.name,f.type);
                }
                result += "\n\t\t\t";
            }
            return result;
        }
        public string ToWriteString()
        {
            var mark = GetMarkTypeName();
            string result = string.Format(WriteFieldValue[mark],"data.__tag__")+";\n\t\t\t";
            for(int i=0;i<fields.Count;++i) 
            {
                var f = fields[i];
                if (f.isArray == 0)
                {
                    if (Field.IsSystemStructType(f.type))
                        result += $"if(data.Has{f.name}())" + string.Format(WriteFieldValue[f.type], "data."+f.name) + ";";
                    else
                        result += $"if(data.Has{f.name}())"+string.Format("buffer.WriteBytes({0}.Write({1}))", f.type, "data." + f.name) + ";";
                }
                else
                {
                    if (Field.IsSystemStructType(f.type))
                        result += $"if(data.Has{f.name}())"+string.Format("buffer.WriteCollection(data.{0},element=>{1});", f.name, string.Format(WriteFieldValue[f.type], "element"));
                    else
                        result += $"if(data.Has{f.name}())"+string.Format("buffer.WriteCollection(data.{0},element=>{1}.Write(element));",f.name,f.type);
                }
                result += "\n\t\t\t";
            }
        
            return result;
        }

        
    }
    class Field
    {
        public string type="";
        public string name="";
        public int isArray = 0;


        public string ToHasField(int index)
        {
            int value = 1 << index;
            return $"public bool Has{name}()" + "{" + "return (__tag__&" + value + ")=="+value+";}";
        }
        public string ToSetter(string className,int index)
        {
            if (isArray == 0)
            {
                if (IsSystemStructType(type))
                    return $"public {className} Set{name}({FieldTypeValue[type]} value)" + "{" + $"{name}=value; __tag__|={1<<index};" + " return this;}";
                else
                    return $"public {className} Set{name}({type} value)"+"{"+$"{name}=value; __tag__|={1<<index};" + " return this;}";
            }
            else
            {
                if (IsSystemStructType(type))
                    return $"public {className} Set{name}(List<{FieldTypeValue[type]}> value)" + "{" + $"{name}=value; __tag__|={1<<index};" + " return this;}";
                else
                    return $"public {className} Set{name}(List<{type}> value)"+"{"+$"{name}=value; __tag__|={1<<index};" + " return this;}";
            }
        }
        public override string ToString()
        {
            if (isArray == 0)
            {
                if (IsSystemStructType(type))
                    return string.Format("public {0} {1}", FieldTypeValue[type], name)+ "{ get;private set;}";
                else
                    return string.Format("public {0} {1}", type,name)+ "{ get;private set;}";
            }
            else
            {
                if (IsSystemStructType(type))
                    return string.Format("public List<{0}> {1}", FieldTypeValue[type], name)+"{ get;private set;}";
                else
                    return string.Format("public List<{0}> {1}", type, name)+ "{ get;private set;}";
            }
        }

        public static bool IsSystemStructType(string type)
        {
            return FieldTypeValue.ContainsKey(type);
        }


        public static Dictionary<string, string> FieldTypeValue = new Dictionary<string, string>()
        {
            { "byte","byte"},
            { "bool","bool"},
            { "int32", "int" },
            { "int16","short"},
            { "int64","long"},
            { "uint32","uint"},
            { "uint16","ushort"},
            { "uint64","ulong"},
            { "string","string"},
            { "float","float"},
            { "double","double"},
        };
    }


}
