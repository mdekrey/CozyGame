using System.Collections.Generic;

namespace MonoGamePlayground.Binding;

public class Bindings
{
    private Dictionary<string, IBinding> dictionary;

    public Bindings(Dictionary<string, IBinding> dictionary)
    {
        this.dictionary = dictionary;
    }

    public object this[BindableCommand command]
    {
        get
        {
            if (!dictionary.TryGetValue(command.Name, out var binding))
                throw new KeyNotFoundException();
            return binding;
        }
    }
}