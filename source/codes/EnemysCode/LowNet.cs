using Godot;
using System;

public class LowNet : Enemy
{
    [Export]
    private NodePath walkThroughLineTimerPath;

    Vector2 distanceProportion;

    public override void _Ready()
    {
        base._Ready();
        distanceProportion = GetDistanceProportionToPlayer();
        GetNode<Timer>(walkThroughLineTimerPath).Start();
    }


    protected override void FollowTheEntity()
    {
        if (GetNode<Timer>(walkThroughLineTimerPath).IsStopped())
        {
            distanceProportion = GetDistanceProportionToPlayer();
            GetNode<Timer>(walkThroughLineTimerPath).Start();
        }
        else
        {
            MoveEntity(distanceProportion);
        }
    }


}
