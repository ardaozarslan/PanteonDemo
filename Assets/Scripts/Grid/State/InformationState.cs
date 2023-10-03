using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public void EndState()
	{
		cellIndicatorSpriteRenderer.size = Vector2.one;
		cellIndicatorSpriteRenderer.color = GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green];
	}

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
			return;
		}

		selectedGameObject = ObjectPlacer.Instance.GetObjectAt(gameObjectIndex);
		EventManager.Instance.ShowInformation(selectedGameObject);

	}

	public void OnSecondaryAction(Vector3Int gridPosition)
	{
		if (InputManager.Instance.IsPointerOverUI())
		{
			return;
		}


		BoardObjectBase selectedBoardObject = selectedGameObject.GetComponent<BoardObjectBase>();

		if (selectedGameObject == null || selectedBoardObject is not SoldierBase)
		{
			return;
		}

		// TODO: implement soldier movement
		Debug.Log("soldier right clicked");
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
			(selectedBoardObject as SoldierBase).SetTargetObject(_placementData);
		}
		return;
	}

	public void UpdateState(Vector3Int gridPosition, Vector3Int lastGridPosition)
	{
		PlacementData data = objectData.GetObjectAt(gridPosition);

		cellIndicator.transform.position = data != null ? grid.CellToWorld(data.GridPosition) : grid.CellToWorld(gridPosition);
		cellIndicatorSpriteRenderer.size = data != null ? new Vector2(data.BoardObjectSO.Size.x, data.BoardObjectSO.Size.y) : Vector2.one;
		cellIndicatorSpriteRenderer.color = data != null ? GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Yellow] : GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green];

	}
}
