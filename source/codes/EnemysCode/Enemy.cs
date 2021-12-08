using Godot;
using System;

public class Enemy : Node2D
{
    [Export]
    protected float velocity;
    [Signal]
    public delegate void death_signal();

    protected Node2D entityToFollow;
    protected Random random;

    public override void _Ready()
    {
        random = new Random();
        GetNode<Area2D>("Area").Connect("area_entered", this, "_OnAreaEntered");
        SetSpawnTimer();
        GetNode<AnimationPlayer>("Animation").Play("spawn_couldown");
    }


    public override void _Process(float delta)
    {
        if (IsInstanceValid(entityToFollow))
            FollowTheEntity();
        else
            RunRandomly();
    }



    protected virtual void FollowTheEntity()
    {
        MoveEntity(GetDistanceProportionToPlayer());
    }

    
    
    protected Vector2 GetDistanceProportionToPlayer()
    {
        Vector2 entityPos = entityToFollow.GlobalPosition;
        Vector2 myPos = GlobalPosition;
        return (entityPos - myPos).Normalized();
    }


    protected void MoveEntity(Vector2 distanceProportion)
    {
        Position += distanceProportion * velocity;
    }


    protected void RunRandomly()
    {
        Position += GetRandomDistance() * velocity;
    }



    protected Vector2 GetRandomDistance()
    {
        float xMove = (float) (random.Next(-1000, 1000)/1000.0);
        float yMove = (random.Next(0,2)==1) ? (1 - xMove) : (xMove-1);
        return new Vector2(xMove, yMove);
    }

    protected virtual void StartDeath()
    {
        EmitSignal(nameof(death_signal));
        GetNode<AnimationPlayer>("Animation").Play("death");
    }

    protected virtual void PlayDeathSound()
    {
        int quantSounds = GetNode<MultipleStreamPlayer>("DeathAudioPlayer").GetQuantOfAudios() + 1;
        GetNode<MultipleStreamPlayer>("DeathAudioPlayer").PlayAudioByName(random.Next(1, quantSounds).ToString());
    }


    protected virtual void _OnAreaEntered(Area2D area)
    {
        if (area.GetCollisionLayerBit((int)GameEnums.LAYERS.Invocation))
        {
            StartDeath();
        }
    }

    protected void SetSpawnTimer()
    {
        GetNode<Timer>("SpawnTimer").Connect("timeout", this, "_OnSpawnTimerOut");
        GetNode<Timer>("SpawnTimer").Start();
    }

    protected void _OnSpawnTimerOut()
    {
        GetNode<AnimationPlayer>("Animation").Seek(0, true);
        GetNode<AnimationPlayer>("Animation").Stop();

        GetNode<Area2D>("Area").Monitorable = true;
        GetNode<Area2D>("Area").Monitoring = true;
    }


    public void SetEntityToFollow(Node2D entity)
    {
        entityToFollow = entity;
    }

}
