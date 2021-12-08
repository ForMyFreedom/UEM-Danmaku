using Godot;
using Godot.Collections;
using System;

public abstract class ScenesDataCross : Godot.Object
{
    protected Dictionary<string, object> data;

    public ScenesDataCross()
    {
        data = new Dictionary<string, object>();
    }

    public abstract void SetAllData(Node node);


    protected void SetDataLine(String str, System.Object obj)
    {
        data.Add(str, obj);
    }

    public Dictionary<String, System.Object> GetData()
    {
        return data;
    }

    public T GetDataLine<T>(String str)
    {
        object obj;
        var result = data.TryGetValue(str, out obj);
        return (T) obj;
    }  
}
