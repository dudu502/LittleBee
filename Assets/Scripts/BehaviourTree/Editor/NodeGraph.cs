using System.Collections.Generic;
using UnityEngine;

public class NodeGraph
{
    static Color S_ALPHA_COLOR = new Color(0, 0, 0, 0.3f);
    public static string[] ToolBarNames = new[] { "Export Tree", "Add SubTree"};
    public enum NODETYPE
    {
        ACTION = 0,
        SEQUENCE = 1,
        RANDOWSELECT = 2,
        PAIALLEL = 3,
    }
    public int ID;
    public string Name="";
    public NODETYPE Type = NODETYPE.ACTION;
    public int Weight=1;
    public string ScriptName="";
    public List<NodeGraph> Nodes = new List<NodeGraph>();
    public Vector2 ClickPos;
    public Rect NodeRect;
    public NodeGraph Parent = null;
    public string OutPutPath = "";
    public TextAsset SubTreeAsset = null;
    public int ToolBarSelectIndex = 0;
    public bool FoldOutDes = true;
    public bool FoldOutScriptName = true;
    public NodeGraph()
    {       
        NodeRect = new Rect(0,30,60,50);
        ID = GetHashCode();
    }
    

    public void AddNode(NodeGraph node)
    {
        node.Parent = this;
        Nodes.Add(node);
        if (Type == NODETYPE.ACTION)
            Type = NODETYPE.SEQUENCE;
    }

    public void AddNode(NodeGraph node, Vector2 offset)
    {
        AddNode(node);
        //
        List<NodeGraph> result = new List<NodeGraph>();
        GetAllNodes(node, result);
        foreach (var nItem in result)
        {
            nItem.NodeRect.position += NodeRect.position + offset ;
        }
    }

    public void RemoveNode(NodeGraph node)
    {
        Nodes.Remove(node);
        if (Nodes.Count == 0)
            Type = NODETYPE.ACTION;
    }

    public Rect ToRect()
    {
        NodeRect.x = ClickPos.x;
        NodeRect.y = ClickPos.y;
        NodeRect.width = 240;
        NodeRect.height = 240;
        return NodeRect;
    }

    public void ExchangeChild(int index0,int index1)
    {
        if (index0 > -1 && index0 < Nodes.Count
            && index1 > -1 && index1 < Nodes.Count)
        {
            var temp = Nodes[index0];
            Nodes[index0] = Nodes[index1];
            Nodes[index1] = temp;
        }
    }

    public bool HasPrevChild(NodeGraph child)
    {
        var index = Nodes.IndexOf(child);
        return index > 0;
    }
    public bool HasNextChild(NodeGraph child)
    {
        var index = Nodes.IndexOf(child);
        return index < Nodes.Count - 1;
    }

    public static Color GetColorByType(int type)
    {
        if (type == (int)NODETYPE.ACTION)
            return Color.green - S_ALPHA_COLOR;
        if (type == (int)NODETYPE.PAIALLEL)
            return Color.red - S_ALPHA_COLOR;
        if (type == (int)NODETYPE.RANDOWSELECT)
            return Color.yellow - S_ALPHA_COLOR;
        return Color.gray - S_ALPHA_COLOR;
    }


    public override string ToString()
    {
        return string.Format("Name:{0} Type:{1} Weight:{2}",Name,Type.ToString(),Weight);
    }
    public static void GetAllNodes(NodeGraph node, List<NodeGraph> result)
    {
        result.Add(node);
        foreach (var child in node.Nodes)
        {
            GetAllNodes(child, result);
        }
    }
    public static NodeGraph FindByID(NodeGraph node, int id)
    {
        List<NodeGraph> all = new List<NodeGraph>();
        GetAllNodes(node, all);
        foreach (var item in all)
        {
            if (item.ID == id)
                return item;
        }
        return null;
    }

    public static NodeGraph FindByMousePos(NodeGraph node, Vector3 mpos)
    {
        List<NodeGraph> all = new List<NodeGraph>();
        GetAllNodes(node, all);
        foreach (var item in all)
        {
            if (item.NodeRect.Contains(mpos))
                return item;
        }
        return null;
    }
    
    public static NodeGraph CreateFromBinary(byte[] value)
    {
        ByteBuffer buffer = new ByteBuffer(value);
        NodeGraph data = new NodeGraph();
        data.ToRect();
        data.Name = buffer.ReadString();
        data.Type = (NODETYPE)buffer.ReadByte();
        data.ScriptName = buffer.ReadString();
        data.Weight = buffer.ReadInt32();
        data.NodeRect.x = buffer.ReadFloat();
        data.NodeRect.y = buffer.ReadFloat();
        int count = buffer.ReadInt32();
        for (int i = 0; i < count; ++i)
        {
            data.AddNode(CreateFromBinary(buffer.ReadBytes()));
        }
        return data;
    }

    public static byte[] CreateInBinary(NodeGraph data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteString(data.Name);
        buffer.WriteByte((byte)data.Type);
        buffer.WriteString(data.ScriptName);
        buffer.WriteInt32(data.Weight);
        buffer.WriteFloat(data.NodeRect.x);
        buffer.WriteFloat(data.NodeRect.y);
        buffer.WriteInt32(data.Nodes.Count);
        for (int i = 0; i < data.Nodes.Count; ++i)
        {
            buffer.WriteBytes(CreateInBinary(data.Nodes[i]));
        }
        return buffer.Getbuffer();
    }

}

