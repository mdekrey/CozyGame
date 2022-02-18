using System;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace MonoGamePlayground.Binding;

public class CommandBindingsShould
{
    [Fact]
    public void CombineSingleBinding()
    {
        // Arrange
        var floatBinding = new Mock<Binding<float>>();
        floatBinding.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 0f));

        // Act
        var target = new BindingCombination<float>((Func<float, float>)(v => v + 1), floatBinding.Object);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(1f, compiled());
    }

    [Fact]
    public void CombineTwoBindings()
    {
        // Arrange
        var floatBinding1 = new Mock<Binding<float>>();
        floatBinding1.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 2f));
        var floatBinding2 = new Mock<Binding<float>>();
        floatBinding2.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 3.5f));

        // Act
        var target = new BindingCombination<float>((Func<float, float, float>)((a, b) => a + b), floatBinding1.Object, floatBinding2.Object);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(5.5f, compiled());
    }

    [Fact]
    public void CombineThreeBindings()
    {
        // Arrange
        var floatBinding1 = new Mock<Binding<float>>();
        floatBinding1.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 2f));
        var floatBinding2 = new Mock<Binding<float>>();
        floatBinding2.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 3.5f));
        var floatBinding3 = new Mock<Binding<float>>();
        floatBinding3.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 1.5f));

        // Act
        var target = new BindingCombination<float>((Func<float, float, float, float>)((a, b, c) => a + b - c), floatBinding1.Object, floatBinding2.Object, floatBinding3.Object);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(4f, compiled());
    }

    [Fact]
    public void CombineFourBindings()
    {
        // Arrange
        var floatBinding1 = new Mock<Binding<float>>();
        floatBinding1.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 2f));
        var floatBinding2 = new Mock<Binding<float>>();
        floatBinding2.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 3.5f));
        var floatBinding3 = new Mock<Binding<float>>();
        floatBinding3.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 1.5f));
        var floatBinding4 = new Mock<Binding<float>>();
        floatBinding4.Setup(mock => mock.CreateValueLambda()).Returns((LambdaExpression)(() => 6f));

        // Act
        var target = new BindingCombination<float>((Func<float, float, float, float, float>)((a, b, c, d) => a + b - c + d), floatBinding1.Object, floatBinding2.Object, floatBinding3.Object, floatBinding4.Object);

        // Assert
        var compiled = (Func<float>)target.CreateValueLambda().Compile();
        Assert.Equal(10f, compiled());
    }
}