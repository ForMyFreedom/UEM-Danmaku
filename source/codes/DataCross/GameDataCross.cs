using Godot;
using Godot.Collections;
using System;

public class MenuDataCross : ScenesDataCross
{
    public MenuDataCross() : base() { }

    public override void SetAllData(Node node)
    {
        MainMenu menu = (MainMenu) node;
        SetDataLine("index", menu.GetNextSceneIndex());
    }

}
