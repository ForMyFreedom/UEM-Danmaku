using Godot;
using System;

public class ExaustiveWork : Enemy
{
    private int lifeChuncks = 2;

    public override void _Ready()
    {
        base._Ready();
    }

    protected override void _OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Invocation))
            lifeChuncks--;
        else
            return;

        if (lifeChuncks == 0)
            StartDeath();
        else
            GetNode<AnimationPlayer>("Animation").Play("hitted");
    }
}
