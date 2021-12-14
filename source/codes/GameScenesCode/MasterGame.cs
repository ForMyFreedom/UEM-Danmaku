using Godot;
using Godot.Collections;
using System;

public class MasterGame : MyControl
{
    [Export]
    NodePath gamePath;
    [Export]
    NodePath playerInfoPath;
    [Export]
    PackedScene playerPackedScene;

    [Export]
    private GameEnums.GAME_TYPE gameType;

    int playerMaxLives;
    int playerQuantOfLives;
    public bool playerWin = false;
    public PackedScene[] playerInvocations;
    Random random;


    public void SetPlayer(PackedScene p)
    {
        playerPackedScene = p;
    }



    public override void _Ready()
    {
        random = new Random();
        Player playerNode = PreparePlayer();
        GetNode(gamePath).AddChild(playerNode);
        playerMaxLives = 3;     //Maybe change in future
        playerQuantOfLives = playerMaxLives;
        GetNode(gamePath).Connect("player_lost_life", this, "_OnPlayerLostLife");
        GetNode(gamePath).Connect("player_win_game", this, "_OnPlayerWinGame");
        GetNode<Game>(gamePath).SetGameType(gameType);
    }

    public override void _Process(float delta)
    {
        if (!playerWin)
            InGameProcess();
        else
            PostGameProcess();
    }


    private void InGameProcess()
    {
        if (Input.IsActionJustReleased("R") && !GetNode<Game>(gamePath).gameContinue)
        {
            ConfigPlayerTryAgain();
        }
    }

    private void PostGameProcess()
    {
        if (Input.IsActionJustReleased("Enter"))
        {
            EmitSignal("scene_end");
        }
    }


    private void ConfigPlayerTryAgain()
    {
        Player player = PreparePlayer();
        Game game = GetNode<Game>(gamePath);
        game.AddChild(player);
        game.SetDefaultData();
        game.ResetAllEnemysEntityToPlayer(player);
        SetRandomPositionToPlayer(player);
        game.EmitSignal("player_come_back");
        game.ShrinkPontuation();
    }


    private void _OnPlayerLostLife()
    {
        playerQuantOfLives--;
        SetRegularDeath();
        if (playerQuantOfLives == 0)
            SetPermanentDeath();
    }


    private void SetRegularDeath()
    {
        AnimationPlayer aniPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        PrepareFuseCrackAnimation(aniPlayer);
        PlayFuseCrackAnimation(aniPlayer);
    }


    private void SetPermanentDeath()
    {
        EmitSignal("scene_end");
    }


    private void PrepareFuseCrackAnimation(AnimationPlayer aniPlayer)
    {
        GetNode<PlayerInfo>(playerInfoPath).LostAFuse(playerQuantOfLives);

        Animation animation = aniPlayer.GetAnimation("fuse_crack");
        TextureRect currentFuse = GetNode<PlayerInfo>(playerInfoPath).GetFuseByIndex(playerQuantOfLives);
        String path = aniPlayer.GetPathTo(currentFuse) + ":modulate";

        animation.TrackSetPath(0, path.Right(3));
    }


    private void PlayFuseCrackAnimation(AnimationPlayer aniPlayer)
    {
        aniPlayer.Play("fuse_crack");
    }



    private void _OnPlayerWinGame()
    {
        playerWin = true;
    }



    private void SetRandomPositionToPlayer(Player player)
    {
        Array<Vector2> limits = player.GetLimits();
        player.Position = new Vector2(
            random.Next((int)limits[0][0], (int)limits[0][1]),
            random.Next((int)limits[1][0], (int)limits[1][1])
        );
    }

    protected override ScenesDataCross GetDataCrossType()
    {
        return new GameDataCross();
    }


    public void SetGameType(GameEnums.GAME_TYPE type)
    {
        gameType = type;
    }

    public void SetEnemyList(PackedScene[] enemyList)
    {
        GetNode<Game>(gamePath).SetEnemyList(enemyList);
    }

    public void SetInvocationsList(PackedScene[] invocationsList)
    {
        this.playerInvocations = invocationsList;
    }


    private Player PreparePlayer()
    {
        Player player = playerPackedScene.Instance<Player>();
        player.SetPackedSceneInvocations(playerInvocations);
        return player;
    }
}
