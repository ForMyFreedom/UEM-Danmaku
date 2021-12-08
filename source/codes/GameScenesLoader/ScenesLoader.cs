using Godot;
using System;

public class ScenesLoader : Node
{
    [Export]
    private NodePath currentSceneNodePath;
    [Export]
    private NodePath globalDataNodePath;

    private int sceneIndex;

    public override void _Ready()
    {
        sceneIndex = 0;
        
        _OnPassToNextScene(new BlankDataCross());
    }


    public void _OnPassToNextScene(ScenesDataCross dataCross)
    {
        if (!SceneIndexIsLimiar()) GetNode(currentSceneNodePath).GetChild(0).QueueFree();
        PassDataCrossToGlobalData(dataCross);
        GetChild<MyGameScene>(sceneIndex).Load(ToAbsolute(currentSceneNodePath), dataCross);
        sceneIndex++;
    }

    private void PassDataCrossToGlobalData(ScenesDataCross dataCross)
    {
        GetNode<GlobalData>(globalDataNodePath).PassDataCross(dataCross);
    }



    private bool SceneIndexIsLimiar()
    {
        return sceneIndex == 0 || sceneIndex == GetChildCount()-1;
    }


    private NodePath ToAbsolute(NodePath nodePath)
    {
        return "/root/AllGame/CurrentScene";
    }


    public void SetSceneIndex(int id)
    {
        sceneIndex = id;
    }

}
