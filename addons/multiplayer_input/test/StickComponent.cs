using Godot;
using System;

public partial class StickComponent : Control
{    
    // [Export]
    // private float _fillX = 0f;
    // [Export]
    // private float _fillY = 0f;
    private TextureRect _filled;

    public override void _EnterTree()
    {
        _filled = GetNode<TextureRect>("%Filled");
        // ChangeFillOpacity(_fillX, _fillY);
    }

    public void ChangeFillOpacity(float valueX, float valueY)
    {
        var modulate = _filled.SelfModulate;
        modulate.A = Math.Max(valueX, valueY);
        _filled.SelfModulate = modulate;
    }
}
