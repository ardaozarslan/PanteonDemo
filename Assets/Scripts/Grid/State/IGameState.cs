using UnityEngine;

/// <summary>
/// Interface for game states. Defines methods for ending the state, performing actions, and updating the state.
/// </summary>
public interface IGameState
{
    void EndState();
	void OnAction(Vector3Int gridPosition);
	void OnSecondaryAction(Vector3Int gridPosition);
	void UpdateState(Vector3Int gridPosition, Vector3Int lastGridPosition);
}
