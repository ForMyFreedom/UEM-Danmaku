using Godot;
using Godot.Collections;
using System;

public abstract class MyControl: Control
{
    [Signal]
    public delegate void scene_end();

    public ScenesDataCross GetDataCross()
    {
        ScenesDataCross dataCross = GetDataCrossType();
        dataCross.SetAllData(this);
        return dataCross;
    }

    protected GlobalData GetGlobalData()
    {
        return GetParent().GetParent<AllGame>().GetGlobalData();
    }

    protected abstract ScenesDataCross GetDataCrossType();
}
