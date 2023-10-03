using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state of the game when the player is placing a board object on the grid.
/// </summary>
public class PlacementState : IGameState
{
	BoardObjectSO selectedBoardObjectSO, lastSelectedBoardObjectSO = null;
	Grid grid;
	GridData objectData;
	CellIndicator cellIndicator;
	PreviewObject previewObject;
	SpriteRenderer cellIndicatorSpriteRenderer;
	bool isOverUI = false;

	public PlacementState(BoardObjectSO _selectedBoardObjectSO, Grid _grid, GridData _objectData, CellIndicator _cellIndicator, PreviewObject _previewObject)
	{

		selectedBoardObjectSO = _selectedBoardObjectSO;
		grid = _grid;
		objectData = _objectData;
		cellIndicator = _cellIndicator;
		previewObject = _previewObject;
		cellIndicatorSpriteRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();

		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);
		previewObject.transform.position = grid.CellToWorld(gridPosition);

		if (selectedBoardObjectSO == null)
		{
#if UNITY_EDITOR
			Debug.LogError("No board object selected");
#endif
			return;
		}
	}

	/// <summary>
	/// Ends the placement state by resetting the selected and last selected board object, hiding the preview object, and resetting the cell indicator sprite renderer.
	/// </summary>
	public void EndState()
	{
		selectedBoardObjectSO = null;
		lastSelectedBoardObjectSO = null;
		previewObject.gameObject.SetActive(false);
		cellIndicatorSpriteRenderer.size = Vector2.one;
		cellIndicatorSpriteRenderer.color = GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green];
	}

	/// <summary>
	/// Handles the action of placing a selected board object on the grid at the given grid position.
	/// </summary>
	/// <param name="gridPosition">The position on the grid where the object will be placed.</param>
	public void OnAction(Vector3Int gridPosition)
	{
		if (InputManager.Instance.IsPointerOverUI())
		{
			return;
		}

		bool placementValidity = CheckPlacementValidity(gridPosition);
		if (!placementValidity)
		{
			return;
		}

		int index = ObjectPlacer.Instance.PlaceObject(selectedBoardObjectSO.Prefab, selectedBoardObjectSO, grid.CellToWorld(gridPosition));
		objectData.AddObjectAt(gridPosition, selectedBoardObjectSO, index);
	}

	public void OnSecondaryAction(Vector3Int gridPosition)
	{
		return;
	}

	/// <summary>
	/// Checks if the placement of the object at the given grid position is valid.
	/// </summary>
	/// <param name="gridPosition">The grid position to check.</param>
	/// <returns>True if the placement is valid, false otherwise.</returns>
	private bool CheckPlacementValidity(Vector3Int gridPosition)
	{
		return objectData.CanPlaceObjectAt(gridPosition, selectedBoardObjectSO.Size);
	}

	/// <summary>
	/// Updates the state of the placement based on the current grid position and the last grid position.
	/// </summary>
	/// <param name="gridPosition">The current grid position.</param>
	/// <param name="lastGridPosition">The last grid position.</param>
	public void UpdateState(Vector3Int gridPosition, Vector3Int lastGridPosition)
	{
		if (selectedBoardObjectSO != null)
		{
			isOverUI = InputManager.Instance.IsPointerOverUI();
			previewObject.gameObject.SetActive(!isOverUI);
			if (selectedBoardObjectSO != lastSelectedBoardObjectSO)
			{
				previewObject.Init(selectedBoardObjectSO);
			}
			if (isOverUI)
			{
				return;
			}
			bool placementValidity = CheckPlacementValidity(gridPosition);
			cellIndicatorSpriteRenderer.size = new Vector2(selectedBoardObjectSO.Size.x, selectedBoardObjectSO.Size.y);
			cellIndicator.transform.position = grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.color = placementValidity ? GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Green] : GameManager.Instance.indicatorColors[GameManager.IndicatorColor.Red];
		}

		lastSelectedBoardObjectSO = selectedBoardObjectSO;
	}
}
