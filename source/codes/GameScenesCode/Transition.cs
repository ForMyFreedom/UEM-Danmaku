using Godot;
using Godot.Collections;
using System;

public class Transition : MyControl
{
    [Export]
    NodePath majorTextNodePath;
    [Export]
    NodePath minorTextNodePath;


    public override void _Ready()
    {
        GetNode<Label>(majorTextNodePath).PercentVisible = 0;
        GetNode<Label>(minorTextNodePath).PercentVisible = 0;
        GetNode<AnimationPlayer>("Animation").Play("show");
        GetNode("Animation").Connect("animation_finished", this, "_OnAnimationFinished");
    }

    

    public void SetMajorText(String text)
    {
        GetNode<Label>(majorTextNodePath).Text = text;
    }

    public void SetMinorText(String text)
    {
        GetNode<Label>(minorTextNodePath).Text = text;
    }



    private void _OnAnimationFinished(String aniName)
    {
        EmitSignal(nameof(scene_end));
    }


    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
