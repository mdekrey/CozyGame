using System.Threading.Tasks;
using CozyGame.Binding.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace CozyGame.Binding.Xna;

public class GamepadButtonBindingShould
{
    [Fact]
    public async Task HandleDefaultState()
    {
        var (parsedBindings, exitCommand, _) = await SetupExitCommand();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(0, exitCommand);
        Assert.False(exitResult);
    }

    [Fact]
    public async Task HandlePlayer1GamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.GamePadState[0] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(0, exitCommand);
        Assert.True(exitResult);
    }

    [Fact]
    public async Task HandleSeparateGamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.GamePadState[0] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(1, exitCommand);
        Assert.False(exitResult);
    }

    [Fact]
    public async Task HandlePlayer2GamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.GamePadState[1] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(1, exitCommand);
        Assert.True(exitResult);
    }

    private static Task<(Bindings parsedBindings, BooleanStateCommand exitCommand, InputStateStubs inputState)> SetupExitCommand()
    {
        // Arrange
        var exitCommand = new BooleanStateCommand("exit");
        var inputStateStubs = new InputStateStubs(2);
        var inputState = inputStateStubs.InputState;

        // Act
        var parsedBindings = new Bindings(new() { [exitCommand] = new GamepadButtonBinding(Buttons.Back) }, inputState);

        return Task.FromResult((parsedBindings, exitCommand, inputStateStubs));
    }
}
