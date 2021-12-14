using Godot;
using System;

public class PostCombatScene : MyControl
{
    [Export]
    private NodePath animationNodePath;
    [Export]
    private NodePath labelNodePath;
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
        GetNode<Control>("MinorTransition").Visible = true;
        GetNode<AnimationPlayer>(animationNodePath).Play("show");
    }


    public void SetText(String text)
    {
        GetNode<Label>(labelNodePath).Text = text;
    }

    public void SetTexture(Texture texture)
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
