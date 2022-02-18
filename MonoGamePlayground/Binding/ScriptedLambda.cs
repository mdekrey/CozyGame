using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable

namespace MonoGamePlayground.Binding;

public class ScriptedLambda
{
    private static readonly ConcurrentDictionary<Type, Func<ScriptedLambda, string, Task<object?>>> TypedEvaluation = new();
    private readonly ScriptOptions options;

    public ScriptedLambda()
    {
        options = ScriptOptions.Default
            .WithReferences(Enumerable.Empty<Microsoft.CodeAnalysis.MetadataReference>())
            .AddReferences(typeof(System.Math).Assembly);
    }

    public async Task<T> EvaluateAsync<T>(string script)
    {
        return await CSharpScript.EvaluateAsync<T>(script, options: this.options);
    }
    
    private async Task<object?> EvaluateUntypedAsync<T>(string script)
    {
        return await EvaluateAsync<T>(script);
    }
    public Task<object?> EvaluateAsync(string script, Type targetType)
    {
        var compiledConversion = TypedEvaluation.GetOrAdd(targetType, BuildTypedEvaluateAsync);

        return compiledConversion(this, script);
    }

    private static Func<ScriptedLambda, string, Task<object?>> BuildTypedEvaluateAsync(Type arg)
    {
        var _this = Expression.Parameter(typeof(ScriptedLambda), "_this");
        var script = Expression.Parameter(typeof(string), "script");

        return Expression.Lambda<Func<ScriptedLambda, string, Task<object?>>>(
            Expression.Call(_this, nameof(EvaluateUntypedAsync), new[] { arg }, script),
            _this,
            script
        ).Compile();
    }
}
