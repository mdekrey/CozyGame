using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

public record KeyboardButtonBinding(Keys Key) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (KeyboardState state) => state.IsKeyDown(Key);
    }
}
