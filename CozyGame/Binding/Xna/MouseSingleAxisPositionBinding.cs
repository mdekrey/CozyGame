using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

/// <summary>Gets the current position of movement. If this is bound, the mouse is allowed to move freely on the window (and outside)</summary>
public record MouseSingleAxisPositionBinding(MouseAxis Axis) : Binding<float>
{
    public override LambdaExpression CreateValueLambda()
    {
        return Axis switch
        {
            MouseAxis.X => (MouseState state) => state.X,
            MouseAxis.Y => (MouseState state) => state.Y,
            MouseAxis.ScrollWheel => (MouseState state) => state.ScrollWheelValue,
            MouseAxis.HorizontalScrollWheel => (MouseState state) => state.HorizontalScrollWheelValue,
            _ => throw new InvalidOperationException(),
        };
    }
}
