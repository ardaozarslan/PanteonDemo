using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class that handles the placement of buildings on a grid.
/// </summary>
public class PlacementSystem : Singleton<PlacementSystem>
{
	[SerializeField] GameObject buildingsParent;
	[SerializeField] private GameObject cellIndicator, previewObject;
	private SpriteRenderer cellIndicatorSpriteRenderer;

	[SerializeField] private Grid grid;

	private BoardObjectSO selectedBoardObjectSO, lastSelectedBoardObjectSO;

	private GridData objectData;

	private List<GameObject> placedObjects = new();

	private enum IndicatorColor
	{
		Red,
		Green,
		Yellow
	}
	private readonly Dictionary<IndicatorColor, Color> indicatorColors = new(){
		{ IndicatorColor.Red, new Color(1, 0, 0, 0.6f) },
		{ IndicatorColor.Green, new Color(0, 1, 0, 0.6f) },
		{ IndicatorColor.Yellow, new Color(1, 1, 0, 0.6f) }
	};

	private Vector3Int lastGridPosition = Vector3Int.zero;

	private void Start()
	{
		previewObject.SetActive(false);
		StopPlacement();
		objectData = new();
		cellIndicatorSpriteRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
	}

	/// <summary>
	/// Starts the placement of the selected building.
	/// </summary>
	/// <param name="_selectedBoardObjectSO">The selected building scriptable object.</param>
	public void StartPlacement(BoardObjectSO _selectedBoardObjectSO)
	{
		StopPlacement();
		selectedBoardObjectSO = _selectedBoardObjectSO;
		if (selectedBoardObjectSO == null)
		{
			Debug.LogError("No building selected");
			return;
		}

		InputManager.Instance.OnMouseLeftClick += PlaceBuilding;
		InputManager.Instance.OnExit += StopPlacement;
	}

	/// <summary>
	/// Places the selected building on the grid at the position of the mouse cursor.
	/// </summary>
	private void PlaceBuilding()
	{
		if (InputManager.Instance.IsPointerOverUI())
		{
			return;
		}
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		bool placementValidity = CheckPlacementValidity(gridPosition);
		if (!placementValidity)
		{
			return;
		}

		GameObject newBuilding = Instantiate(selectedBoardObjectSO.Prefab, grid.CellToWorld(gridPosition), Quaternion.identity, buildingsParent.transform);
		newBuilding.transform.position = (Vector2)grid.CellToWorld(gridPosition);
		newBuilding.GetComponent<BuildingBase>().Init(selectedBoardObjectSO);
		placedObjects.Add(newBuilding);
		objectData.AddObjectAt(gridPosition, selectedBoardObjectSO);
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
	/// Stops the placement of the selected building and removes event listeners.
	/// </summary>
	private void StopPlacement()
	{
		selectedBoardObjectSO = null;
		lastSelectedBoardObjectSO = null;
		InputManager.Instance.OnMouseLeftClick -= PlaceBuilding;
		InputManager.Instance.OnExit -= StopPlacement;
	}

	/// <summary>
	/// This method is called every frame and updates the position and color of the cell indicator based on the current mouse position and placement validity.
	/// </summary>
	private void Update()
	{
		// if (InputManager.Instance.IsPointerOverUI())
		// {
		// 	return;
		// }
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		PlacementData data = objectData.GetObjectAt(gridPosition);
		if (selectedBoardObjectSO != null)
		{
			previewObject.SetActive(true);
			if (selectedBoardObjectSO != lastSelectedBoardObjectSO)
			{
				previewObject.GetComponent<PreviewObject>().Init(selectedBoardObjectSO);
			}
			if (gridPosition != lastGridPosition)
			{
				previewObject.transform.position = grid.CellToWorld(gridPosition);
			}
			bool placementValidity = CheckPlacementValidity(gridPosition);
			cellIndicatorSpriteRenderer.size = new Vector2(selectedBoardObjectSO.Size.x, selectedBoardObjectSO.Size.y);
			cellIndicator.transform.position = grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.color = placementValidity ? indicatorColors[IndicatorColor.Green] : indicatorColors[IndicatorColor.Red];
		}
		else
		{
			if (gridPosition != lastGridPosition)
			{
				previewObject.SetActive(false);
			}
			cellIndicator.transform.position = data != null ? grid.CellToWorld(data.GridPosition) : grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.size = data != null ? new Vector2(data.BoardObjectSO.Size.x, data.BoardObjectSO.Size.y) : Vector2.one;
			cellIndicatorSpriteRenderer.color = data != null ? indicatorColors[IndicatorColor.Yellow] : indicatorColors[IndicatorColor.Green];
		}

		lastGridPosition = gridPosition;
		lastSelectedBoardObjectSO = selectedBoardObjectSO;
	}
}