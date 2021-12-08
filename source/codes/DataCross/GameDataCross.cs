using Godot;
using Godot.Collections;
using System;

public class GameDataCross : ScenesDataCross
{
    public GameDataCross() : base() { }

    public override void SetAllData(Node node)
    {
        MasterGame master = (MasterGame) node;
        SetDataLine("win", master.playerWin);
    }

}
