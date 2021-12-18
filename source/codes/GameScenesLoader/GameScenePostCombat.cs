using Godot;
using System;

public class GameScenePostCombat : MyGameScene
{
    [Export]
    private PackedScene failureScene;
    [Export]
    private PackedScene minorTransitionScene;
    [Export]
    private String majorText;
    [Export]
    private String minorText;
    [Export]
    private Texture newPowerTexture;

    protected override void PassDataToScene(Control scene, ScenesDataCross dataCross)
    {
        if (dataCross.GetDataLine<bool>("win") == true)
        {
            myScene = minorTransitionScene.Instance<MyControl>();
            BaseTransition transition = (BaseTransition) myScene;
            transition.SetMajorText(majorText);
            transition.SetMinorText(minorText);
            transition.SetTexture(newPowerTexture);
        }
        else
            myScene = failureScene.Instance<MyControl>();
    }
}
