using System;
using Godot;

namespace GodotMultiplayerInput;

public partial class Test : GridContainer
{
  [Export] private PackedScene TestControllerScene;
  private InputManager inputManager = new InputManager();
  public override void _Ready()
  {
    inputManager.ParseMap("res://addons/multiplayer_input/test/inputmap.cfg");
    Input.JoyConnectionChanged += OnJoyConnectionChanged;
  }
  private void OnJoyConnectionChanged(long deviceId, bool connected)
  {
    if (connected)
    {
      if (TestControllerScene != null)
      {
        var controller = TestControllerScene.Instantiate<TestController>();
        controller.PlayerInput = inputManager.GetPlayer((int)deviceId);
        AddChild(controller);
        var childCount = GetChildren().Count;
        if (childCount < 4)
        {
          Columns = childCount;
        }
        else
        {
          Columns = (int)Math.Ceiling(Math.Sqrt(childCount));
        }
      }
    }
  }
}