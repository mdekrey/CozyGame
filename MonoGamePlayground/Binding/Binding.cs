using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace MonoGamePlayground.Binding;

public abstract record Binding<T>()
{
    public abstract LambdaExpression CreateValueLambda();
}
