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
        if(IsInstanceValid(entityToFollow)) distanceProportion = GetDistanceProportionToPlayer();
        distanceProportion = GetDistanceProportionToPlayer();
        GetNode<Timer>(walkThroughLineTimerPath).Start();
    }


    protected override void MoveProcess()
    {
        if (IsInstanceValid(entityToFollow))
            MakeAlterMove();
        else
            MoveEntity(GetRandomProportion());
    }


    private void MakeAlterMove()
    {
        try
        {
            if (GetNode<Timer>(walkThroughLineTimerPath).IsStopped())
            {
                distanceProportion = GetDistanceProportionToPlayer();
                GetNode<Timer>(walkThroughLineTimerPath).WaitTime = GetRandomWaitTime();
                GetNode<Timer>(walkThroughLineTimerPath).Start();
            }
            else
            {
                MoveEntity(distanceProportion);
            }
        }
        catch (Exception)
        {
            MoveEntity(GetRandomProportion());
        }

    }



    private float GetRandomWaitTime()
    {
        return random.Next(400, 1300) / 1000.0f;
    }


}
