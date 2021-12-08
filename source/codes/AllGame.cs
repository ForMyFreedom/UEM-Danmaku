using Godot;
using System;

public class AllGame : Control
{
    [Export]
    private NodePath loaderNodePath;

    public void ReturnToMainMenu()
    {
        GetNode("CurrentScene").GetChild(0).QueueFree();
        GetNode<ScenesLoader>(loaderNodePath).SetSceneIndex(0);
    }
}
