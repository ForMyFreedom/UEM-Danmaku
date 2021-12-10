using Godot;
using Godot.Collections;
using System;

public class GameEnums
{
    public enum LAYERS { Player, Enemy, Invocation, Wall };
    public enum INVOCATION_ACTIONS { Create, Cast };
    public enum GAME_TYPE {Time, Point};



    public static Dictionary<string, Vector2> GetDirectionsVectors()
    {
        return new Dictionary<string, Vector2>()
        {
            {"Up", new Vector2(0,-1)},
            {"Down", new Vector2(0,1)},
            {"Left", new Vector2(-1,0)},
            {"Right", new Vector2(1,0)}
        };
    }
}
