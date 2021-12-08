using Godot;
using Godot.Collections;
using System;

public class Transition : MyControl
{
    [Export]
    NodePath majorTextNodePath;
    [Export]
    NodePath minorTextNodePath;

    Random random;

    public void SetMajorText(String text)
    {
        GetNode<Label>(majorTextNodePath).Text = text;
    }

    public void SetMinorText(String text)
    {
        GetNode<Label>(minorTextNodePath).Text = text;
    }



    public override void _Ready()
    {
        GetNode<AnimationPlayer>("Animation").Connect("animation_finished", this, "_OnAnimationFinished");
    }

    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }

    private void _OnAnimationFinished(String aniName)
    {
        EmitSignal("scene_end");
    }

}
