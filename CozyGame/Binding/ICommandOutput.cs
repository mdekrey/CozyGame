namespace CozyGame.Binding
{
    public interface ICommandOutput
    {
        T GetOutput<T>(int playerIndex, BindableCommand<T> command);
    }
}