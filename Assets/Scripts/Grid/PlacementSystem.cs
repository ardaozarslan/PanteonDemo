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

	private BuildingSO selectedBuildingSO, lastSelectedBuildingSO;

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
		lastSelectedBuildingSO = null;
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
			previewObject.SetActive(true);
			if (selectedBuildingSO != lastSelectedBuildingSO)
			{
				previewObject.GetComponent<PreviewObject>().Init(selectedBuildingSO);
			}
			if (gridPosition != lastGridPosition)
			{
				previewObject.transform.position = grid.CellToWorld(gridPosition);
			}
			bool placementValidity = CheckPlacementValidity(gridPosition);
			cellIndicatorSpriteRenderer.size = new Vector2(selectedBuildingSO.Size.x, selectedBuildingSO.Size.y);
			cellIndicator.transform.position = grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.color = placementValidity ? indicatorColors[IndicatorColor.Green] : indicatorColors[IndicatorColor.Red];
		}
		else
		{
			if (gridPosition != lastGridPosition)
			{
				previewObject.SetActive(false);
			}
			cellIndicator.transform.position = grid.CellToWorld(gridPosition);
			cellIndicatorSpriteRenderer.size = data != null ? new Vector2(data.BuildingSO.Size.x, data.BuildingSO.Size.y) : Vector2.one;
			cellIndicatorSpriteRenderer.color = data != null ? indicatorColors[IndicatorColor.Yellow] : indicatorColors[IndicatorColor.Green];
		}

		lastGridPosition = gridPosition;
		lastSelectedBuildingSO = selectedBuildingSO;
	}
}