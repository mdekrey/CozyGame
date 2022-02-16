using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework.Input;

namespace MonoGamePlayground.Binding;

static class MutateBuilder
{
    public static LambdaExpression Build(Delegate combine, params LambdaExpression[] binding)
    {
        var parameters = (from lambda in binding
                          from param in lambda.Parameters
                          group param by param.Type).ToDictionary(g => Expression.Parameter(g.Key), g => g.ToArray());

        var visitor = new ReplaceParamsVisitor((from kvp in parameters
                                from p in kvp.Value
                                select (p, kvp.Key)).ToDictionary(t => t.p, t => t.Key));

        var body = Expression.Call(Expression.Constant(combine.Target), combine.Method, binding.Select(b => b.Body));

        return Expression.Lambda(body, parameters.Keys);
    }

    private class ReplaceParamsVisitor : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> parameterReplacements;

        public ReplaceParamsVisitor(Dictionary<ParameterExpression, ParameterExpression> parameterReplacements)
        {
            this.parameterReplacements = parameterReplacements;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return parameterReplacements.TryGetValue(node, out var result) ? result : node;
        }
    }
}

public record Mutate<T1, TOut>(Binding<T1> Binding1, Func<T1, TOut> Combine) : Binding<TOut>
{
    public override LambdaExpression CreateValueLambda()
    {
        return MutateBuilder.Build(Combine, Binding1.CreateValueLambda());
    }
}
public record Mutate<T1, T2, TOut>(Binding<T1> Binding1, Binding<T2> Binding2, Func<T1, T2, TOut> Combine) : Binding<TOut>
{
    public override LambdaExpression CreateValueLambda()
    {
        return MutateBuilder.Build(Combine, Binding1.CreateValueLambda(), Binding2.CreateValueLambda());
    }
}

public record Mutate<T1, T2, T3, TOut>(Binding<T1> Binding1, Binding<T2> Binding2, Binding<T3> Binding3, Func<T1, T2, T3, TOut> Combine) : Binding<TOut>
{
    public override LambdaExpression CreateValueLambda()
    {
        return MutateBuilder.Build(Combine, Binding1.CreateValueLambda(), Binding2.CreateValueLambda(), Binding3.CreateValueLambda());
    }
}

public record Mutate<T1, T2, T3, T4, TOut>(Binding<T1> Binding1, Binding<T2> Binding2, Binding<T3> Binding3, Binding<T4> Binding4, Func<T1, T2, T3, T4, TOut> Combine) : Binding<TOut>
{
    public override LambdaExpression CreateValueLambda()
    {
        return MutateBuilder.Build(Combine, Binding1.CreateValueLambda(), Binding2.CreateValueLambda(), Binding3.CreateValueLambda(), Binding4.CreateValueLambda());
    }
}

// record ToEvent(Binding<bool> Target, bool WhenChangesTo) : Binding<EventUnit>;
