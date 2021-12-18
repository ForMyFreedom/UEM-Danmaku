using Godot;
using Godot.Collections;
using System;

public class Transition : BaseTransition
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

    

    public override void SetMajorText(String text)
    {
        GetNode<Label>(majorTextNodePath).Text = text;
    }

    public override void SetMinorText(String text)
    {
        GetNode<Label>(minorTextNodePath).Text = text;
    }

    public override void SetTexture(Texture texture) { }


    private void _OnAnimationFinished(String aniName)
    {
        EmitSignal(nameof(scene_end));
    }


    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
