using Godot;
using System;

public class StartPlayingAudioPlayer : MyAudioStreamPlayer
{
    public override void _Ready()
    {
        Play();
    }
}
