using System.Collections.Generic;
using Godot;

namespace GodotMultiplayerInput;

public class InputBinding
{
  public float Deadzone { get; private set; }
  public List<InputEvent> Events { get; private set; }
  public InputBinding(Variant binding_config)
  {
    var config = binding_config.AsGodotDictionary<string, Variant>();
    Deadzone = config["deadzone"].AsSingle();
    var events = config["events"].AsGodotArray<Variant>();
    Events = new List<InputEvent>();
    foreach (var eventVariant in events)
    {
      var @event = eventVariant.AsGodotObject();
      if (@event is InputEvent inputEvent)
      {
        Events.Add(inputEvent);
      }
    }
  }
}