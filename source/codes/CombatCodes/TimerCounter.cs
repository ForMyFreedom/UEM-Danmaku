using Godot;
using System;

public class TimerCounter : AbstractCounter
{
    public override void StartUp(Game game)
    {
        game.Connect("player_lost_life", this, "_OnPlayerLostLife");
        game.Connect("player_come_back", this, "_OnPlayerComeBack");
    }


    public override void _Ready()
    {
        baseText = "Tempo: ";
    }


    public override void _Process(float delta)
    {
        GetNode<Label>("Label").Text = baseText + (int) GetNode<Timer>("Timer").TimeLeft;
    }


    public override bool IsWinCondition()
    {
        return GetNode<Timer>("Timer").TimeLeft <= 0;
    }



    public void _OnPlayerLostLife()
    {
        GetNode<Timer>("Timer").Paused = true;
    }


    public void _OnPlayerComeBack()
    {
        GetNode<Timer>("Timer").Paused = false;
    }

}
