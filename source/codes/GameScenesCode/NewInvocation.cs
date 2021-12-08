using Godot;
using System;

public class NewInvocation : MyControl
{
    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("Enter"))
        {
            EmitSignal("scene_end");
        }
    }

    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
