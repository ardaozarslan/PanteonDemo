using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;

	[SerializeField] private Grid grid;


	private void Update() {
		Vector2 mousePosition = InputManager.Instance.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		// This will make the cell indicator follow the mouse
		cellIndicator.transform.position = (Vector2)grid.CellToWorld(gridPosition);
	}
}
