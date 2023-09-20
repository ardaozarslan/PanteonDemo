using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] private Color _baseColor, _offsetColor;

	// This method will initialize the tile
    public void Init(Vector2Int gridPos) {
		name = $"Tile ({gridPos.x}, {gridPos.y})";
		bool isOffset = (gridPos.x + gridPos.y) % 2 == 0;
		GetComponent<SpriteRenderer>().color = isOffset ? _offsetColor : _baseColor;
	}
}
