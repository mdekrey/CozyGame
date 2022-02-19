using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;
enum MouseButton
{
    Left,
    Middle,
    Right,
    X1,
    X2,
}
record MouseButtonBinding(MouseButton Button) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return Button switch
        {
            MouseButton.Left => (MouseState state) => state.LeftButton == ButtonState.Pressed,
            MouseButton.Middle => (MouseState state) => state.MiddleButton == ButtonState.Pressed,
            MouseButton.Right => (MouseState state) => state.RightButton == ButtonState.Pressed,
            MouseButton.X1 => (MouseState state) => state.XButton1 == ButtonState.Pressed,
            MouseButton.X2 => (MouseState state) => state.XButton2 == ButtonState.Pressed,
            _ => throw new InvalidOperationException(),
        };
    }
}

enum MouseAxis
{
    X,
    Y,
    ScrollWheel,
    HorizontalScrollWheel,
}
/// <summary>Gets the delta of movement. If this is bound, the mouse is restricted to move only within the window when it is in focus</summary>
record MouseMovementBinding(MouseAxis Axis) : Binding<float>
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
/// <summary>Gets the current position of movement. If this is bound, the mouse is allowed to move freely on the window (and outside)</summary>
record MousePositionBinding(MouseAxis Axis) : Binding<float>
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
