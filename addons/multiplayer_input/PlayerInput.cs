using System;
using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public class PlayerInput : IDisposable
{
  public int PlayerId { get; private set; }
  public Dictionary<string, InputBinding> Bindings { get; private set; }

  public PlayerInput(int playerId, Dictionary<string, InputBinding> bindings)
  {
    PlayerId = playerId;
    Bindings = ScopeInputBindings(bindings);
    RegisterInputBindings();
  }
  private Dictionary<string, InputBinding> ScopeInputBindings(Dictionary<string, InputBinding> bindings)
  {
    var prefixedBindings = new Dictionary<string, InputBinding>();
    foreach (var binding in bindings)
    {
      var config = binding.Value;
    }
    return prefixedBindings;
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

}