using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonoGamePlayground.Binding;

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

public record BindingCombination<TOut>(Delegate Combine, params IBinding[] Bindings) : Binding<TOut>
{
    public override LambdaExpression CreateValueLambda()
    {
        return MutateBuilder.Build(Combine, Bindings.Select(b => b.CreateValueLambda()).ToArray());
    }
}
