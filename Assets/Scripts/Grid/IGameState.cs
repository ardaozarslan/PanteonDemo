using UnityEngine;

public interface IGameState
{
    void EndState();
	void OnAction(Vector3Int gridPosition);
	void OnSecondaryAction(Vector3Int gridPosition);
	void UpdateState(Vector3Int gridPosition, Vector3Int lastGridPosition);
}
