using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;
using System.Threading.Tasks;

namespace MonoGamePlayground.Binding;

public class ScriptedLambda
{
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
}
