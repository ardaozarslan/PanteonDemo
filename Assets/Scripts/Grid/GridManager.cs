using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GridManager class is responsible for generating and managing a grid of tiles.
/// </summary>
public class GridManager : Singleton<GridManager>
{

	[SerializeField] private int _width, _height;
	[SerializeField] private Tile _tilePrefab;
	[SerializeField] private Transform _cam;

	private Dictionary<Vector2Int, Tile> _tiles;

	[SerializeField] private GameObject tilesParent;

	[SerializeField] private Grid grid;
	public Grid Grid { get { return grid; } }

	private void Awake()
	{
		GenerateGrid();
	}

	/// <summary>
	/// Generates a grid of tiles and sets the camera to the center of the grid.
	/// </summary>
	private void GenerateGrid()
	{
		_tiles = new();
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				Tile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, tilesParent.transform);
				tile.Init(new Vector2Int(x, y));

				_tiles.Add(new Vector2Int(x, y), tile);
			}
		}
		// This will set the camera to the center of the grid
		_cam.transform.position = new Vector3(_width / 2f - 0.5f, _height / 2f - 0.5f, -10);

		foreach (var tile in _tiles.Values) tile.CacheNeighbors();
	}

	/// <summary>
	/// Returns the tile at the specified grid position, if it exists.
	/// </summary>
	/// <param name="gridPos">The grid position of the tile to retrieve.</param>
	/// <returns>The tile at the specified grid position, or null if no tile exists at that position.</returns>
	public Tile GetTileAtPosition(Vector2Int gridPos)
	{
		return _tiles.ContainsKey(gridPos) ? _tiles[gridPos] : null;
	}

	/// <summary>
	/// Returns the tile at the given world position, if it exists.
	/// </summary>
	/// <param name="worldPos">The world position to check.</param>
	/// <returns>The tile at the given world position, or null if no tile exists at that position..</returns>
	public Tile GetTileAtPosition(Vector3 worldPos)
	{
		Vector2Int gridPos = (Vector2Int)grid.WorldToCell(worldPos);
		return GetTileAtPosition(gridPos);
	}
}