using Godot;

public partial class CrossButtonsComponent : Control
{
  [Export] private ButtonComponent NorthButton;
  [Export] private ButtonComponent SouthButton;
  [Export] private ButtonComponent EastButton;
  [Export] private ButtonComponent WestButton;
  private float[] _buttonValues = new float[4];
  public float[] ButtonValues
  {
    get => _buttonValues;
    set
    {
      _buttonValues = value;
      NorthButton.Value = _buttonValues[0];
      SouthButton.Value = _buttonValues[1];
      EastButton.Value = _buttonValues[2];
      WestButton.Value = _buttonValues[3];
    }
  }
}
