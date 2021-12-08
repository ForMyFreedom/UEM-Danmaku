using Godot;
using Godot.Collections;
using System;

public class GameSceneTransition : MyGameScene
{
    [Export]
    private string majorText;
    [Export]
    private string minorText;

    protected override void PassDataToScene(Control baseScene, ScenesDataCross dataCross)
    {
        Transition scene = (Transition) baseScene;
        scene.SetMajorText(majorText);
        scene.SetMinorText(minorText);
    }
}
