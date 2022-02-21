using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

public record PerPlayerKeyboardBinding(Keys[] KeysByPlayer) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (KeyboardState state, int playerIndex) => KeysByPlayer.Length > playerIndex && state.IsKeyDown(KeysByPlayer[playerIndex]);
    }
}