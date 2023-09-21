using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	[SerializeField] private Camera _camera;

	private Vector2 lastPosition;

	[SerializeField] private LayerMask gridLayerMask;

	private float currentCameraSize;
	private Vector3 currentCameraPosition;

	private void Start()
	{
		currentCameraSize = _camera.orthographicSize;
		currentCameraPosition = _camera.transform.position;
	}


	// This method will return the position of the mouse on the grid
	public Vector2 GetSelectedMapPosition()
	{
		Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gridLayerMask);

		if (hit.collider != null)
		{
			lastPosition = hit.point;
			return hit.point;
		}
		return lastPosition;
	}

	// This method controls the camera zoom
	public void CameraZoom()
	{
		float scrollData;
		scrollData = Input.GetAxis("Mouse ScrollWheel");
		if (scrollData != 0)
		{
			currentCameraSize = Mathf.Clamp(currentCameraSize - scrollData * 3, 2f, 10f);
		}
		_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, currentCameraSize, Time.deltaTime * 10);
	}

	// This method controls the camera movement
	public void CameraMove()
	{
		Vector3 move = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		currentCameraPosition += 5 * Time.deltaTime * move;
		// _camera.transform.position += 5 * Time.deltaTime * move;
		_camera.transform.position = Vector3.Lerp(_camera.transform.position, currentCameraPosition, Time.deltaTime * 20);
	}

	void Update()
	{

	}

	private void LateUpdate()
	{
		CameraZoom();
		CameraMove();
	}

}
