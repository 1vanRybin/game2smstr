using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MazeEscape;

public static class Controller
{
    static Dictionary<Keys, Vector2> MovePosition = new()
    {
        {Keys.W, new Vector2(0,-1)}, 
        {Keys.S, new Vector2(0,1)}, 
        {Keys.A, new Vector2(-1,0)}, 
        {Keys.D, new Vector2(1,0)}
    };
    static public void ControlPlayer(Player player)
    {
        var keyState = Keyboard.GetState();
        var pressedKey = keyState.GetPressedKeys().FirstOrDefault();
        if (MovePosition.ContainsKey(pressedKey))
        {
            player.Position += MovePosition[pressedKey];
        }
    }
}