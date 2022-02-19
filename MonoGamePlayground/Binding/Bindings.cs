using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static System.Linq.Expressions.Expression;

namespace MonoGamePlayground.Binding;

public class Bindings
{
    private readonly IReadOnlyDictionary<BindableCommand, IBinding> dictionary;
    private readonly InputState inputState;
    private readonly ConcurrentDictionary<BindableCommand, Func<InputState, int, object>> commandResolvers = new();
    private readonly Dictionary<OutputStateKey, object> outputStates;

    public Bindings(Dictionary<BindableCommand, IBinding> dictionary, InputState inputState)
    {
        this.dictionary = dictionary;
        this.inputState = inputState;
        this.outputStates = new Dictionary<OutputStateKey, object>(dictionary.Count * inputState.MaximumPlayers);

        foreach (var command in dictionary.Keys)
        {
            commandResolvers.GetOrAdd(command, CompileCommand);
        }
    }

    public InputState InputState => inputState;

    public void UpdateOutputs()
    {
        for (var playerIndex = 0; playerIndex < inputState.MaximumPlayers; playerIndex++)
        {
            foreach (var command in dictionary.Keys)
            {
                var compiled = commandResolvers.GetOrAdd(command, CompileCommand);
                var result = compiled(inputState, playerIndex);
                outputStates[new OutputStateKey(playerIndex, command)] = result;
            }
        }
    }

    public T GetOutput<T>(int playerIndex, BindableCommand<T> command)
    {
        return (T)outputStates[new OutputStateKey(playerIndex, command)];
    }

    private Func<InputState, int, object> CompileCommand(BindableCommand command)
    {
        if (!dictionary.TryGetValue(command, out var binding))
            throw new KeyNotFoundException();
        
        var lambda = binding.CreateValueLambda();

        var inputState = Parameter(typeof(InputState), "inputState");
        var playerIndex = Parameter(typeof(int), "playerIndex");

        var resultBody = new ReplaceParamsInputStateVisitor(inputState, playerIndex)
            .Visit(lambda.Body);
        
        return Lambda<Func<InputState, int, object>>(
            Convert(resultBody, typeof(object)),
            inputState,
            playerIndex
        ).Compile();
    }

    private record struct OutputStateKey(int PlayerIndex, BindableCommand Command);
}

class ReplaceParamsInputStateVisitor : ExpressionVisitor
{
    private readonly ParameterExpression inputState;
    private readonly ParameterExpression playerIndex;

    public ReplaceParamsInputStateVisitor(ParameterExpression inputState, ParameterExpression playerIndex)
    {
        this.inputState = inputState;
        this.playerIndex = playerIndex;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member.Name == nameof(Previous<object>.Value) && node.Expression is ParameterExpression inner 
            && inner.Type.IsGenericType && inner.Type.GetGenericTypeDefinition() == typeof(Previous<>))
        {
            var type = node.Type;
            if (type == typeof(KeyboardState))
                return Call(inputState, nameof(InputState.GetPreviousKeyboardState), null);
            if (type == typeof(MouseState))
                return Call(inputState, nameof(InputState.GetPreviousMouseState), null);
            if (type == typeof(GamePadState))
                return Call(inputState, nameof(InputState.GetPreviousGamePadState), null, playerIndex);
            throw new NotSupportedException();
        }
        return base.VisitMember(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node == inputState || node == playerIndex)
            return base.VisitParameter(node);
        var type = node.Type;
        if (type == typeof(KeyboardState))
            return Call(inputState, nameof(InputState.GetKeyboardState), null);
        if (type == typeof(MouseState))
            return Call(inputState, nameof(InputState.GetMouseState), null);
        if (type == typeof(GamePadState))
            return Call(inputState, nameof(InputState.GetGamePadState), null, playerIndex);
        if (type == typeof(int))
            return playerIndex;

        return base.VisitParameter(node);
    }
}