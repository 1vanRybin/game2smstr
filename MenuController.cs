using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using SharpDX.XInput;

namespace MazeEscape;

public static class MenuController
{
    static bool menuChecker;

    static Dictionary<Keys, Action> controls = new () 
    { 
        { Keys.Down, ()=> ChangeState(MenuState.Exit, MenuState.StartGame, 1) },
        { Keys.Up, ()=> ChangeState(MenuState.StartGame, MenuState.Exit, -1) }
    };

    static Dictionary<MenuState, Mode> modes = new()
    {
        { MenuState.StartGame, Mode.Game},
        { MenuState.Rules, Mode.Rules},
        { MenuState.Exit, Mode.Exit}
    };
    public static MenuState State { get; private set; }


    static public void Control()
    {
        KeyboardState keyState = Keyboard.GetState();
        var pressedKey = keyState.GetPressedKeys().FirstOrDefault();

        if(controls.ContainsKey(pressedKey))
            controls[pressedKey].Invoke();

        else
            menuChecker = true;

        if (keyState.IsKeyDown(Keys.Enter) && modes.ContainsKey(State))
            MazeEscape.Mode = modes[State];
    }

    private static void ChangeState(MenuState from, MenuState to, int direction)
    {
        if (menuChecker)
        {
            menuChecker = false;
            State = (State == from) ? to : State + direction;
        }
    }
}
