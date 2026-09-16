using Godot;

namespace GodotMultiplayerInput;

public partial class TestController : Control
{
  [Export] private TextureRect ControllerBase;
  [Export] private Label ControllerLabel;
  [Export] private Label DeviceNameLabel;
  [Export] private Control DisconnectedBanner;
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
  public PlayerInput PlayerInput;
  private Vector2 BaseSize = Vector2.One;
  private StringName LeftXNeg = "left_x_neg";
  private StringName LeftXPos = "left_x_pos";
  private StringName LeftYNeg = "left_y_neg";
  private StringName LeftYPos = "left_y_pos";
  private StringName RightXNeg = "right_x_neg";
  private StringName RightXPos = "right_x_pos";
  private StringName RightYNeg = "right_y_neg";
  private StringName RightYPos = "right_y_pos";
  private StringName North = "north";
  private StringName South = "south";
  private StringName East = "east";
  private StringName West = "west";
  private StringName DPadNorth = "dpad_north";
  private StringName DPadSouth = "dpad_south";
  private StringName DPadEast = "dpad_east";
  private StringName DPadWest = "dpad_west";
  private StringName MetaLeftAction = "meta_left";
  private StringName MetaRightAction = "meta_right";
  private StringName ShoulderLeftAction = "shoulder_left";
  private StringName ShoulderRightAction = "shoulder_right";
  private StringName TriggerLeftAction = "trigger_left";
  private StringName TriggerRightAction = "trigger_right";
  private bool IsDeviceConnected = true;
  public override void _EnterTree()
  {
    ControllerLabel.Text = $"Player {PlayerInput.PlayerId + 1}";
    PlayerInput.ConnectionStateChanged += ConnectionStateChanged;
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
    if (!IsDeviceConnected)
    {
      return;
    }
    var start = Time.GetTicksUsec();
    ControllerStick.Value = PlayerInput.GetVector(LeftXNeg, LeftXPos, LeftYNeg, LeftYPos);
    ControllerStick2.Value = PlayerInput.GetVector(RightXNeg, RightXPos, RightYNeg, RightYPos);
    FaceButtons.ButtonValues = new float[]
    {
          PlayerInput.GetActionStrength(North),
          PlayerInput.GetActionStrength(South),
          PlayerInput.GetActionStrength(East),
          PlayerInput.GetActionStrength(West)
    };
    DPad.ButtonValues = new float[]
    {
          PlayerInput.GetActionStrength(DPadNorth),
          PlayerInput.GetActionStrength(DPadSouth),
          PlayerInput.GetActionStrength(DPadEast),
          PlayerInput.GetActionStrength(DPadWest)
    };
    MetaLeft.Value = PlayerInput.GetActionStrength(MetaLeftAction);
    MetaRight.Value = PlayerInput.GetActionStrength(MetaRightAction);
    ShoulderLeft.Value = PlayerInput.GetActionStrength(ShoulderLeftAction);
    ShoulderRight.Value = PlayerInput.GetActionStrength(ShoulderRightAction);
    TriggerLeft.Value = PlayerInput.GetActionStrength(TriggerLeftAction);
    TriggerRight.Value = PlayerInput.GetActionStrength(TriggerRightAction);
    var end = Time.GetTicksUsec();
  }
  private void ConnectionStateChanged(bool connected)
  {
    IsDeviceConnected = connected;
    // handle connection state changes if necessary
    DisconnectedBanner.Visible = !IsDeviceConnected;
    if (IsDeviceConnected)
    {
      UpdateDeviceName();
    }
  }
  private void UpdateDeviceName()
  {
    DeviceNameLabel.Text = PlayerInput.DeviceName;
  }
}
