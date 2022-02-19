
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CozyGame.Binding;

public class BindingParsing
{
    private static readonly Dictionary<string, Func<object, BindingParsing, Task<IBinding>>> CommandParsers = new()
    {
        ["combineToBoolean"] = (fromYaml, parsing) => {
            var arr = (IReadOnlyList<object>)fromYaml;
            return parsing.CombineBindings<bool>(
                (string)arr[0],
                arr.Skip(1)
                   .Cast<Dictionary<object, object>>()
                   .Select(d => d.ToDictionary(kvp => (string)kvp.Key, kvp => kvp.Value))
                   .ToArray());
        },
        ["gamepadButton"] = (fromYaml, parsing) => Task.FromResult(GamepadButton((string)fromYaml)),
        ["keyboard"] = (fromYaml, parsing) => Task.FromResult(Keyboard((string)fromYaml)),
        ["perPlayerKeyboard"] = (fromYaml, parsing) =>
        {
            var arr = (IReadOnlyList<object>)fromYaml;
            return Task.FromResult(PerPlayerKeyboard(arr.Cast<string>().ToArray()));
        },
    };
    private static readonly ScriptedLambda scriptParser = new();
    private static readonly IReadOnlyList<Type> funcTypes = new[]
    {
        typeof(Func<>),
        typeof(Func<,>),
        typeof(Func<,,>),
        typeof(Func<,,,>),
        typeof(Func<,,,,>),
        typeof(Func<,,,,,>),
        typeof(Func<,,,,,,>),
        typeof(Func<,,,,,,,>),
        typeof(Func<,,,,,,,,>),
        typeof(Func<,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,,>),
    };

    public async Task<Bindings> Parse(string config, CommandSet commandSet, InputState inputState)
    {
        return await ExtractBindingsFromParsed(commandSet, inputState, ParseYaml(config));
    }

    public static Dictionary<string, Dictionary<string, object>> ParseYaml(string config)
    {
        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                    .Build();
        using var sr = new StringReader(config);
        var result = deserializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(sr);
        return result;
    }

    public async Task<Bindings> ExtractBindingsFromParsed(CommandSet commandSet, InputState inputState, Dictionary<string, Dictionary<string, object>> result)
    {
        return new Bindings(
            new Dictionary<BindableCommand, IBinding>(
                await Task.WhenAll(
                    commandSet
                        .Select(async command => new KeyValuePair<BindableCommand, IBinding>(command, await ParseBinding(result[command.Name])))
                )
            ),
            inputState
        );
    }

    private async Task<IBinding> ParseBinding(Dictionary<string, object> value)
    {
        if (value.Count != 1) throw new NotImplementedException();

        var key = value.Keys.Single();
        return await CommandParsers[key](value[key], this);
    }

    // Expects an array, first element is script to combine, second is an array of other bindings
    private async Task<IBinding> CombineBindings<T>(string expression, Dictionary<string, object>[] subBindingTrees)
    {
        var subBindings = await Task.WhenAll(subBindingTrees.Select(ParseBinding));
        var baseType = funcTypes[subBindings.Length];
        var delegateType = baseType.MakeGenericType(subBindings.Select(b => b.ReturnType).Concat(new[] { typeof(T) }).ToArray());
        var combiner = (Delegate?)await scriptParser.EvaluateAsync(expression, delegateType);
        System.Diagnostics.Debug.Assert(combiner != null);
        return new BindingCombination<T>(combiner, subBindings);
    }

    // Expects a string representing a gamepad button
    private static IBinding GamepadButton(string button)
    {
        return new Xna.GamepadButtonBinding(Enum.Parse<Microsoft.Xna.Framework.Input.Buttons>(button));
    }

    // Expects a string representing a keyboard key
    private static IBinding Keyboard(string key)
    {
        return new Xna.KeyboardButtonBinding(Enum.Parse<Microsoft.Xna.Framework.Input.Keys>(key));
    }

    private static IBinding PerPlayerKeyboard(string[] keysByPlayer)
    {
        return new Xna.PerPlayerKeyboardBinding(keysByPlayer.Select(key => Enum.Parse<Microsoft.Xna.Framework.Input.Keys>(key)).ToArray());
    }
}