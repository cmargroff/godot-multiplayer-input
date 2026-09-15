using Godot;
using System;

public partial class ButtonComponent : Control
{
    // [Export]
    // private float _fill = 0f;
    private TextureRect _filled;

    public override void _EnterTree()
    {
        _filled = GetNode<TextureRect>("%Filled");
        // ChangeFillOpacity(_fill);
    }

    public void ChangeFillOpacity(float value)
    {
        var modulate = _filled.SelfModulate;
        modulate.A = value;
        _filled.SelfModulate = modulate;
    }

}
