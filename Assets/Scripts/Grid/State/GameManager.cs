using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Singleton class that is responsible for managing the game state, initializing the game objects handles, and the placement of buildings on a grid.
/// </summary>
public class GameManager : Singleton<GameManager>
{
	// [SerializeField] GameObject boardObjectsParent;
	public CellIndicator cellIndicator;
	public PreviewObject previewObject;

	public Grid Grid => GridManager.Instance.Grid;

	private GridData objectData;
	public GridData ObjectData { get { return objectData; } }

	private List<BuildingSO> buildings;
	public List<BuildingSO> Buildings { get { return buildings ??= new(Resources.LoadAll<BuildingSO>("Buildings")); } }

	public enum IndicatorColor
	{
		Red,
		Green,
		Yellow
	}
	public readonly Dictionary<IndicatorColor, Color> indicatorColors = new(){
		{ IndicatorColor.Red, new Color(1, 0, 0, 0.6f) },
		{ IndicatorColor.Green, new Color(0, 1, 0, 0.6f) },
		{ IndicatorColor.Yellow, new Color(1, 1, 0, 0.6f) }
	};

	private Vector3Int lastGridPosition = Vector3Int.zero;

	IGameState gameState;

	private void Start()
	{
		StartCoroutine(WaitUntilEndOfFrame());
		previewObject.gameObject.SetActive(false);
		StopPlacement();
		objectData = new();
		StartInformation();
	}

	// This is a workaround to make sure that the production menu is initialized after the UI is scaled
	IEnumerator WaitUntilEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		ProductionMenuManager.Instance.Init();
	}

	private void OnEnable()
	{
		EventManager.Instance.OnSpawnProductEvent += SpawnProduct;
	}

	private void OnDisable()
	{
		EventManager.Instance.OnSpawnProductEvent -= SpawnProduct;
	}

	/// <summary>
	/// Starts the placement of the selected board object.
	/// </summary>
	/// <param name="_selectedBoardObjectSO">The selected board object scriptable object.</param>
	public void StartPlacement(BoardObjectSO _selectedBoardObjectSO)
	{
		StopPlacement();
		StopInformation();

		gameState = new PlacementState(_selectedBoardObjectSO, Grid, objectData, cellIndicator, previewObject);

		InputManager.Instance.OnMouseLeftClick += PlaceBoardObject;
		InputManager.Instance.OnExit += StopPlacement;
	}

	/// <summary>
	/// Places the selected board object on the grid at the position of the mouse cursor.
	/// </summary>
	private void PlaceBoardObject()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = Grid.WorldToCell(mousePosition);

		gameState?.OnAction(gridPosition);
	}

	/// <summary>
	/// Stops the placement of the selected board object and removes event listeners.
	/// </summary>
	private void StopPlacement()
	{
		gameState?.EndState();
		InputManager.Instance.OnMouseLeftClick -= PlaceBoardObject;
		InputManager.Instance.OnExit -= StopPlacement;
		gameState = null;

		StartInformation();
	}

	/// <summary>
	/// Starts the Information state and subscribes to relevant input events.
	/// </summary>
	public void StartInformation()
	{
		StopInformation();
		gameState = new InformationState(Grid, objectData, cellIndicator);

		InputManager.Instance.OnMouseLeftClick += ShowInformation;
		InputManager.Instance.OnMouseRightClick += SecondaryAction;
		InputManager.Instance.OnExit += HideInformation;
	}

	/// <summary>
	/// Shows information about the selected map position and triggers the corresponding game state action.
	/// </summary>
	private void ShowInformation()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = Grid.WorldToCell(mousePosition);

		gameState?.OnAction(gridPosition);
	}

	/// <summary>
	/// Performs the secondary action on the grid based on the current game state.
	/// </summary>
	private void SecondaryAction()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = Grid.WorldToCell(mousePosition);

		gameState?.OnSecondaryAction(gridPosition);
	}

	/// <summary>
	/// Hides the information panel by passing null to the ShowInformation method of the EventManager.
	/// </summary>
	private void HideInformation()
	{
		EventManager.Instance.ShowInformation(null);
	}

	/// <summary>
	/// Stops the current game state and hides any information being displayed.
	/// </summary>
	private void StopInformation()
	{
		gameState?.EndState();
		InputManager.Instance.OnMouseLeftClick -= ShowInformation;
		InputManager.Instance.OnMouseRightClick -= SecondaryAction;
		InputManager.Instance.OnExit -= HideInformation;
		HideInformation();
		gameState = null;
	}

	/// <summary>
	/// Spawns a product on the grid at the position of the given spawner game object.
	/// If there is already a non-building object at the target position, the product will not be spawned.
	/// </summary>
	/// <param name="productSO">The BoardObjectSO of the product to spawn.</param>
	/// <param name="spawnerGameObject">The GameObject that will be used as the spawn position.</param>
	private void SpawnProduct(BoardObjectSO productSO, GameObject spawnerGameObject)
	{
		Vector3Int gridPosition = Grid.WorldToCell(spawnerGameObject.transform.position);

		if (objectData.placedObjects.ContainsKey(gridPosition) && objectData.placedObjects[gridPosition].Count > 0 && objectData.placedObjects[gridPosition].Last().BoardObjectSO is not BuildingSO)
		{
			return;
		}
		int index = ObjectPlacer.Instance.PlaceObject(productSO.Prefab, productSO, spawnerGameObject.transform.position);
		objectData.AddObjectAt(gridPosition, productSO, index);
	}

	/// <summary>
	/// Removes the object at the specified grid position and board object index.
	/// </summary>
	/// <param name="boardObjectIndex">The index of the board object to remove.</param>
	/// <param name="gridPosition">The grid position of the object to remove.</param>
	public void RemoveObjectAt(int boardObjectIndex, Vector3Int gridPosition)
	{
		PlacementData removedObject = objectData.GetObjectAt(gridPosition, boardObjectIndex);
		if (removedObject == null)
		{
#if UNITY_EDITOR
			Debug.LogWarning("No object to remove");
#endif
			return;
		}
		objectData.RemoveObjectAt(boardObjectIndex, gridPosition);
		ObjectPlacer.Instance.RemoveObjectAt(boardObjectIndex);
	}

	/// <summary>
	/// This method is called every frame and updates the position and color of the cell indicator based on the current mouse position and placement validity.
	/// </summary>
	private void Update()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = Grid.WorldToCell(mousePosition);

		gameState?.UpdateState(gridPosition, lastGridPosition);

		if (!InputManager.Instance.IsPointerOverUI())
		{
			cellIndicator.gameObject.SetActive(true);
			previewObject.UpdatePosition(Grid.CellToWorld(gridPosition));
		}
		else
		{
			cellIndicator.gameObject.SetActive(false);
		}
		lastGridPosition = gridPosition;
	}
}