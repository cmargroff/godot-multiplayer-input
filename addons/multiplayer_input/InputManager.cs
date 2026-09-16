using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public class InputManager : IInputManager
{

  private Dictionary<string, InputBinding> InputMap;
  private const string InputSection = "input";
  public void ParseMap(string filePath = "res://project.godot")
  {
    var settings_file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
    var settings_content = settings_file.GetAsText();
    settings_file.Close();
    var settings_raw = new ConfigFile();
    settings_raw.Parse(settings_content);
    var action_list = settings_raw.GetSectionKeys(InputSection);
    InputMap = new Dictionary<string, InputBinding>();
    foreach (var action in action_list)
    {
      var binding_config = settings_raw.GetValue(InputSection, action);
      InputMap[action] = new InputBinding(binding_config);
    }
  }
  public PlayerInput GetPlayer(int playerId)
  {
    return new PlayerInput(playerId, InputMap);
  }
}