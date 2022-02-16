namespace MonoGamePlayground.Binding;

abstract record BindableCommand(string Name);

/// <summary>Instantaneous commands are for button presses (menu OK, cancel, etc.)</summary>
record InstantaneousCommand(string Name) : BindableCommand(Name);

/// <summary>Boolean state is for tracking the current state, either held or toggled (crouch, run, etc.)</summary>
record BooleanStateCommand(string Name) : BindableCommand(Name);
/// <summary>Number state is for a position, such as X position of a mouse.</summary>
record NumberStateCommand(string Name) : BindableCommand(Name);
/// <summary>Number change is for a delta value, such turning in a 3d world.</summary>
record NumberChangeCommand(string Name) : BindableCommand(Name);
