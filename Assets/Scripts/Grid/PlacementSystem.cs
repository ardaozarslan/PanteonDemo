using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class that handles the placement of buildings on a grid.
/// </summary>
public class PlacementSystem : Singleton<PlacementSystem>
{
	[SerializeField] GameObject buildingsParent;
	[SerializeField] private GameObject cellIndicator;
	private SpriteRenderer cellIndicatorSpriteRenderer;

	[SerializeField] private Grid grid;

	private BuildingSO selectedBuildingSO = null;

	private GridData objectData;

	private List<GameObject> placedObjects = new();

	private Dictionary<IndicatorColor, Color> indicatorColors = new(){
		{ IndicatorColor.Red, new Color(1, 0, 0, 0.6f) },
		{ IndicatorColor.Green, new Color(0, 1, 0, 0.6f) },
		{ IndicatorColor.Yellow, new Color(1, 1, 0, 0.6f) }
	};

	private void Start()
	{
		StopPlacement();
		objectData = new();
		cellIndicatorSpriteRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
	}

	/// <summary>
	/// Starts the placement of the selected building.
	/// </summary>
	/// <param name="_selectedBuildingSO">The selected building scriptable object.</param>
	public void StartPlacement(BuildingSO _selectedBuildingSO)
	{
		StopPlacement();
		selectedBuildingSO = _selectedBuildingSO;
		if (selectedBuildingSO == null)
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
		if (InputManager.Instance.IsPinterOverUI())
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

		GameObject newBuilding = Instantiate(selectedBuildingSO.Prefab, grid.CellToWorld(gridPosition), Quaternion.identity, buildingsParent.transform);
		newBuilding.transform.position = (Vector2)grid.CellToWorld(gridPosition);
		newBuilding.GetComponent<BuildingBase>().Init(selectedBuildingSO);
		placedObjects.Add(newBuilding);
		objectData.AddObjectAt(gridPosition, selectedBuildingSO);
	}

	/// <summary>
	/// Checks if the placement of the object at the given grid position is valid.
	/// </summary>
	/// <param name="gridPosition">The grid position to check.</param>
	/// <returns>True if the placement is valid, false otherwise.</returns>
	private bool CheckPlacementValidity(Vector3Int gridPosition)
	{
		return objectData.CanPlaceObjectAt(gridPosition, selectedBuildingSO.Size);
	}

	/// <summary>
	/// Stops the placement of the selected building and removes event listeners.
	/// </summary>
	private void StopPlacement()
	{
		selectedBuildingSO = null;
		InputManager.Instance.OnMouseLeftClick -= PlaceBuilding;
		InputManager.Instance.OnExit -= StopPlacement;
	}

	/// <summary>
	/// This method is called every frame and updates the position and color of the cell indicator based on the current mouse position and placement validity.
	/// </summary>
	private void Update()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		PlacementData data = objectData.GetObjectAt(gridPosition);
		if (selectedBuildingSO != null)
		{
			bool placementValidity = CheckPlacementValidity(gridPosition);
			cellIndicatorSpriteRenderer.size = new Vector2(selectedBuildingSO.Size.x, selectedBuildingSO.Size.y);
			cellIndicator.transform.position = (Vector2)grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.color = placementValidity ? indicatorColors[IndicatorColor.Green] : indicatorColors[IndicatorColor.Red];
		}
		else if (data != null)
		{
			cellIndicator.transform.position = grid.CellToWorld(data.GridPosition);
			cellIndicatorSpriteRenderer.size = new Vector2(data.BuildingSO.Size.x, data.BuildingSO.Size.y);
			cellIndicatorSpriteRenderer.color = indicatorColors[IndicatorColor.Yellow];
		}
		else
		{
			cellIndicatorSpriteRenderer.size = Vector2.one;
			cellIndicator.transform.position = (Vector2)grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.color = indicatorColors[IndicatorColor.Green];
		}
	}
}

enum IndicatorColor
{
	Red,
	Green,
	Yellow
}