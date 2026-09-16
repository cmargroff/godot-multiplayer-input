using Godot;

namespace GodotMultiplayerInput;

public partial class TestController : Control
{
  [Export] private TextureRect ControllerBase;
  [Export] private Label ControllerLabel;
  [Export] private StickComponent ControllerStick;
  [Export] private StickComponent ControllerStick2;
  [Export] private CrossButtonsComponent FaceButtons;
  [Export] private CrossButtonsComponent DPad;
  [Export] private ButtonComponent MetaLeft;
  [Export] private ButtonComponent MetaRight;
  [Export] private ButtonComponent ShoulderLeft;
  [Export] private ButtonComponent ShoulderRight;
  [Export] private ButtonComponent TriggerLeft;
  [Export] private ButtonComponent TriggerRight;
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
  public override void _Process(double delta)
  {
    ControllerStick.Value = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    ControllerStick2.Value = Input.GetVector("look_left", "look_right", "look_up", "look_down");
    FaceButtons.ButtonValues = new float[]
    {
      Input.GetActionStrength("north"),
      Input.GetActionStrength("south"),
      Input.GetActionStrength("east"),
      Input.GetActionStrength("west")
    };
    DPad.ButtonValues = new float[]
    {
      Input.GetActionStrength("dpad_north"),
      Input.GetActionStrength("dpad_south"),
      Input.GetActionStrength("dpad_east"),
      Input.GetActionStrength("dpad_west")
    };
    MetaLeft.Value = Input.GetActionStrength("meta_left");
    MetaRight.Value = Input.GetActionStrength("meta_right");
    ShoulderLeft.Value = Input.GetActionStrength("shoulder_left");
    ShoulderRight.Value = Input.GetActionStrength("shoulder_right");
    TriggerLeft.Value = Input.GetActionStrength("trigger_left");
    TriggerRight.Value = Input.GetActionStrength("trigger_right");
  }
}
