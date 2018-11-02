using UnityEditor;
using System.IO;

public class NodeScriptTemplate 
{
    public static void DeleteScript(string path)
    {
        if(EditorUtility.DisplayDialog("提示", "确定删除" + path, "OK"))
        {
            File.Delete(path);
            AssetDatabase.Refresh();
        }      
    }
    public static void NewEmptyScript(string path,NodeGraph node)
    {
        string result = string.Format(@"
--[[
    基本信息：{0}
    创建日期：{1}
--]]

--进入条件检测
function detect()
    return true
end
", node.ToString(),System.DateTime.Now.ToString());
        if (node.Type == NodeGraph.NODETYPE.ACTION)
        {
            result += @"

--进入调用
function enter()

end

--每隔dt秒更新
function update(dt)
    
end

--事件监听
function trigger(type,obj)

end

--退出调用
function exit()

end
";
        }
        File.WriteAllText(path, result);
        AssetDatabase.Refresh();
    }
}