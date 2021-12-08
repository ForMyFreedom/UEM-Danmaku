using Godot;
using System;

public class LowNet : Enemy
{
    [Export]
    private NodePath followTimerPath;

    Vector2 distanceProportion;

    public override void _Ready()
    {
        base._Ready();
        distanceProportion = GetDistanceProportionToPlayer();
        GetNode<Timer>(followTimerPath).Start();
    }


    protected override void FollowTheEntity()
    {
        if (!GetNode<Timer>(followTimerPath).IsStopped())
            MoveEntity(distanceProportion);
        else
        {
            GetNode<Timer>(followTimerPath).Start();
            distanceProportion = GetDistanceProportionToPlayer();
        }
    }


}
