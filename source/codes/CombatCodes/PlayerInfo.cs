using Godot;
using Godot.Collections;
using System;

public class PlayerInfo : Control
{
    [Export]
    private Array<Array<Texture>> fusesTextures = new Array<Array<Texture>>();

    [Export]
    private NodePath LifeBoxContainer;

    public override void _Ready()
    {
        
    }

    public void LostAFuse(int playerQuantOfLives)
    {
        GetFuseByIndex(playerQuantOfLives).Texture = fusesTextures[playerQuantOfLives][1];
    }


    public TextureRect GetFuseByIndex(int id)
    {
        return GetNode(LifeBoxContainer).GetChild<TextureRect>(id);
    }
}
