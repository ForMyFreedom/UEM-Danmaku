using Godot;
using System;

public class MinorTransition : BaseTransition
{
    [Export]
    private bool autoPlay;
    [Export]
    private String text;

    public override void _Ready()
    {
        GetNode<Label>("Label").Text = text;
        if (autoPlay)
            Play();
    }

    public void Play()
    {
        GetNode<AnimationPlayer>("Animation").Play("show");
    }

    public override void SetMinorText(String s)
    {
        text = s;
    }


    public override void SetMajorText(String s) { }

    public override void SetTexture(Texture texture) { }



    public void QuitScene()
    {
        if(autoPlay)
            EmitSignal(nameof(scene_end));
    }
}
