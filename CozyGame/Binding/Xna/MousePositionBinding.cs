using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

/// <summary>Gets the current position of movement. If this is bound, the mouse is allowed to move freely on the window (and outside)</summary>
public record MousePositionBinding() : Binding<Microsoft.Xna.Framework.Vector2>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (MouseState state) => new Microsoft.Xna.Framework.Vector2(state.X, state.Y);
    }
}
