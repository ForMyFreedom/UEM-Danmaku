using Godot;
using System;

public class AllGame : Control
{
    [Export]
    private NodePath loaderNodePath;

    public void ReturnToMainMenu()
    {
        GetGlobalData().SetFarestFase(GetNode<ScenesLoader>("ScenesLoader").sceneIndex-2);
        GetNode("CurrentScene").GetChild(0).QueueFree();
        GetNode<ScenesLoader>(loaderNodePath).SetSceneIndex(0);
    }

    public void SetNextSceneIndex(int index)
    {
        GetNode<ScenesLoader>(loaderNodePath).SetSceneIndex(index);
    }

    public GlobalData GetGlobalData()
    {
        return GetNode<GlobalData>("GlobalData");
    }
}
