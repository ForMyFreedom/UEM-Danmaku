using Godot;
using System;

public class MainMenu : MyControl
{
    [Export]
    private NodePath StartGameButtonPath;
    [Export]
    private NodePath DeturpationButtonPath;
    [Export]
    private NodePath ContinueButtonPath;

    private int nextSceneIndex;

    public override void _Ready()
    {
        nextSceneIndex = 1;
        GetNode(StartGameButtonPath).Connect("pressed", this, "_OnStartGamePressed");
        GetNode(DeturpationButtonPath).Connect("pressed", this, "_OnDeturpationPressed");
        GetNode(ContinueButtonPath).Connect("pressed", this, "_OnContinuePressed");
    }


    private void _OnStartGamePressed()
    {
        QuitMainMenu();
    }


    private void _OnDeturpationPressed()
    {
        GetNode<ContinuosAudioPlayer>("AudioPlayer").SetDeturpatedAudioFormat();
        //zoa mais essa porra
    }

    private void _OnContinuePressed()
    {
        int farestFase = GetGlobalData().GetFarestFase();
        if (farestFase == 0) return;
        nextSceneIndex = farestFase;
        QuitMainMenu();
    }


    private void QuitMainMenu()
    {
        GetParent().GetParent<AllGame>().SetNextSceneIndex(nextSceneIndex);
        EmitSignal(nameof(scene_end));
    }


    public int GetNextSceneIndex()
    {
        return nextSceneIndex;
    }



    protected override ScenesDataCross GetDataCrossType()
    {
        return new MenuDataCross();
    }

}
