using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Base class for all soldiers in the game. Provides functionality for initializing soldier properties and positioning the soldier sprite.
/// </summary>
public abstract class SoldierBase : BoardObjectBase
{
	public SoldierSO SoldierSO { get { return boardObjectSO as SoldierSO; } }
	private Tile currentTile;
	public Tile CurrentTile { get { return currentTile; } private set { currentTile = value; } }
	[HideInInspector] public Tile targetTile;
	[HideInInspector] public Tile lastTargetTile;

	private int damage;
	public int Damage { get { return damage; } private set { damage = value; } }

	private readonly List<Vector3Int> occupiedPositions = new();
	private BoardObjectBase targetBoardObject;
	public BoardObjectBase TargetBoardObject
	{
		get { return targetBoardObject; }
		set
		{
			targetBoardObject = value;
			if (targetBoardObject != null)
			{
				targetBoardObject.OnDestroyedEvent += OnTargetDestroyed;
			}
		}
	}
	[HideInInspector] public BoardObjectBase lastTargetBoardObject;

	private bool isMoving = false;
	private float moveSpeed = 1f;

	private List<Tile> path;
	public List<Tile> Path { get; private set; }

	private Coroutine lastMoveCoroutine;

	/// <summary>
	/// Called when the target of the soldier is destroyed. Cancels the soldier's movement.
	/// </summary>
	private void OnTargetDestroyed()
	{
		// Cancel movement
		TargetBoardObject = null;
	}

	/// <summary>
	/// Sets the target tile for the soldier and initiates a move command if the soldier is not already moving.
	/// </summary>
	/// <param name="_targetTile">The target tile to set.</param>
	public void SetTargetTile(Tile _targetTile)
	{
		targetTile = _targetTile;
		TargetBoardObject = null;

		if (!isMoving)
		{
#if UNITY_EDITOR
			Debug.Log("soldier is not moving, so new target assigned and move command is given");
#endif
			lastTargetTile = _targetTile;
			MoveCommand();
		}
	}

	/// <summary>
	/// Sets the target object for the soldier to move towards.
	/// </summary>
	/// <param name="_targetBoardObjectPlacementData">The placement data of the target board object.</param>
	public void SetTargetObject(PlacementData _targetBoardObjectPlacementData)
	{
		TargetBoardObject = null;
		Tile _tempTargetTile = Pathfinding.FindClosestEmptyTile(CurrentTile, _targetBoardObjectPlacementData.OccupiedPositions);
		if (_tempTargetTile == null)
		{
#if UNITY_EDITOR
			Debug.Log("no empty tile found, so no target assigned");
#endif
			return;
		}
		TargetBoardObject = ObjectPlacer.Instance.GetObjectAt(_targetBoardObjectPlacementData.PlacedObjectIndex).GetComponent<BoardObjectBase>();
		targetTile = _tempTargetTile;
		if (!isMoving)
		{
#if UNITY_EDITOR
			Debug.Log("soldier is not moving, so new target assigned and move command is given");
#endif
			lastTargetBoardObject = TargetBoardObject;
			lastTargetTile = targetTile;
			MoveCommand();
		}
	}

	/// <summary>
	/// Moves the soldier along the path to the last target tile.
	/// If the soldier reaches the last target tile, it will attack the last target board object if it exists.
	/// </summary>
	private void MoveCommand()
	{
		Path = Pathfinding.FindPath(CurrentTile, lastTargetTile);
		if (lastMoveCoroutine != null)
		{
			StopCoroutine(lastMoveCoroutine);
			lastMoveCoroutine = null;
		}
		if (CurrentTile == lastTargetTile)
		{
			if (lastTargetBoardObject != null)
			{
				AttackCommand();
			}
			return;
		}
		lastMoveCoroutine = StartCoroutine(MoveAlongPath());
	}

