namespace CozyGame.Binding;

/// <summary>Used for dependency injection to indicate the previous value of immutable state</summary>
public record struct Previous<T>(T Value);