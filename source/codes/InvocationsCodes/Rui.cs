using Godot;
using System;

public class Rui : Invocation
{
    [Export]
    private float velocity;

    private bool wasCasted = false;
    private Vector2 moveVector;


    public override void _Ready()
    {
        base._Ready();
    }
    

    public override void _Process(float delta)
    {
        if (!wasCasted)
            base._Process(delta);
        else
            Position += moveVector * velocity;
    }


    public override void PerformAction()
    {
        CalculateMoveVector();
        RemoveItFromPlayersDomains();
        DoGeneralAlterations();
    }



    protected override void OnAnimationEnd(String aniName) { }



    private void CalculateMoveVector()
    {
        float rad = RotationDegrees*0.0174533f;
        moveVector = new Vector2((float)-Math.Sin(rad), (float)Math.Cos(rad));
        Vector2 orientation = new Vector2((moveVector.x > 0) ? 1 : -1, (moveVector.y > 0) ? 1 : -1);

        moveVector  = moveVector * moveVector * orientation;
    }


    private void RemoveItFromPlayersDomains()
    {
        Vector2 position = GlobalPosition;
        SetAsToplevel(true);
        GlobalPosition = position;
    }

    
    private void DoGeneralAlterations()
    {
        Modulate = new Color(1, 1, 1, 1);
        GetNode<Area2D>("Area").Monitorable = true;
        wasCasted = true;
    }
}
