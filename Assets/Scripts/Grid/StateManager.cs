using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class that handles the placement of buildings on a grid.
/// </summary>
public class StateManager : Singleton<StateManager>
{
	// [SerializeField] GameObject boardObjectsParent;
	public CellIndicator cellIndicator;
	public PreviewObject previewObject;

	[SerializeField] private Grid grid;

	private GridData objectData;

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
		previewObject.gameObject.SetActive(false);
		StopPlacement();
		objectData = new();
		StartInformation();
	}

	/// <summary>
	/// Starts the placement of the selected board object.
	/// </summary>
	/// <param name="_selectedBoardObjectSO">The selected board object scriptable object.</param>
	public void StartPlacement(BoardObjectSO _selectedBoardObjectSO)
	{
		StopPlacement();
		StopInformation();

		gameState = new PlacementState(_selectedBoardObjectSO, grid, objectData, cellIndicator, previewObject);

		InputManager.Instance.OnMouseLeftClick += PlaceBoardObject;
		InputManager.Instance.OnExit += StopPlacement;
	}

	/// <summary>
	/// Places the selected board object on the grid at the position of the mouse cursor.
	/// </summary>
	private void PlaceBoardObject()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

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

	public void StartInformation()
	{
		StopInformation();
		gameState = new InformationState(grid, objectData, cellIndicator);

		InputManager.Instance.OnMouseLeftClick += ShowInformation;
		InputManager.Instance.OnMouseRightClick += SecondaryAction;
		InputManager.Instance.OnExit += HideInformation;
	}

	private void ShowInformation() {
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		gameState?.OnAction(gridPosition);
	}

	private void SecondaryAction() {
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		gameState?.OnSecondaryAction(gridPosition);
	}

	private void HideInformation() {
		EventManager.Instance.ShowInformation(null);
	}

	private void StopInformation() {
		gameState?.EndState();
		InputManager.Instance.OnMouseLeftClick -= ShowInformation;
		InputManager.Instance.OnMouseRightClick -= SecondaryAction;
		InputManager.Instance.OnExit -= HideInformation;
		HideInformation();
		gameState = null;
	}

	/// <summary>
	/// This method is called every frame and updates the position and color of the cell indicator based on the current mouse position and placement validity.
	/// </summary>
	private void Update()
	{
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		gameState?.UpdateState(gridPosition, lastGridPosition);

		if (!InputManager.Instance.IsPointerOverUI())
		{
			cellIndicator.gameObject.SetActive(true);
			previewObject.UpdatePosition(grid.CellToWorld(gridPosition));
		}
		else
		{
			cellIndicator.gameObject.SetActive(false);
		}
		lastGridPosition = gridPosition;
	}
}