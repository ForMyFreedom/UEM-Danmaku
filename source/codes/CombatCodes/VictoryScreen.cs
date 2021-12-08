using Godot;
using System;

public class VictoryScreen : Control
{
    public void ShowVictory()
    {
        Visible = true;
        GetNode<AnimationPlayer>("Animation").Play("double_reveal");
    }
}
