using Godot;
using Godot.Collections;
using System;

public class MasterGame : MyControl     //@
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
    Random random;


    public void SetPlayer(PackedScene p)
    {
        playerPackedScene = p;
    }



    public override void _Ready()
    {
        random = new Random();
        GetNode(gamePath).AddChild(playerPackedScene.Instance());
        playerMaxLives = 3;
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

        if (Input.IsActionJustReleased("9"))
        {
            GetNode<Game>(gamePath).SetPoints(29);
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
        Player player = (Player)playerPackedScene.Instance();
        Game game = GetNode<Game>(gamePath);
        game.AddChild(player);
        game.SetDefaultData();
        game.ResetAllEnemysEntityToPlayer(player);
        SetRandomPositionToPlayer(player);
        game.EmitSignal("player_come_back");
        //LostPoints(); @
    }


    private void _OnPlayerLostLife()
    {
        playerQuantOfLives--;
        SetRegularDeath();
        if (playerQuantOfLives == 0)
            SetPermanentDeath();
    }

    private void _OnPlayerWinGame()
    {
        playerWin = true;
    }



    private void SetPermanentDeath()
    {
        EmitSignal("scene_end");
    }


    private void SetRegularDeath()
    {
        GetNode<PlayerInfo>(playerInfoPath).LostAFuse(playerQuantOfLives);
        PlayFuseCrackAnimation();
    }

    private void PlayFuseCrackAnimation()
    {
        AnimationPlayer aniPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Animation animation = aniPlayer.GetAnimation("fuse_crack");
        String path = aniPlayer.GetPathTo(GetNode<PlayerInfo>(playerInfoPath).GetFuseByIndex(playerQuantOfLives)) + ":modulate";
        animation.TrackSetPath(0, path.Right(3));

        aniPlayer.Play("fuse_crack");
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
}
