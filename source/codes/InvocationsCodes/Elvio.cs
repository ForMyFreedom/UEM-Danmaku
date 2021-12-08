using Godot;
using System;

public class Elvio : Invocation
{
    public override void _Ready()
    {
        base._Ready();
        actionsOrder = new[] { GameEnums.INVOCATION_ACTIONS.Create };
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }


    public override void PerformAction() { }

    protected override void OnAnimationEnd(String aniName)
    {
        if (aniName == "spawn")
        {
            GetNode<Area2D>("Area").Monitoring = true;
            GetNode<Area2D>("Area").Monitorable = true;
        }
    }
}
