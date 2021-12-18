using Godot;
using System;

public class PostCombatScene : BaseTransition
{
    [Export]
    private NodePath animationNodePath;
    [Export]
    private NodePath labelNodePath;
    [Export]
    private NodePath minorTransitionNodePath;
    [Export]
    private NodePath majorTransitionNodePath;
    [Export]
    private NodePath invocationTextureNodePath;

    public override void _Ready()
    {
        GetNode<Label>(labelNodePath).PercentVisible = 0;
        GetNode(animationNodePath).Connect("animation_finished", this, "_OnAnimationFinished");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("Enter"))
        {
            PlayMinorTransitionAnimation();
            SetProcess(false);
        }
    }


    public void PlayMinorTransitionAnimation()
    {
        GetNode<Control>(minorTransitionNodePath).Visible = true;
        GetNode<MinorTransition>(minorTransitionNodePath).Play();
    }


    public override void SetMinorText(String minorText)
    {
        GetNode<MinorTransition>(minorTransitionNodePath).SetMinorText(minorText);
    }

    public override void SetMajorText(String majorText)
    {
        majorText = majorText.Replace(".", "\n");
        GetNode<Label>(majorTransitionNodePath).Text = majorText;
    }


    public override void SetTexture(Texture texture)
    {
        GetNode<TextureRect>(invocationTextureNodePath).Texture = texture;
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
