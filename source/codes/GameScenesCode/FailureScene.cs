using Godot;
using System;

public class FailureScene : MyControl
{
    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("Enter"))
        {
            GetParent().GetParent<AllGame>().ReturnToMainMenu();        //@
            EmitSignal(nameof(scene_end));
        }
    }

    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
