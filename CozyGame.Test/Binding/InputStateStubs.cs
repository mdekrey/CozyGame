
using System;
using Microsoft.Xna.Framework.Input;
using Moq;

namespace CozyGame.Binding;

public class InputStateStubs
{
    public InputState InputState { get; }

    private readonly Mock<Stubs> mockInputState;

    public InputStateStubs(int maximumGamepads)
    {
        GamePadState = new GamePadState[maximumGamepads];
        mockInputState = new Mock<Stubs>(MockBehavior.Loose);
        mockInputState.Setup(s => s.GetKeyboardState()).Returns(() => KeyboardState);
        mockInputState.Setup(s => s.GetMouseState()).Returns(() => MouseState);
        mockInputState.Setup(s => s.GetGamePadState(It.IsAny<int>())).Returns((Func<int, GamePadState>)((int index) => GamePadState[index]));
        InputState = new InputState(maximumGamepads, mockInputState.Object.GetKeyboardState, mockInputState.Object.GetGamePadState, mockInputState.Object.GetMouseState);
    }


    public KeyboardState KeyboardState { get; set; }
    public GamePadState[] GamePadState { get; }
    public MouseState MouseState { get; set; }

    public void Update() => InputState.Update();

    public abstract class Stubs
    {
        public abstract KeyboardState GetKeyboardState();
        public abstract GamePadState GetGamePadState(int gamePadIndex);
        public abstract MouseState GetMouseState();
    }

}
