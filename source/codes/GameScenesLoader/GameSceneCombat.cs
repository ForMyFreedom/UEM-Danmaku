using Godot;
using Godot.Collections;
using System;

public class GameSceneCombat : MyGameScene
{
    [Export]
    private GameEnums.GAME_TYPE gameType;
    [Export]
    private PackedScene[] enemyList;
    [Export]
    private PackedScene[] invocationList;
    [Export]
    private Texture backgroundTexture;
    [Export]
    private AudioStream musicStream;


    protected override void PassDataToScene(Control baseScene, ScenesDataCross dataCross)
    {
        MasterGame game = (MasterGame) baseScene;
        game.SetGameType(gameType);
        game.SetEnemyList(enemyList);
        game.SetInvocationsList(invocationList);
        game.SetBackgroundTexture(backgroundTexture);
        game.SetMusicStream(musicStream);
    }
}
