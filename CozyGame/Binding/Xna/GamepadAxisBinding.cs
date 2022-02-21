using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;


public enum GamepadAxis
{
    LeftTrigger,
    RightTrigger,
    LeftStickXAxis,
    LeftStickYAxis,
    RightStickXAxis,
    RightStickYAxis,
}

public record GamepadAxisBinding(GamepadAxis Axis) : Binding<float>
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
