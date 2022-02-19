using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CozyGame.Binding;

public class CommandSet : IReadOnlySet<BindableCommand>
{
    private readonly IReadOnlySet<BindableCommand> commands;

    public CommandSet(IEnumerable<BindableCommand> commands)
    {
        this.commands = commands.ToHashSet();
    }
    public int Count => commands.Count;

    public bool Contains(BindableCommand item)
    {
        return commands.Contains(item);
    }

    public IEnumerator<BindableCommand> GetEnumerator()
    {
        return commands.GetEnumerator();
    }

    public bool IsProperSubsetOf(IEnumerable<BindableCommand> other)
    {
        return commands.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<BindableCommand> other)
    {
        return commands.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<BindableCommand> other)
    {
        return commands.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<BindableCommand> other)
    {
        return commands.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<BindableCommand> other)
    {
        return commands.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<BindableCommand> other)
    {
        return commands.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)commands).GetEnumerator();
    }
}
