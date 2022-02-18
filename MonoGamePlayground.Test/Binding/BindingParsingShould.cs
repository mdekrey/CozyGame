
using System.Threading.Tasks;
using Xunit;

namespace MonoGamePlayground.Binding;

public class BindingParsingShould
{
    [Fact]
    public async Task HandleASingleEvent()
    {
        // Arrange
        var target = new BindingParsing();
        var script = @"
            exit:
                combineToBoolean:
                    - '(a, b) => a || b'
                    - gamepadButton: Back
                    - keyboard: Escape
        ";

        // Act
        var parsedBindings = await target.Parse(script);

        // Assert
        var exitResult = parsedBindings[new BooleanStateCommand("exit")];
        Assert.NotNull(exitResult);
        Assert.True(false); // TODO

    }
}
