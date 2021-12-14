using Godot;
using Godot.Collections;
using System.Linq;
using System;

public class Player : Node2D
{
    [Export]
    private Array<String> invocationsKeys = new Array<string>();
    [Export]
    private Array<PackedScene> invocationsScenes = new Array<PackedScene>();
    [Export]
    private int velocity;
    [Export]
    private Array<Vector2> limits = new Array<Vector2>() {Vector2.Zero, Vector2.Zero};
    [Export]
    private bool DEBUG_GOD_MODE=false; //DEBUG

    private int VELOCITY_CONST;

    private Dictionary<String, Vector2> DIRECTIONS;
    private Array<String> SPRITE_ORIENTATION;

    private Array<Invocation> invocationsNodesArray;
    private Array<GameEnums.INVOCATION_ACTIONS> invocationsActionsArray;
    private Array<int> invocationsActionIndexArray;
    
    private bool invulnerable = false;

    private Random random;


    public override void _Ready()
    {
        VELOCITY_CONST = velocity;

        DIRECTIONS = GameEnums.GetDirectionsVectors();

        SPRITE_ORIENTATION = new Array<String> { ".", "Right", "Left", "Up", "Down" };
        
        random = new Random();

        if (!DEBUG_GOD_MODE) //DEBUG
            GetNode<Area2D>("Area").Connect("area_entered", this, "_OnAreaEntered");

        CreateAllAttackTimers();
        ConnectAllGeneralTimers();

        SetDefaultInvocationsNodes();
        SetDefaultInvocationsActions();
        SetDefaultInvocationsIndexs();

        PlayInvulnerable();
    }



    public override void _Process(float delta)
    {
        DoMoveProcess();
        DoCrouchProcess();
        DoInvocationProcess();
    }


    public void DoMoveProcess()
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


    public void DoCrouchProcess()
    {
        if (Input.IsActionPressed("Shift"))
            velocity = VELOCITY_CONST / 2;
        else if (Input.IsActionJustReleased("Shift"))
            velocity = VELOCITY_CONST;
    }


    private void DoInvocationProcess()
    {
        for (int i=0 ; i < invocationsKeys.Count ; i++)
        {
            if (invocationsScenes.ToList().ElementAtOrDefault(i) == null) return;
            if (Input.IsActionPressed(invocationsKeys[i]) &&
                GetNode<Timer>($"AttackTimers/Timer{i}").IsStopped())
            {
                PerformInvocationAction(i);
                ChangeInvocationAction(i);
                AlterateTimer(i);
                StartTheTimer(i);
            }
        }
    }


    private bool PassThroughALimit(Vector2 positionMod)
    {
        if ( Position[0] + positionMod[0] < limits[0][0] ||
             Position[0] + positionMod[0] > limits[0][1] ||
             Position[1] + positionMod[1] < limits[1][0] ||
             Position[1] + positionMod[1] > limits[1][1])
        {
            return true;
        }
        return false;
    }


    private void PerformInvocationAction(int i)
    {
        if (invocationsActionsArray[i] == GameEnums.INVOCATION_ACTIONS.Create)
            MakeInvocation(i);
        if (invocationsActionsArray[i] == GameEnums.INVOCATION_ACTIONS.Cast)
            CastCustomAction(i);
    }


    private void ChangeInvocationAction(int i)
    {
        Invocation invocation = invocationsNodesArray[i];
        invocationsActionIndexArray[i] = invocation.GetNextIndex(invocationsActionIndexArray[i]);
        invocationsActionsArray[i] = invocation.GetNextActionType(invocationsActionIndexArray[i]);
    }


    private void AlterateTimer(int i)
    {
        Invocation invocation = invocationsNodesArray[i];
        GetNode<Timer>($"AttackTimers/Timer{i}").WaitTime = invocation.GetInvocationTime();
    }


    private void StartTheTimer(int i)
    {
        GetNode<Timer>($"AttackTimers/Timer{i}").Start();
    }




    private void MakeInvocation(int i)
    {
        Invocation newInvocation = invocationsScenes[i].Instance<Invocation>();
        newInvocation.Position = newInvocation.GetRandomInvocationPosition();
        GetNode("Invocations").AddChild(newInvocation);
        invocationsNodesArray[i] = newInvocation;
    }


    private void CastCustomAction(int i)
    {
        Invocation invocation = invocationsNodesArray[i];
        invocation.PerformAction();
    }


    public void SetNewInvocation(String key, PackedScene invocation)
    {
        invocationsKeys.Add(key);
        invocationsScenes.Add(invocation);
    }


    private void _OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Enemy) && !invulnerable)
            QueueFree();
    }

    private void _OnInvulTimerTimeout()
    {
        invulnerable = false;
        GetNode<AnimationPlayer>("Animation").Stop();
    }



    private void CreateAllAttackTimers()
    {
        for(int i=0; i< invocationsScenes.Count ; i++)
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




    public void SetDefaultInvocationsActions()
    {
        invocationsActionsArray = new Array<GameEnums.INVOCATION_ACTIONS>();
        for (int i = 0 ; i < invocationsScenes.Count ; i++)
        {
            invocationsActionsArray.Add(GameEnums.INVOCATION_ACTIONS.Create);
        }
    }


    public void SetDefaultInvocationsNodes()
    {
        invocationsNodesArray = new Array<Invocation>();
        for (int i = 0 ; i < invocationsScenes.Count ; i++)
        {
            invocationsNodesArray.Add(null);
        }
    }

    public void SetDefaultInvocationsIndexs()
    {
        invocationsActionIndexArray = new Array<int>();
        for(int i = 0 ; i < invocationsScenes.Count ; i++)
        {
            invocationsActionIndexArray.Add(0);
        }
    }




    public void SetLimitPair(Vector2 limitPair, int index)
    {
        limits[index] = limitPair;
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


    public void SetPackedSceneInvocations(PackedScene[] invocations)
    {
        this.invocationsScenes = new Array<PackedScene>(invocations);
    }
}
