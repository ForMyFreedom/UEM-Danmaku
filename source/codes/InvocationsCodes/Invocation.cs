using Godot;
using System;

public abstract class Invocation : Node2D
{
    [Export]
    protected float timeSpawn;

    protected GameEnums.INVOCATION_ACTIONS[] actionsOrder;
    protected static int invocationActionIndex;
    
    public override void _Ready()
    {
        invocationActionIndex = -1;
        Random rgn = new Random();
        RotationDegrees = rgn.Next(0, 360);
        Visible = true;
        GetNode<Area2D>("Area").Connect("area_entered", this, "OnAreaEntered");
        GetNode<AnimationPlayer>("Animation").Connect("animation_finished", this, "OnAnimationEnd");
        GetNode<AnimationPlayer>("Animation").Play("spawn");
    }

    public override void _Process(float delta)
    {
        RotationDegrees+=3;
    }


    protected void OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Enemy))
        {
                QueueFree();
        }
    }

    public abstract void PerformAction();
    protected abstract void OnAnimationEnd(String aniName);

    public float GetInvocationTime()
    {
        return timeSpawn;
    }
    
    public GameEnums.INVOCATION_ACTIONS GetNextActionType()
    {
        invocationActionIndex += 1;
        if (invocationActionIndex > actionsOrder.Length - 1)
        {
            invocationActionIndex = 0;
        }

        return actionsOrder[invocationActionIndex];
    }

}
