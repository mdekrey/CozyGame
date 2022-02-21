using System.Threading.Tasks;
using CozyGame.Binding.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace CozyGame.Binding.Xna;

public class PerPlayerKeyboardBindingShould
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
    public async Task HandlePlayer1KeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Escape);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(0, exitCommand);
        Assert.True(exitResult);
    }

    [Fact]
    public async Task HandlePlayer2KeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Delete);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(1, exitCommand);
        Assert.True(exitResult);
    }

    [Fact]
    public async Task HandleSeparatePlayerKeyboardState()
    {
        var (parsedBindings, exitCommand, inputState) = await SetupExitCommand();
        inputState.KeyboardState = new KeyboardState(Keys.Escape);
        inputState.Update();
        parsedBindings.UpdateOutputs();

        // Assert
        var exitResult = parsedBindings.GetOutput(1, exitCommand);
        Assert.False(exitResult);
    }

    private static Task<(Bindings parsedBindings, BooleanStateCommand exitCommand, InputStateStubs inputState)> SetupExitCommand()
    {
        // Arrange
        var exitCommand = new BooleanStateCommand("exit");
        var inputStateStubs = new InputStateStubs(2);
        var inputState = inputStateStubs.InputState;

        // Act
        var parsedBindings = new Bindings(new() { [exitCommand] = new PerPlayerKeyboardBinding(new[] { Keys.Escape, Keys.Delete }) }, inputState);

        return Task.FromResult((parsedBindings, exitCommand, inputStateStubs));
    }
}
