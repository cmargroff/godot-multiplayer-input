using System;
using Godot;

public partial class StickComponent : Control
{
    [Export] private TextureRect _clear;
    [Export] private TextureRect _fill;
    [Export] public float DistanceScale = 15.0f;
    public Vector2 Value
    {
        get => Position / DistanceScale;
        set
        {
            var modulate = _fill.Modulate;
            modulate.A = value.Length();
            _fill.Modulate = modulate;
            _clear.OffsetTransformPosition = value * DistanceScale;
        }
    }

}
