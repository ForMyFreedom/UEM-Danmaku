using Godot;
using Godot.Collections;
using System;

public class GameSceneCombat : MyGameScene
{
    [Export]
    private GameEnums.GAME_TYPE gameType;

    protected override void PassDataToScene(Control baseScene, ScenesDataCross dataCross)
    {
        MasterGame game = (MasterGame) baseScene;
        game.SetGameType(gameType);
    }
}
