using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace MonoGamePlayground.Binding;

public class BindingParsingShould
{
    [Fact]
    public async Task HandleDefaultState()
    {
        var (parsedBindings, exitCommand, _) = await SetupCombinedExitCommand();

        // Assert
        var exitResult = parsedBindings.GetValue(0, exitCommand);
        Assert.False(exitResult);
    }

    [Fact]
    public async Task HandlePlayer1KeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Escape);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(0, exitCommand);
        Assert.True(exitResult);
    }
    
    [Fact]
    public async Task HandlePlayer2KeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Delete);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(1, exitCommand);
        Assert.True(exitResult);
    }
    
    [Fact]
    public async Task HandleSeparatePlayerKeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Escape);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(1, exitCommand);
        Assert.False(exitResult);
    }

    [Fact]
    public async Task HandlePlayer1GamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.GamePadState[0] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(0, exitCommand);
        Assert.True(exitResult);
    }

    [Fact]
    public async Task HandleSeparateGamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.GamePadState[0] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(1, exitCommand);
        Assert.False(exitResult);
    }

    [Fact]
    public async Task HandlePlayer2GamepadState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupCombinedExitCommand();
        inputState.GamePadState[1] = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.Back);
        inputState.Update();

        // Assert
        var exitResult = parsedBindings.GetValue(1, exitCommand);
        Assert.True(exitResult);
    }

    private static async Task<(Bindings parsedBindings, BooleanStateCommand exitCommand, InputStateStubs inputState)> SetupCombinedExitCommand()
    {

        // Arrange
        var target = new BindingParsing();
        var script = @"
            exit:
                combineToBoolean:
                    - '(a, b) => a || b'
                    - gamepadButton: Back
                    - perPlayerKeyboard: 
                        - Escape
                        - Delete
        ";
        var exitCommand = new BooleanStateCommand("exit");
        var commandSet = new CommandSet(new[] { exitCommand });
        var inputStateStubs = new InputStateStubs(2);
        var inputState = inputStateStubs.InputState;

        // Act
        var parsedBindings = await target.Parse(script, commandSet, inputState);

        return (parsedBindings, exitCommand, inputStateStubs);
    }
}
