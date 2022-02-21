using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

public enum DPadDirection
{
    Up,
    Right,
    Down,
    Left,
}
public record GamepadDPadBinding(DPadDirection Button) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return Button switch
        {
            DPadDirection.Up => (GamePadState state) => state.DPad.Up == ButtonState.Pressed,
            DPadDirection.Right => (GamePadState state) => state.DPad.Right == ButtonState.Pressed,
            DPadDirection.Down => (GamePadState state) => state.DPad.Down == ButtonState.Pressed,
            DPadDirection.Left => (GamePadState state) => state.DPad.Left == ButtonState.Pressed,
            _ => throw new InvalidOperationException(),
        };
    }
}