using Godot;

namespace GodotMultiplayerInput;

public partial class TestController : Control
{
  [Export] private TextureRect ControllerBase;
  [Export] private Label ControllerLabel;
  public int PlayerId = 0;
  private Vector2 BaseSize = Vector2.One;
  public override void _EnterTree()
  {
    ControllerLabel.Text = $"Player {PlayerId + 1}";
  }
  public override void _Ready()
  {
    // capture the initial size of the controller base
    BaseSize = ControllerBase.Size;
    // bind event for resizing
    Connect("resized", new Callable(this, "OnResized"));
    // call the resize handler initially
    OnResized();
  }
  private void OnResized()
  {
    // this will update the controller base size
    // all controls should be relative to the base by parenting and inherit the transformation matrix
    var minScale = Mathf.Min(BaseSize.X / Size.X, BaseSize.Y / Size.Y);
    ControllerBase.Scale = new Vector2(minScale, minScale);
  }
}
