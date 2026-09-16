using System;
using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public class InputManager : IInputManager, IDisposable
{
  public event Action<PlayerInput> PlayerConnected;
  public event Action<PlayerInput> PlayerDisconnected;
  public event Action<PlayerInput> PlayerReconnected;
  public event Action<int> PlayerRemoved;
  private Dictionary<string, InputBinding> InputMap;
  private const string InputSection = "input";
  private Dictionary<int, PlayerInput> PlayerInputs = new Dictionary<int, PlayerInput>();
  public InputManager()
  {
    Input.JoyConnectionChanged += OnJoyConnectionChanged;
  }

  private void OnJoyConnectionChanged(long deviceId, bool connected)
  {
    if (!connected && PlayerInputs.ContainsKey((int)deviceId))
    {
      PlayerReconnected?.Invoke(GetPlayer((int)deviceId));
      return;
    }
    var player = GetPlayer((int)deviceId);
    if (connected)
    {
      PlayerConnected?.Invoke(player);
    }
    else
    {
      PlayerDisconnected?.Invoke(player);
    }
    player.EmitPlayerConnectedEvent(connected);
  }
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
    if (!PlayerInputs.ContainsKey(playerId))
    {
      PlayerInputs[playerId] = new PlayerInput(playerId, InputMap);
    }
    return PlayerInputs[playerId];
  }
  public void RemovePlayer(int playerId)
  {
    if (PlayerInputs.ContainsKey(playerId))
    {
      PlayerInputs[playerId].Dispose();
      PlayerInputs.Remove(playerId);
      PlayerRemoved?.Invoke(playerId);
    }
  }
  public void Dispose()
  {
    foreach (var playerInput in PlayerInputs.Values)
    {
      playerInput.Dispose();
    }
    PlayerInputs.Clear();
  }
}