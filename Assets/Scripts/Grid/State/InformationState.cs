using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state of the game where the player is selecting a cell to get information about the object placed on it and has access to controlling it.
/// </summary>
public class InformationState : IGameState
{
	private int gameObjectIndex = -1;
	GameObject selectedGameObject = null;
	Grid grid;
	GridData objectData;
	CellIndicator cellIndicator;
	SpriteRenderer cellIndicatorSpriteRenderer;

	public InformationState(Grid _grid, GridData _objectData, CellIndicator _cellIndicator)
	{
		grid = _grid;
		objectData = _objectData;
		cellIndicator = _cellIndicator;
		cellIndicatorSpriteRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
	}

	/// <summary>
	/// Ends the current state by resetting the cell indicator sprite renderer's size to Vector2.one and setting its color to green.
	/// </summary>
	public void EndState()
	{
		cellIndicatorSpriteRenderer.size = Vector2.one;
		cellIndicatorSpriteRenderer.color = GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green];
	}

	/// <summary>
	/// Handles the action to be taken when a grid position is clicked.
	/// </summary>
	/// <param name="gridPosition">The position of the clicked grid.</param>
	public void OnAction(Vector3Int gridPosition)
	{
		if (InputManager.Instance.IsPointerOverUI())
		{
			return;
		}

		gameObjectIndex = objectData.GetRepresentationIndex(gridPosition);
		if (gameObjectIndex == -1)
		{
			EventManager.Instance.ShowInformation(null);
			selectedGameObject = null;
			return;
		}

		selectedGameObject = ObjectPlacer.Instance.GetObjectAt(gameObjectIndex);
		EventManager.Instance.ShowInformation(selectedGameObject);

	}

	/// <summary>
	/// Handles the secondary action for the selected soldier on the grid.
	/// </summary>
	/// <param name="gridPosition">The position of the grid where the action is performed.</param>
	public void OnSecondaryAction(Vector3Int gridPosition)
	{
		if (InputManager.Instance.IsPointerOverUI())
		{
			return;
		}
		if (selectedGameObject == null)
		{
			return;
		}
		BoardObjectBase selectedBoardObject = selectedGameObject.GetComponent<BoardObjectBase>();
		if (selectedBoardObject is not SoldierBase)
		{
			return;
		}

		PlacementData _placementData = objectData.GetObjectAt(gridPosition);
		if (_placementData == null)
		{
			(selectedBoardObject as SoldierBase).SetTargetTile(GridManager.Instance.GetTileAtPosition(gridPosition));
		}
		else
		{
			// self check
			int otherGameObjectIndex = objectData.GetRepresentationIndex(gridPosition);
			if (otherGameObjectIndex == -1)
			{
				return;
			}
			GameObject otherGameObject = ObjectPlacer.Instance.GetObjectAt(otherGameObjectIndex);
			if (otherGameObject == null || otherGameObject == selectedGameObject)
			{
				return;
			}
			// self check end
			(selectedBoardObject as SoldierBase).SetTargetObject(_placementData);
		}
		return;
	}

	/// <summary>
	/// Updates the state of the cell indicator based on the object data at the given grid position.
	/// </summary>
	/// <param name="gridPosition">The grid position to update the state for.</param>
	/// <param name="lastGridPosition">The last grid position.</param>
	public void UpdateState(Vector3Int gridPosition, Vector3Int lastGridPosition)
	{
		PlacementData data = objectData.GetObjectAt(gridPosition);

		cellIndicator.transform.position = data != null ? grid.CellToWorld(data.GridPosition) : grid.CellToWorld(gridPosition);
		cellIndicatorSpriteRenderer.size = data != null ? new Vector2(data.BoardObjectSO.Size.x, data.BoardObjectSO.Size.y) : Vector2.one;
		cellIndicatorSpriteRenderer.color = data != null ? GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Yellow] : GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green];

	}
}
