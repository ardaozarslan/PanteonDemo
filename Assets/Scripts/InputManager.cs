using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Singleton class that manages user input for the game.
/// </summary>
public class InputManager : Singleton<InputManager>
{
	[SerializeField] private Camera _camera;

	private Vector2 lastPosition;

	[SerializeField] private LayerMask gridLayerMask;

	private float currentCameraSize;
	private Vector3 currentCameraPosition;

	public event Action OnMouseLeftClick, OnMouseRightClick, OnExit;

	private void Start()
	{
		currentCameraSize = _camera.orthographicSize;
		currentCameraPosition = _camera.transform.position;
	}

	/// <summary>
	/// Returns the position of the selected map point based on the current mouse position.
	/// </summary>
	/// <returns>The position of the selected map point.</returns>
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

	/// <summary>
	/// Zooms the camera in and out based on the mouse scroll wheel input.
	/// </summary>
	public void CameraZoom()
	{
		float scrollData;
		scrollData = Input.GetAxis("Mouse ScrollWheel");
		if (scrollData != 0 && !IsPointerOverUI())
		{
			currentCameraSize = Mathf.Clamp(currentCameraSize - scrollData * 5, 2f, 20f);
		}
		_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, currentCameraSize, Time.deltaTime * 5);
	}

	/// <summary>
	/// Moves the camera based on the user's input.
	/// </summary>
	public void CameraMove()
	{
		Vector3 move = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		currentCameraPosition += 10 * Time.deltaTime * move;
		// _camera.transform.position += 5 * Time.deltaTime * move;
		_camera.transform.position = Vector3.Lerp(_camera.transform.position, currentCameraPosition, Time.deltaTime * 20);
	}

	/// <summary>
	/// Checks if the pointer is currently over a UI element.
	/// </summary>
	/// <returns>True if the pointer is over a UI element, false otherwise.</returns>
	public bool IsPointerOverUI()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}

	/// <summary>
	/// This method is called every frame and checks for mouse left click and escape key press events.
	/// </summary>
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnMouseLeftClick?.Invoke();
		}
		if (Input.GetMouseButtonDown(1))
		{
			OnMouseRightClick?.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnExit?.Invoke();
		}
	}

	private void LateUpdate()
	{
		CameraZoom();
		CameraMove();
	}

}
