using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace MonoGamePlayground.Binding.Xna;

record GamepadButtonBinding(Buttons Button) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (GamePadState state) => state.IsButtonDown(Button);
    }
}

enum DPadDirection
{
    Up,
    Right,
    Down,
    Left,
}
record GamepadDPadBinding(DPadDirection Button) : Binding<bool>
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
enum GamepadAxis
{
    LeftTrigger,
    RightTrigger,
    LeftStickXAxis,
    LeftStickYAxis,
    RightStickXAxis,
    RightStickYAxis,
}
record GamepadAxisBinding(GamepadAxis Axis) : Binding<float>
{
    public override LambdaExpression CreateValueLambda()
    {
        return Axis switch
        {
            GamepadAxis.LeftTrigger => (GamePadState state) => state.Triggers.Left,
            GamepadAxis.RightTrigger => (GamePadState state) => state.Triggers.Right,
            GamepadAxis.LeftStickXAxis => (GamePadState state) => state.ThumbSticks.Left.X,
            GamepadAxis.LeftStickYAxis => (GamePadState state) => state.ThumbSticks.Left.Y,
            GamepadAxis.RightStickXAxis => (GamePadState state) => state.ThumbSticks.Right.X,
            GamepadAxis.RightStickYAxis => (GamePadState state) => state.ThumbSticks.Right.Y,
            _ => throw new InvalidOperationException(),
        };
    }
}
