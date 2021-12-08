using Godot;
using System;

public abstract class AbstractCounter : Control
{
    protected String baseText;

    public abstract void StartUp(Game game);
    public abstract bool IsWinCondition();
}
