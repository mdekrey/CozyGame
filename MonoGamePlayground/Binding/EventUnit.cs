namespace MonoGamePlayground.Binding;

public record EventUnit
{
    private EventUnit() { }

    public static readonly EventUnit Value = new();
}
