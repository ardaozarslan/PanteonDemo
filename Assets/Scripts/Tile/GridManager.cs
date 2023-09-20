using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{

	[SerializeField] private int _width, _height;
	[SerializeField] private Tile _tilePrefab;
	[SerializeField] private Transform _cam;

	private Dictionary<Vector2Int, Tile> _tiles;

	private void Start()
	{
		GenerateGrid();
	}

	// This is a method that will generate a grid of tiles
	private void GenerateGrid()
	{
		_tiles = new();
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				Tile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
				tile.Init(new Vector2Int(x, y));

				_tiles.Add(new Vector2Int(x, y), tile);
			}
		}
		// This will set the camera to the center of the grid
		_cam.transform.position = new Vector3(_width / 2f - 0.5f, _height / 2f - 0.5f, -10);
	}

	// This method will return the tile at a given position
	public Tile GetTileAtPosition(Vector2Int gridPos)
	{
		if (_tiles.ContainsKey(gridPos))
		{
			return _tiles[gridPos];
		}
		return null;
	}
}