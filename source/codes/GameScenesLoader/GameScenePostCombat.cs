using Godot;
using System;

public class GameScenePostCombat : MyGameScene
{
    [Export]
    private PackedScene failureScene;

    protected override void PassDataToScene(Control scene, ScenesDataCross dataCross)
    {
        if (dataCross.GetDataLine<bool>("win") != true)
            currentScene = failureScene.Instance<MyControl>();
    }
}
