namespace GodotMultiplayerInput;

public interface IInputManager
{
  PlayerInput GetPlayer(int playerId);
}