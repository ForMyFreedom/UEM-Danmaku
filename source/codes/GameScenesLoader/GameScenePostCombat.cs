using Godot;
using System;

public class GameScenePostCombat : MyGameScene
{
    [Export]
    private PackedScene failureScene;
    [Export]
    private PackedScene minorTransitionScene;
    [Export]
    private String transitionText;
    [Export]
    private Texture newPowerTexture;

    protected override void PassDataToScene(Control scene, ScenesDataCross dataCross)
    {
        if (dataCross.GetDataLine<bool>("win") == true)
        {
            myScene = minorTransitionScene.Instance<MyControl>();
            PostCombatScene transition = (PostCombatScene) myScene;
            transition.SetText(transitionText);
            transition.SetTexture(newPowerTexture);
        }
        else
            myScene = failureScene.Instance<MyControl>();
    }
}
