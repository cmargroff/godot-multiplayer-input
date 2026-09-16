using System;
using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public partial class Test : GridContainer
{
  [Export] private PackedScene TestControllerScene;
  private InputManager inputManager = new InputManager();
  private Dictionary<int, Control> playerIds = new Dictionary<int, Control>();

  public override void _Ready()
  {
    inputManager.ParseMap("res://addons/multiplayer_input/test/inputmap.cfg");
    inputManager.PlayerConnected += PlayerConnected;
    inputManager.PlayerRemoved += PlayerRemoved;
  }
  private void PlayerConnected(PlayerInput playerInput)
  {
    if (TestControllerScene != null)
    {
      if (playerIds.ContainsKey(playerInput.PlayerId))
      {
        return;
      }
      var controller = TestControllerScene.Instantiate<TestController>();
      playerIds[playerInput.PlayerId] = controller;
      controller.PlayerInput = playerInput;
      AddChild(controller);
      UpdateColumns();
    }
  }
  private void PlayerRemoved(int playerId)
  {
    if (playerIds.ContainsKey(playerId))
    {
      var controller = playerIds[playerId];
      playerIds.Remove(playerId);
      controller.QueueFree();
      UpdateColumns();
    }
  }
  private void UpdateColumns()
  {
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