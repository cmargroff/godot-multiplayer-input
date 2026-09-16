using System;
using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public class PlayerInput : IDisposable
{
  public int PlayerId { get; private set; }
  public Dictionary<string, InputBinding> Bindings { get; private set; }
  public event Action<bool> ConnectionStateChanged;
  private Dictionary<StringName, StringName> ActionNameMap = new();

  public PlayerInput(int playerId, Dictionary<string, InputBinding> bindings)
  {
    PlayerId = playerId;
    Bindings = ScopeInputBindings(bindings);
    RegisterInputBindings();
  }
  private Dictionary<string, InputBinding> ScopeInputBindings(Dictionary<string, InputBinding> bindings)
  {
    var scopedBindings = new Dictionary<string, InputBinding>();
    foreach (var binding in bindings)
    {
      var scopedKey = $"p{PlayerId}_{binding.Key}";
      var newBinding = binding.Value.MemberwiseClone();
      newBinding.Events.ForEach(e => e.Device = PlayerId);
      scopedBindings[scopedKey] = newBinding;
      ActionNameMap[binding.Key] = new StringName(scopedKey);
    }
    return scopedBindings;
  }
  private void RegisterInputBindings()
  {
    foreach (var binding in Bindings)
    {
      InputMap.AddAction(binding.Key, binding.Value.Deadzone);
      foreach (var inputEvent in binding.Value.Events)
      {
        InputMap.ActionAddEvent(binding.Key, inputEvent);
      }
    }
  }
  private void UnregisterInputBindings()
  {
    foreach (var binding in Bindings)
    {
      foreach (var inputEvent in binding.Value.Events)
      {
        InputMap.ActionEraseEvent(binding.Key, inputEvent);
      }
      InputMap.EraseAction(binding.Key);
    }
  }
  public void Dispose()
  {
    UnregisterInputBindings();
  }

  public bool IsActionPressed(StringName action)
  {
    if (ActionNameMap.TryGetValue(action, out var scopedAction))
    {
      return Input.IsActionPressed(scopedAction);
    }
    return false;
  }
  public bool IsActionJustPressed(StringName action)
  {
    if (ActionNameMap.TryGetValue(action, out var scopedAction))
    {
      return Input.IsActionJustPressed(scopedAction);
    }
    return false;
  }
  public bool IsActionJustReleased(StringName action)
  {
    if (ActionNameMap.TryGetValue(action, out var scopedAction))
    {
      return Input.IsActionJustReleased(scopedAction);
    }
    return false;
  }
  public float GetActionStrength(StringName action)
  {
    if (ActionNameMap.TryGetValue(action, out var scopedAction))
    {
      return Input.GetActionStrength(scopedAction);
    }
    return 0f;
  }
  public bool HasAction(StringName action)
  {
    return ActionNameMap.ContainsKey(action);
  }
  public Vector2 GetVector(StringName up, StringName down, StringName left, StringName right)
  {
    var leftName = ActionNameMap.TryGetValue(left, out var leftScoped) ? leftScoped : left;
    var rightName = ActionNameMap.TryGetValue(right, out var rightScoped) ? rightScoped : right;
    var upName = ActionNameMap.TryGetValue(up, out var upScoped) ? upScoped : up;
    var downName = ActionNameMap.TryGetValue(down, out var downScoped) ? downScoped : down;
    return Input.GetVector(upName, downName, leftName, rightName);
  }
  internal void EmitPlayerConnectedEvent(bool connected)
  {
    ConnectionStateChanged?.Invoke(connected);
  }
}