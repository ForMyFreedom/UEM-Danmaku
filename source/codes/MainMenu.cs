using Godot;
using System;

public class MainMenu : MyControl
{
    [Export]
    private NodePath StartGameButtonPath;
    [Export]
    private NodePath DeturpationButtonPath;

    public override void _Ready()
    {
        GetNode(StartGameButtonPath).Connect("pressed", this, "_OnStartGamePressed");
        GetNode(DeturpationButtonPath).Connect("pressed", this, "_OnDeturpationPressed");
    }


    private void _OnStartGamePressed()
    {
        EmitSignal("scene_end");
    }


    private void _OnDeturpationPressed()
    {
        GetNode<ContinuosAudioPlayer>("AudioPlayer").SetDeturpatedAudioFormat();
        //zoa mais essa porra
    }

    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