	/// <summary>
	/// Moves the soldier along the path to the target tile or board object.
	/// </summary>
	/// <returns>An IEnumerator representing the coroutine.</returns>
	private IEnumerator MoveAlongPath()
	{
		isMoving = true;
		if (Path == null || Path.Count == 0)
		{
			Debug.Log("return to current tile");
			yield return StartCoroutine(MoveToTile(CurrentTile));
			isMoving = false;
			yield break;
		}
		StartCoroutine(MoveToTile(Path[0]));
		yield return new WaitUntil(() => CurrentTile == Path[0]);
		isMoving = false;
		if (lastTargetTile != targetTile)
		{
			lastTargetTile = targetTile;
		}
		if (lastTargetBoardObject != TargetBoardObject)
		{
			lastTargetBoardObject = TargetBoardObject;
		}
		if (lastTargetBoardObject != null)
		{
			List<Vector3Int> _occupiedPositions = new();
			if (lastTargetBoardObject is BuildingBase)
			{
				PlacementData _placementData = null;
				_placementData = GameManager.Instance.ObjectData.GetObjectAt(lastTargetBoardObject.GridPosition);
				_occupiedPositions = _placementData.OccupiedPositions;
			}
			else if (lastTargetBoardObject is SoldierBase)
			{
				_occupiedPositions = (lastTargetBoardObject as SoldierBase).occupiedPositions;
			}
			Tile _tempTargetTile = Pathfinding.FindClosestEmptyTile(CurrentTile, _occupiedPositions);
			if (_tempTargetTile != null)
			{
				lastTargetTile = _tempTargetTile;
			}
		}
		if (CurrentTile != lastTargetTile)
		{
			Path = Pathfinding.FindPath(CurrentTile, lastTargetTile);
			yield return StartCoroutine(MoveAlongPath());
		}
		else if (lastTargetBoardObject != null)
		{
			AttackCommand();
		}
	}

	/// <summary>
	/// Executes an attack command on the last target board object.
	/// </summary>
	private void AttackCommand()
	{
		if (lastTargetBoardObject == null)
		{
			return;
		}
		if (lastTargetBoardObject is BuildingBase)
		{
			(lastTargetBoardObject as BuildingBase).TakeDamage(damage);
		}
		else if (lastTargetBoardObject is SoldierBase)
		{
			(lastTargetBoardObject as SoldierBase).TakeDamage(damage);
		}
	}


	/// <summary>
	/// Moves the soldier to the given tile.
	/// </summary>
	/// <param name="tile">The tile to move to.</param>
	private IEnumerator MoveToTile(Tile tile)
	{
		OccupyNewPosition(tile);
		Vector3 direction = GridManager.Instance.Grid.CellToWorld(tile.GridPos) - GridManager.Instance.Grid.CellToWorld(CurrentTile.GridPos);
		
		transform.DOMove(transform.position + direction, 0.1f / moveSpeed).SetEase(Ease.Linear);

		yield return new WaitForSeconds(0.1f / moveSpeed);
		// Set the soldier's current tile to the target tile
		RemoveFromOccupationList(CurrentTile);
		CurrentTile = tile;
	}

	/// <summary>
	/// Occupies a new position on the game board.
	/// </summary>
	/// <param name="tile">The tile to occupy.</param>
	private void OccupyNewPosition(Tile tile)
	{
		GameManager.Instance.ObjectData.AddToOccupationList(BoardObjectIndex, occupiedPositions, tile.GridPos);
		occupiedPositions.Add(tile.GridPos);
	}

	/// <summary>
	/// Removes the given tile from the list of occupied positions and updates the GameManager's ObjectData accordingly.
	/// </summary>
	/// <param name="tile">The tile to remove from the list of occupied positions.</param>
	private void RemoveFromOccupationList(Tile tile)
	{
		GameManager.Instance.ObjectData.RemoveFromOccupationList(BoardObjectIndex, occupiedPositions, tile.GridPos);
		occupiedPositions.Remove(tile.GridPos);
	}

	/// <summary>
	/// Initializes the soldier with the given SoldierSO.
	/// </summary>
	/// <param name="_soldierSO">The SoldierSO to use for initialization.</param>
	public override void Init(BoardObjectSO _soldierSO, int _boardObjectIndex = -1)
	{
		base.Init(_soldierSO, _boardObjectIndex);
		CurrentTile = GridManager.Instance.GetTileAtPosition(transform.position);
		occupiedPositions.Add(GridManager.Instance.Grid.WorldToCell(transform.position));
		damage = SoldierSO.Damage;
	}

}
