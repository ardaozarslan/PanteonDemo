using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Camera _camera;

	private Vector2 lastPosition;

	[SerializeField] private LayerMask gridLayerMask;


	// This method will return the position of the mouse on the grid
	public Vector2 GetSelectedMapPosition() {
		Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, -Vector2.up, Mathf.Infinity, gridLayerMask);

		if (hit.collider != null) {
			lastPosition = hit.point;
			return hit.point;
		}
		return lastPosition;
	}
}
