using Godot;

namespace GodotMultiplayerInput;

public partial class Test : Control
{
  [Export] private PackedScene TestControllerScene;
  private InputManager inputManager = new InputManager();
  public override void _Ready()
  {
    inputManager.ParseMap("res://addons/multiplayer_input/test/inputmap.cfg");
    }
  }
}