using System;
using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public partial class Test : GridContainer
{
  [Export] private PackedScene TestControllerScene;
  private InputManager inputManager = new InputManager();
  private List<int> playerIds = new List<int>();
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
        if (playerIds.Contains((int)deviceId))
        {
          return;
        }
        var controller = TestControllerScene.Instantiate<TestController>();
        var playerInput = inputManager.GetPlayer((int)deviceId);
        playerIds.Add((int)deviceId);
        controller.PlayerInput = playerInput;
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