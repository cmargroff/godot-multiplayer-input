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
  public PlayerInput PlayerInput;
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
    var minScale = Mathf.Min(Size.X / BaseSize.X, Size.Y / BaseSize.Y);
    ControllerBase.Scale = new Vector2(minScale, minScale);
  }
  public override void _Process(double delta)
  {
    ControllerStick.Value = PlayerInput.GetVector("left_x_neg", "left_x_pos", "left_y_neg", "left_y_pos");
    ControllerStick2.Value = PlayerInput.GetVector("right_x_neg", "right_x_pos", "right_y_neg", "right_y_pos");
    FaceButtons.ButtonValues = new float[]
    {
          PlayerInput.GetActionStrength("north"),
          PlayerInput.GetActionStrength("south"),
          PlayerInput.GetActionStrength("east"),
          PlayerInput.GetActionStrength("west")
    };
    DPad.ButtonValues = new float[]
    {
          PlayerInput.GetActionStrength("dpad_north"),
          PlayerInput.GetActionStrength("dpad_south"),
          PlayerInput.GetActionStrength("dpad_east"),
          PlayerInput.GetActionStrength("dpad_west")
    };
    MetaLeft.Value = PlayerInput.GetActionStrength("meta_left");
    MetaRight.Value = PlayerInput.GetActionStrength("meta_right");
    ShoulderLeft.Value = PlayerInput.GetActionStrength("shoulder_left");
    ShoulderRight.Value = PlayerInput.GetActionStrength("shoulder_right");
    TriggerLeft.Value = PlayerInput.GetActionStrength("trigger_left");
    TriggerRight.Value = PlayerInput.GetActionStrength("trigger_right");
  }
}
