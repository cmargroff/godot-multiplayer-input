using Godot;
using System;

public partial class StickComponent : Control
{    
    [Export] private TextureRect _fill;
    public Vector2 Value
    {
        get => Value;
        set
        {
            var modulate = _fill.Modulate;
            modulate.A = value.Length();
            _fill.Modulate = modulate;
            Position = value;
        }
    }

}
