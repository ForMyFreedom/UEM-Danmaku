using Godot;
using System;

public abstract class MyGameScene : Node
{
    [Export]
    protected PackedScene originScene;

    protected MyControl currentScene;


    public void Load(NodePath parent, ScenesDataCross dataCross)
    {
        currentScene = originScene.Instance<MyControl>();
        PassDataToScene(currentScene, dataCross);
        currentScene.Connect("scene_end", this, "_OnSceneEnd");
        GetNode(parent).AddChild(currentScene);
    }

    protected abstract void PassDataToScene(Control scene, ScenesDataCross dataCross);


    protected void _OnSceneEnd()
    {
        GetParent<ScenesLoader>()._OnPassToNextScene(currentScene.GetDataCross());
    }

}
