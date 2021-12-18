using Godot;
using System;

public class Game : Control
{
	[Export]
	private float REDUCTION_TIME_PER_ENEMY;
	[Export]
	private PackedScene[] enemyList;
	[Export]
	private float MINIMUM_ENEMY_SPAWN_RATE;
	[Export]
	private PackedScene timerCounterPacked;
	[Export]
	private PackedScene pointsCounterPacked;
	[Export]
	private NodePath victoryScreenNodePath;

	[Signal]
	public delegate void player_lost_life();
	[Signal]
	public delegate void player_win_game();
	[Signal]
	public delegate void enemy_died();
	[Signal]
	public delegate void player_come_back();

	Player player;
	private int points;
	Random random;
	public bool gameContinue;
	AbstractCounter counter;



	public override void _Ready()
	{
		GetTree().Connect("idle_frame", this, "DoTrueReady");
	}

	public void DoTrueReady()
    {
		SetDefaultData();
		GetTree().Disconnect("idle_frame", this, "DoTrueReady");
	}

	public void SetDefaultData()
    {
		gameContinue = true;
		player = GetNode<Player>("Player");
		random = new Random();
		SetLimitsOfScreenToPlayer();
	}


	public override void _Process(float delta)
    {
		if (gameContinue == true)
        {
			ConsiderateSummonAnEnemy();
			ConsiderateDeathOfPlayer();
			ConsiderateWinOfPlayer();
        }
	}	


	private void ConsiderateSummonAnEnemy()
    {
		if (GetNode<Timer>("EnemyTimer").IsStopped()) //talvez tirar isso e usar signal
		{
			SummonEnemy();
			RaiseDificultyOfTimer();
			GetNode<Timer>("EnemyTimer").Start();
		}
	}


	private void ConsiderateDeathOfPlayer()
    {
		if (GetNodeOrNull("Player") == null)
		{
			GetNode<MultipleStreamPlayer>("AudioPlayer").PlayAudioByName("game_lose");
			gameContinue = false;
			EmitSignal(nameof(player_lost_life));
		}
	}


	private void ConsiderateWinOfPlayer()
    {
		if (counter.IsWinCondition())
			PlayVictory();
	}


	private void SummonEnemy()
    {
		Enemy enemy = (Enemy) enemyList[random.Next(0, enemyList.Length)].Instance<Node2D>();
		enemy.Position = GetRandomEnemyPosition();
		enemy.Connect("death_signal", this, "_OnEnemyDeathSignal");
		enemy.SetEntityToFollow(player);
		GetNode("Enemys").AddChild(enemy);
    }


	private void RaiseDificultyOfTimer()
    {
		Timer timer = GetNode<Timer>("EnemyTimer");
		if (timer.WaitTime-REDUCTION_TIME_PER_ENEMY >= MINIMUM_ENEMY_SPAWN_RATE)
        {
			GetNode<Timer>("EnemyTimer").WaitTime -= REDUCTION_TIME_PER_ENEMY;
		}
	}

	private Vector2 GetRandomEnemyPosition()
    {
		return new Vector2(random.Next(0, (int)RectSize[0]), random.Next(0, (int)RectSize[1]));
    }

	private void _OnEnemyDeathSignal()
    {
		GetNode<MultipleStreamPlayer>("AudioPlayer").PlayAudioByName("enemy_death");
		EmitSignal(nameof(enemy_died));
    }

	private void PlayVictory()
    {
		EmitSignal(nameof(player_win_game));
		gameContinue = false;
		KillAllEnemys();
		GetNode<MultipleStreamPlayer>("AudioPlayer").PlayAudioByName("game_win");
		GetNode<VictoryScreen>(victoryScreenNodePath).ShowVictory();
    }


	private void KillAllEnemys()
    {
		Node enemys = GetNode("Enemys");
		foreach (Enemy child in enemys.GetChildren())
        {
			enemys.RemoveChild(child);
			child.QueueFree();
        }
    }

	private void SetLimitsOfScreenToPlayer()
    {
		Player player = GetNode<Player>("Player");

		player.SetLimitPair(new Vector2(25, RectSize.x-25), 0);
		player.SetLimitPair(new Vector2(10, RectSize.y-20), 1);
	}


	public void ResetAllEnemysEntityToPlayer(Player player)
	{
		foreach (Enemy enemy in GetNode("Enemys").GetChildren())
		{
			enemy.SetEntityToFollow(player);
		}
	}

	public void SetPoints(int p)
    {
		points = p;
    }

	public void SetGameType(GameEnums.GAME_TYPE type) //awfull...
    {
        switch (type)
        {
			case GameEnums.GAME_TYPE.Point:
				counter = pointsCounterPacked.Instance<AbstractCounter>();
				break;
			case GameEnums.GAME_TYPE.Time:
				counter = timerCounterPacked.Instance<AbstractCounter>();
				break;
        }

		counter.StartUp(this);
		AddChild(counter);
    }



	public void SetEnemyList(PackedScene[] enemyList)
    {
		this.enemyList = enemyList;
    }

	
	public void ShrinkPontuation()
    {
		counter.ShrinkPontuation();
    }

}
