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

    [Fact]
    public void MutateTwoBindings()
    {
        // Arrange
        var floatBinding1 = new Mock<Binding<float>>();
        floatBinding1.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 2f));
        var floatBinding2 = new Mock<Binding<float>>();
        floatBinding2.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 3.5f));

        // Act
        var target = new Mutate<float, float, float>(floatBinding1.Object, floatBinding2.Object, (a, b) => a + b);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(5.5f, compiled());
    }
}