using Godot;

namespace GodotMultiplayerInput;

public partial class Test : Control
{
  [Export] private PackedScene TestControllerScene;

  public override void _Ready()
  {
    // instantiate the test controller scene
    if (TestControllerScene != null)
    {
      var controller = TestControllerScene.Instantiate<Control>();
      AddChild(controller);
    }
  }
}