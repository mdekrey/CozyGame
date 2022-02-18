namespace MonoGamePlayground.Binding;

public abstract record BindableCommand(string Name);
public abstract record BindableCommand<TOutput>(string Name) : BindableCommand(Name);

/// <summary>Instantaneous commands are for button presses (menu OK, cancel, etc.)</summary>
public record InstantaneousCommand(string Name) : BindableCommand<bool>(Name);

/// <summary>Boolean state is for tracking the current state, either held or toggled (crouch, run, etc.)</summary>
public record BooleanStateCommand(string Name) : BindableCommand<bool>(Name);
/// <summary>Number state is for a position, such as X position of a mouse.</summary>
public record NumberStateCommand(string Name) : BindableCommand<float>(Name);
/// <summary>Number change is for a delta value, such turning in a 3d world.</summary>
public record NumberChangeCommand(string Name) : BindableCommand<float>(Name);
