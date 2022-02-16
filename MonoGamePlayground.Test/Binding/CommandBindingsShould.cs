using System;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace MonoGamePlayground.Binding;

public class CommandBindingsShould
{
    [Fact]
    public void MutateSingleBinding()
    {
        // Arrange
        var floatBinding = new Mock<Binding<float>>();
        floatBinding.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 0f));

        // Act
        var target = new Mutate<float, float>(floatBinding.Object, v => v + 1);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(1f, compiled());
    }
}