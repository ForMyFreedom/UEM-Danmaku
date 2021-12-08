using Godot;
using Godot.Collections;
using System;

public class Player : Node2D
{
    [Export]
    private String[] invocationsKeys;
    [Export]
    private PackedScene[] invocationsScenes;
    [Export]
    private int velocity;
    [Export]
    private Array<Vector2> limits = new Array<Vector2>() {Vector2.Zero, Vector2.Zero};
    [Export]
    private bool DEBUG_GOD_MODE=false;

    private int VELOCITY_CONST;

    private Dictionary<String, Vector2> DIRECTIONS;
    private Array<String> SPRITE_ORIENTATION;
    private Array<Godot.Collections.Array> currentInvocationsAndActions;
    private bool invulnerable = false;

    private Random random;


    public override void _Ready()
    {
        VELOCITY_CONST = velocity;

        DIRECTIONS = new Dictionary<string, Vector2>()
        {
            {"Up", new Vector2(0,-1)},
            {"Down", new Vector2(0,1)},
            {"Left", new Vector2(-1,0)},
            {"Right", new Vector2(1,0)}
        };

        SPRITE_ORIENTATION = new Array<String> { ".", "Right", "Left", "Up", "Down" };
        random = new Random();

        if (!DEBUG_GOD_MODE) //@remove it later
            GetNode<Area2D>("Area").Connect("area_entered", this, "_OnAreaEntered");

        CreateAllAttackTimers();
        ConnectAllGeneralTimers();
        SetDefaultCurrentActions();

        PlayInvulnerable();
    }

    public override void _Process(float delta)
    {
        MoveProcess();
        CrouchProcess();
        InvocationProcess();
    }


    public void MoveProcess()
    {
        String lastDirectionMoved = ".";

        foreach (String directionName in DIRECTIONS.Keys)
        {
            Vector2 positionMod = DIRECTIONS[directionName] * velocity;
            if (Input.IsActionPressed(directionName) && !PassThroughALimit(positionMod))
            {
                Position += positionMod;
                lastDirectionMoved = directionName;
            }
        }

        GetNode<Sprite>("Sprite").Frame = SPRITE_ORIENTATION.IndexOf(lastDirectionMoved);
    }

    private bool PassThroughALimit(Vector2 positionMod)
    {
        if ( Position[0] + positionMod[0] < limits[0][0] ||
             Position[0] + positionMod[0] > limits[0][1] ||
             Position[1] + positionMod[1] < limits[1][0] ||
             Position[1] + positionMod[1] > limits[1][1] )
        {
            return true;
        }
        return false;
    }


    public void CrouchProcess()
    {
        if (Input.IsActionPressed("Shift"))
            velocity = VELOCITY_CONST / 2;
        else if (Input.IsActionJustReleased("Shift"))
            velocity = VELOCITY_CONST;
    }


    private void InvocationProcess()
    {
        for (int i=0 ; i < invocationsKeys.Length ; i++)
        {
            if (Input.IsActionPressed(invocationsKeys[i]) && GetNode<Timer>($"AttackTimers/Timer{i}").IsStopped())
            {
                PerformInvocationAction(i);
                AlterateTimer(i);
                ChangeInvocationAction(i);
                StartTheTimer(i);
            }
        }
    }

    private void PerformInvocationAction(int i)
    {
        if ((GameEnums.INVOCATION_ACTIONS) currentInvocationsAndActions[i][0] == GameEnums.INVOCATION_ACTIONS.Create)
            MakeInvocation(i);
        if ((GameEnums.INVOCATION_ACTIONS)currentInvocationsAndActions[i][0] == GameEnums.INVOCATION_ACTIONS.Cast)
            CastCustomAction(i);
    }


    private void AlterateTimer(int i)
    {
        Invocation invocation = (Invocation)currentInvocationsAndActions[i][1];
        GetNode<Timer>($"AttackTimers/Timer{i}").WaitTime = invocation.GetInvocationTime();
    }


    private void ChangeInvocationAction(int i)
    {
        Invocation invocation = (Invocation) currentInvocationsAndActions[i][1];
        currentInvocationsAndActions[i][0] = invocation.GetNextActionType();
    }



    private void MakeInvocation(int i)
    {
        Node2D newInvocation = invocationsScenes[i].Instance<Node2D>();
        newInvocation.Position = GetRandomInvocationPosition();
        GetNode("Invocations").AddChild(newInvocation);
        currentInvocationsAndActions[i][1] = newInvocation;
    }


    private void CastCustomAction(int i)
    {
        Invocation invocation = (Invocation) currentInvocationsAndActions[i][1];
        invocation.PerformAction();
    }

    private void StartTheTimer(int i)
    {
        GetNode<Timer>($"AttackTimers/Timer{i}").Start();
    }




    private Vector2 GetRandomInvocationPosition()
    {
        return new Vector2(
            GetRandomOrientation() * GetMediumRandomPosition(),
            GetRandomOrientation() * GetMediumRandomPosition()
        );
    }


    private void _OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Enemy) && !invulnerable)
            QueueFree();
    }



    private int GetRandomOrientation()
    {
        return (random.Next(1, 3) == 1) ? -1 : 1;
    }

    private int GetMediumRandomPosition()
    {
        return random.Next(50, 200);
    }


    public void SetLimitPair(Vector2 limitPair, int index)
    {
        limits[index] = limitPair;
    }


    public void SetDefaultCurrentActions()
    {
        currentInvocationsAndActions = new Array<Godot.Collections.Array>() {};

        for (int i=0; i < invocationsKeys.Length; i++)
        {
            currentInvocationsAndActions.Add(
                new Godot.Collections.Array() { GameEnums.INVOCATION_ACTIONS.Create, null }
            );
        }
    }

    private void CreateAllAttackTimers()
    {
        for(int i=0; i<invocationsScenes.Length ; i++)
        {
            Timer newTimer = new Timer();
            newTimer.Name = $"Timer{i}";
            newTimer.OneShot = true;
            GetNode<Node>("AttackTimers").AddChild(newTimer);
        }
    }

    private void ConnectAllGeneralTimers()
    {
        foreach(Timer timer in GetNode("GeneralTimers").GetChildren())
        {
            timer.Connect("timeout", this, $"_On{timer.Name}Timeout");
        }
    }



    public Array<Vector2> GetLimits()
    {
        return limits;
    }


    public void PlayInvulnerable()
    {
        invulnerable = true;
        GetNode<Timer>("GeneralTimers/InvulTimer").Start();
        GetNode<AnimationPlayer>("Animation").Play("invul");
    }


    private void _OnInvulTimerTimeout()
    {
        invulnerable = false;
        GetNode<Area2D>("Area").Monitoring = true;
        GetNode<AnimationPlayer>("Animation").Stop();
    }
}
