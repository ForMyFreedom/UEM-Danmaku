using Godot;
using Godot.Collections;
using System;

public abstract class Invocation : Node2D
{
    [Export]
    protected Array<float> timeToActions;
    [Export]
    protected Array<GameEnums.INVOCATION_ACTIONS> actionsOrder;
    
    protected float rotationPerFrame = 3;
    protected int actionIndex = 0;
    protected Random random = new Random();

    public override void _Ready()
    {
        RotationDegrees = random.Next(0, 360);
        Visible = true;

        GetNode<MultipleStreamPlayer>("AudioPlayer").PlayAudioByIndex(actionIndex);
        GetNode<Area2D>("Area").Connect("area_entered", this, "OnAreaEntered");
        GetNode<AnimationPlayer>("Animation").Connect("animation_finished", this, "OnAnimationEnd");
        GetNode<AnimationPlayer>("Animation").Play("spawn");
    }


    public override void _Process(float delta)
    {
        RotationDegrees += rotationPerFrame;
    }


    protected void OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Enemy))
                QueueFree();
    }


    public abstract void PerformAction();
    
    
    protected virtual void OnAnimationEnd(String aniName)
    {
        if (aniName == "spawn")
        {
            GetNode<Area2D>("Area").Monitoring = true;
            GetNode<Area2D>("Area").Monitorable = true;
        }
    }


    public float GetInvocationTime()
    {
        return timeToActions[actionIndex];
    }
    

    public int GetNextIndex(int index)
    {
        GetNode<MultipleStreamPlayer>("AudioPlayer").PlayAudioByIndex(actionIndex);

        actionIndex = index+1;

        if (actionIndex > actionsOrder.Count - 1)
            actionIndex = 0;
        
        return actionIndex;
    }

    public GameEnums.INVOCATION_ACTIONS GetNextActionType(int index)
    {
        return actionsOrder[actionIndex];
    }


    public Vector2 GetRandomInvocationPosition()
    {
        return new Vector2(
            GetRandomOrientation() * GetRandomPosition(),
            GetRandomOrientation() * GetRandomPosition()
        );
    }


    protected int GetRandomOrientation()
    {
        return (random.Next(1, 3) == 1) ? 1 : -1;
    }

    protected int GetRandomPosition()
    {
        return random.Next(50, 200);
    }

}
