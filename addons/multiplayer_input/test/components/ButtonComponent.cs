using Godot;

public partial class ButtonComponent : Control
{
  [Export] private TextureRect _fill;
  public float Value
  {
    get => _fill.Modulate.A;
    set
    {
      var modulate = _fill.Modulate;
      modulate.A = value;
      _fill.Modulate = modulate;
    }
  }

}
