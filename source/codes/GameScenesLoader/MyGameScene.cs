using Godot;
using System;

public abstract class MyGameScene : Node
{
    [Export]
    protected PackedScene originScene;

    protected MyControl myScene;


    public void Load(NodePath currentSceneNodePath, ScenesDataCross dataCross)
    {
        myScene = originScene.Instance<MyControl>();
        PassDataToScene(myScene, dataCross);
        myScene.Connect("scene_end", this, "_OnSceneEnd");
        GetNode(currentSceneNodePath).AddChild(myScene);
    }

    protected abstract void PassDataToScene(Control scene, ScenesDataCross dataCross);


    protected void _OnSceneEnd()
    {
        GetParent<ScenesLoader>()._OnPassToNextScene(myScene.GetDataCross());
    }

}
