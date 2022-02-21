using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;
public enum MouseButton
{
    Left,
    Middle,
    Right,
    X1,
    X2,
}
public record MouseButtonBinding(MouseButton Button) : Binding<bool>
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
