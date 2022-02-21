using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

/// <summary>Gets the delta of movement. If this is bound, the mouse is restricted to move only within the window when it is in focus</summary>
public record MouseMovementBinding(MouseAxis Axis) : Binding<float>
{
    public override LambdaExpression CreateValueLambda()
    {
        return Axis switch
        {
            MouseAxis.X => (MouseState state, Previous<MouseState> previous) => state.X - previous.Value.X,
            MouseAxis.Y => (MouseState state, Previous<MouseState> previous) => state.Y - previous.Value.Y,
            MouseAxis.ScrollWheel => (MouseState state, Previous<MouseState> previous) => state.ScrollWheelValue - previous.Value.ScrollWheelValue,
            MouseAxis.HorizontalScrollWheel => (MouseState state, Previous<MouseState> previous) => state.HorizontalScrollWheelValue - previous.Value.HorizontalScrollWheelValue,
            _ => throw new InvalidOperationException(),
        };
    }
}
