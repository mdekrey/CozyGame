using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace CozyGame.Binding;

public class ScriptedLambdaShould
{
    [Fact]
    public async Task GenerateALambda()
    {
        // Arrange
        var target = new ScriptedLambda();
        var script = "v => v + 1";

        // Act
        var actual = await target.EvaluateAsync<Func<float, float>>(script);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(1, actual?.Invoke(0));
        Assert.Equal(2, actual?.Invoke(1));
        Assert.Equal(2.5f, actual?.Invoke(1.5f));
    }

    [Fact]
    public async Task GenerateAWhenTypeIsAVariable()
    {
        // Arrange
        var target = new ScriptedLambda();
        var script = "v => v + 1";

        // Act
        var actual = (Func<float, float>?)await target.EvaluateAsync(script, typeof(Func<float, float>));

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(1, actual?.Invoke(0));
        Assert.Equal(2, actual?.Invoke(1));
        Assert.Equal(2.5f, actual?.Invoke(1.5f));
    }

    [InlineData(typeof(Program))]
    [InlineData(typeof(Process))]
    [Theory]
    public async Task NotBeAbleToAccessSomeTypes(Type targetType)
    {
        // Arrange
        var target = new ScriptedLambda();

        // Act / Assert
        await Assert.ThrowsAsync<CompilationErrorException>(async () => await target.EvaluateAsync<Type>($"typeof({targetType.FullName})"));
    }

    [InlineData(typeof(Math))]
    [Theory]
    public async Task BeAbleToAccessSomeTypes(Type targetType)
    {
        // Arrange
        var target = new ScriptedLambda();

        // Act
        var type = await target.EvaluateAsync<Type>($"typeof({targetType.FullName})");

        // Assert
        Assert.NotNull(type);
    }
}