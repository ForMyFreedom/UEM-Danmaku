using Godot;
using System;

public class ContinuosAudioPlayer : MyAudioStreamPlayer
{
    public override void _Ready()
    {
        Connect("finished", this, "_OnFinished");
    }

    private void _OnFinished()
    {
        Play();
    }
}
