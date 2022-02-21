using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding.Xna;

public record GamepadButtonBinding(Buttons Button) : Binding<bool>
{
    public override LambdaExpression CreateValueLambda()
    {
        return (GamePadState state) => state.IsButtonDown(Button);
    }
}
