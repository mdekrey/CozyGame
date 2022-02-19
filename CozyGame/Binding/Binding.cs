using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace CozyGame.Binding;

public interface IBinding
{
    System.Type ReturnType { get; }
    LambdaExpression CreateValueLambda();
}

public abstract record Binding<T>() : IBinding
{
    public System.Type ReturnType => typeof(T);
    public abstract LambdaExpression CreateValueLambda();
}
