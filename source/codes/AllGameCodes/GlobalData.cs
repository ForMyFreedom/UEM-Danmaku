using Godot;
using System;

public class GlobalData : Node
{
    private ScenesDataCross currentDataCross;
    private int FarestFase = 0;

    public void PassDataCross(ScenesDataCross data)
    {
        currentDataCross = data;
    }

    public int GetFarestFase()
    {
        return FarestFase;
    }

    public void SetFarestFase(int index)
    {
        FarestFase = index;
    }

}