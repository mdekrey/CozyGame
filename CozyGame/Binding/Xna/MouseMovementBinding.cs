using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

/// <summary>Gets the delta of movement. If this is bound, the mouse is restricted to move only within the window when it is in focus</summary>
public record MouseMovementBinding() : Binding<Microsoft.Xna.Framework.Vector2>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (MouseState state, Previous<MouseState> previous) => new Microsoft.Xna.Framework.Vector2(state.X - previous.Value.X, state.Y - previous.Value.Y);
    }
}
