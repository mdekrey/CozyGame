using System.Collections.Generic;
using System.Threading.Tasks;
using CozyGame.Binding.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace CozyGame.Binding.Xna;

public class GamepadAxisBindingShould
{
    [Fact]
    public void HandleDefaultState()
    {
        var (parsedBindings, xCommand, _) = SetupExitCommand(GamepadAxis.LeftStickXAxis);
        parsedBindings.UpdateOutputs();

        // Assert
        var xResult = parsedBindings.GetOutput(0, xCommand);
        Assert.Equal(0, xResult);
    }

    [MemberData(nameof(GetSettings))]
    [Theory]
    public void HandleGamepadState(GamepadAxis axis, GamePadState gamePadState, float expected)
    {
        var (parsedBindings, xCommand, inputState) = SetupExitCommand(axis);
        UpdateGamepadState(parsedBindings, inputState, gamePadState);

        // Assert
        Assert.Equal(expected, parsedBindings.GetOutput(0, xCommand));
    }

    public static IEnumerable<object[]> GetSettings()
    {
        yield return new object[] { GamepadAxis.LeftStickXAxis, new GamePadState(new Vector2(1, 0), Vector2.Zero, 0, 0, (Buttons)0), 1f };
        yield return new object[] { GamepadAxis.LeftStickXAxis, new GamePadState(new Vector2(-1, 0), Vector2.Zero, 0, 0, (Buttons)0), -1f };
        yield return new object[] { GamepadAxis.LeftStickYAxis, new GamePadState(new Vector2(0, 1), Vector2.Zero, 0, 0, (Buttons)0), 1f };
        yield return new object[] { GamepadAxis.LeftStickYAxis, new GamePadState(new Vector2(0, -1), Vector2.Zero, 0, 0, (Buttons)0), -1f };

        yield return new object[] { GamepadAxis.RightStickXAxis, new GamePadState(Vector2.Zero, new Vector2(1, 0), 0, 0, (Buttons)0), 1f };
        yield return new object[] { GamepadAxis.RightStickXAxis, new GamePadState(Vector2.Zero, new Vector2(-1, 0), 0, 0, (Buttons)0), -1f };
        yield return new object[] { GamepadAxis.RightStickYAxis, new GamePadState(Vector2.Zero, new Vector2(0, 1), 0, 0, (Buttons)0), 1f };
        yield return new object[] { GamepadAxis.RightStickYAxis, new GamePadState(Vector2.Zero, new Vector2(0, -1), 0, 0, (Buttons)0), -1f };

        yield return new object[] { GamepadAxis.LeftTrigger, new GamePadState(Vector2.Zero, Vector2.Zero, 1, 0, (Buttons)0), 1f };

        yield return new object[] { GamepadAxis.RightTrigger, new GamePadState(Vector2.Zero, Vector2.Zero, 0, 1, (Buttons)0), 1f };
    }

    private static void UpdateGamepadState(Bindings parsedBindings, InputStateStubs inputState, GamePadState gamepad)
    {
        inputState.GamePadState[0] = gamepad;
        inputState.Update();
        parsedBindings.UpdateOutputs();
    }

    private static (Bindings parsedBindings, NumberCommand xCommand, InputStateStubs inputState) SetupExitCommand(GamepadAxis axis)
    {
        // Arrange
        var xCommand = new NumberCommand("x");
        var inputStateStubs = new InputStateStubs(2);
        var inputState = inputStateStubs.InputState;

        // Act
        var parsedBindings = new Bindings(new() { [xCommand] = new GamepadAxisBinding(axis) }, inputState);

        return (parsedBindings, xCommand, inputStateStubs);
    }
}
