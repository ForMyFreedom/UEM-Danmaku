using Godot;
using System;

public class PointsCounter : AbstractCounter
{
    private int maxPoints;
    private int points;

    public override void StartUp(Game game)
    {
        game.Connect("enemy_died", this, "_OnEnemyKilled");
        maxPoints = 30; //@
    }

    public override void _Ready()
    {
        points = 0;
    }

    private void _OnEnemyKilled()
    {
        points++;
        ActualizeLabel();
    }

    public override void ShrinkPontuation()
    {
        points -= 5;
        ActualizeLabel();
    }



    public override bool IsWinCondition()
    {
        return points >= maxPoints;
    }



    private void ActualizeLabel()
    {
        GetNode<Label>("Label").Text = GetBaseText() + points;
    }


    public override string GetBaseText()
    {
        return "Pontos: ";
    }
}
