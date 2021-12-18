using Godot;
using System;

public abstract class AbstractCounter : Control
{
    public abstract String GetBaseText();
    public abstract void StartUp(Game game);
    public abstract bool IsWinCondition();
    public abstract void ShrinkPontuation();
}
