using Godot;
using System;

public class GlobalData : Node
{
    private ScenesDataCross currentDataCross;

    public void PassDataCross(ScenesDataCross data)
    {
        currentDataCross = data;
    }

}