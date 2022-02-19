using System;
using Microsoft.Xna.Framework.Input;

namespace MonoGamePlayground.Binding;

public class InputState
{
    public int MaximumPlayers { get; }
    private readonly Func<KeyboardState> getKeyboardState;
    private readonly Func<int, GamePadState> getGamePadState;
    private readonly Func<MouseState> getMouseState;

    private KeyboardState previousKeyboardState;
    private KeyboardState keyboardState;
    private GamePadState[] previousGamePadState;
    private GamePadState[] gamePadState;
    private MouseState previousMouseState;
    private MouseState mouseState;

    public InputState(int maximumGamepads) : this(maximumGamepads, Keyboard.GetState, GamePad.GetState, Mouse.GetState)
    {
    }

    public InputState(int maximumGamepads, Func<KeyboardState> getKeyboardState, Func<int, GamePadState> getGamePadState, Func<MouseState> getMouseState)
    {
        // TODO: This assumes every player has their own gamepad. Some may only have keyboard, etc.
        this.MaximumPlayers = maximumGamepads;
        this.getKeyboardState = getKeyboardState;
        this.getGamePadState = getGamePadState;
        this.getMouseState = getMouseState;

        previousGamePadState = new GamePadState[maximumGamepads];
        gamePadState = new GamePadState[maximumGamepads];
    }

    public KeyboardState GetPreviousKeyboardState() => previousKeyboardState;
    public KeyboardState GetKeyboardState() => keyboardState;
    public GamePadState GetPreviousGamePadState(int gamePadIndex) => previousGamePadState[gamePadIndex];
    public GamePadState GetGamePadState(int gamePadIndex) => gamePadState[gamePadIndex];
    public MouseState GetPreviousMouseState() => previousMouseState;
    public MouseState GetMouseState() => mouseState;

    public void Update()
    {
        // Copy current state to previous state
        previousKeyboardState = keyboardState;
        previousMouseState = mouseState;
        (previousGamePadState, gamePadState) = (gamePadState, previousGamePadState);

        // Get new state
        keyboardState = getKeyboardState();
        mouseState = getMouseState();
        for (var i = 0; i < MaximumPlayers; i++)
            gamePadState[i] = getGamePadState(i);
    }
}
