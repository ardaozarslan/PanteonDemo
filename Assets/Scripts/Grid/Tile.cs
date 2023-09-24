using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a tile in the game grid.
/// </summary>
public class Tile : MonoBehaviour
{
	[SerializeField] private Color _baseColor, _offsetColor;
	private Vector2Int gridPos;

	/// <summary>
	/// Initializes the tile with the given grid position and sets its name and color based on the position.
	/// </summary>
	/// <param name="_gridPos">The grid position of the tile.</param>
	public void Init(Vector2Int _gridPos)
	{
		gridPos = _gridPos;
		name = $"Tile ({gridPos.x}, {gridPos.y})";
		bool isOffset = (gridPos.x + gridPos.y) % 2 == 0;
		GetComponent<SpriteRenderer>().color = isOffset ? _offsetColor : _baseColor;
	}
}
