using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Represents a tile in the game grid.
/// </summary>
public class Tile : MonoBehaviour
{
	[SerializeField] private Color _baseColor, _offsetColor;
	private Vector3Int gridPos;
	public Vector3Int GridPos { get { return gridPos; }}

	public List<Tile> Neighbors { get; private set; }
	public Tile Connection { get; private set; }
	public float G { get; private set; }
	public float H { get; private set; }
	public float F => G + H;

	public Action OnWalkableChanged;

	private bool _walkable;
	public bool Walkable
	{
		get 
		{
			bool newValue = GameManager.Instance.ObjectData.IsWalkable((Vector3Int)gridPos);
			if (newValue != _walkable)
			{
				_walkable = newValue;
				OnWalkableChanged?.Invoke();
			}
			return _walkable;
		}
	}

	private readonly List<Vector2Int> Dirs = new() {
			new(0, 1), new(-1, 0), new(0, -1), new(1, 0)
			// new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1)
		};

	/// <summary>
	/// Initializes the tile with the given grid position and sets its name and color based on the position.
	/// </summary>
	/// <param name="_gridPos">The grid position of the tile.</param>
	public void Init(Vector2Int _gridPos)
	{
		// gridPos = _gridPos;
		gridPos = GridManager.Instance.Grid.WorldToCell(transform.position);
		name = $"Tile ({gridPos.x}, {gridPos.y})";
		bool isOffset = (gridPos.x + gridPos.y) % 2 == 0;
		GetComponent<SpriteRenderer>().color = isOffset ? _offsetColor : _baseColor;
	}

	/// <summary>
	/// Caches the neighboring tiles of this tile.
	/// </summary>
	public void CacheNeighbors()
	{
		Neighbors = new List<Tile>();
		foreach (var tile in Dirs.Select(dir => GridManager.Instance.GetTileAtPosition((Vector2Int)gridPos + dir)).Where(tile => tile != null))
		{
			Neighbors.Add(tile);
		}
	}

	public void SetConnection(Tile _tile)
	{
		Connection = _tile;
	}

	public void SetG(float g)
	{
		G = g;
	}

	public void SetH(float h)
	{
		H = h;
	}

	/// <summary>
	/// Calculates the distance between this tile and another tile using the A* algorithm.
	/// </summary>
	/// <param name="_tile">The tile to calculate the distance to.</param>
	/// <returns>The distance between the two tiles.</returns>
	public float GetDistance(Tile _tile)
	{
		var dist = new Vector2Int(Mathf.Abs(gridPos.x - _tile.GridPos.x), Mathf.Abs(gridPos.y - _tile.GridPos.y));

		var lowest = Mathf.Min(dist.x, dist.y);
		var highest = Mathf.Max(dist.x, dist.y);

		var horizontalMovesRequired = highest - lowest;

		return lowest * 14 + horizontalMovesRequired * 10;
	}
}
