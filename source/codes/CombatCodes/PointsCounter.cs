using Godot;
using System;

public class PointsCounter : AbstractCounter
{
    private int maxPoints;
    private int points;

    public override void StartUp(Game game)
    {
        game.Connect("enemy_died", this, "_OnEnemyKilled");
    }

    public override void _Ready()
    {
        maxPoints = 30;
        points = 0;
        baseText = "Pontos: ";
    }

    private void _OnEnemyKilled()
    {
        points++;
        GetNode<Label>("Label").Text = baseText + points;
    }

    public override bool IsWinCondition()
    {
        return points >= maxPoints;
    }
}
