using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all board objects in the game. Provides functionality for initializing board object properties and positioning the board object sprite.
/// </summary>
public abstract class BoardObjectBase : MonoBehaviour
{
	protected Vector2Int objectSize;
	public Vector2Int ObjectSize { get { return objectSize; } }

	[SerializeField] protected GameObject spriteObject;
	protected SpriteRenderer spriteRenderer;
	protected BoxCollider2D col2d;
	protected BoardObjectSO boardObjectSO;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponentInChildren<BoxCollider2D>();
	}

	/// <summary>
	/// Initializes the board object with the given BoardObjectSO.
	/// </summary>
	/// <param name="_boardObjectSO">The BoardObjectSO to use for initialization.</param>
	public virtual void Init(BoardObjectSO _boardObjectSO)
	{
		boardObjectSO = _boardObjectSO;
		objectSize = boardObjectSO.Size;
		spriteRenderer.sprite = boardObjectSO.Sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;

		// Moves the spriteObject local position to the bottom left corner
		spriteObject.transform.localPosition = new Vector3(Mathf.CeilToInt(objectSize.x / 2f) + (objectSize.x % 2f == 0 ? 0 : -0.5f), Mathf.CeilToInt(objectSize.y / 2f) + (objectSize.y % 2f == 0 ? 0 : -0.5f), 0);
	}
}
