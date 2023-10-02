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
	public BoardObjectSO BoardObjectSO
	{
		get { return boardObjectSO; }
		private set { boardObjectSO = value; }
	}
	private int health;
	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	private int boardObjectIndex;
	public int BoardObjectIndex
	{
		get { return boardObjectIndex; }
	}

	public delegate void OnDestroyed();
	public event OnDestroyed OnDestroyedEvent;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponentInChildren<BoxCollider2D>();
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			OnDestroyedEvent?.Invoke();
			GameManager.Instance.RemoveObjectAt(transform.position);
		}
	}


	/// <summary>
	/// Initializes the board object with the given BoardObjectSO.
	/// </summary>
	/// <param name="_boardObjectSO">The BoardObjectSO to use for initialization.</param>
	public virtual void Init(BoardObjectSO _boardObjectSO, int _boardObjectIndex = -1)
	{
		BoardObjectSO = _boardObjectSO;
		objectSize = BoardObjectSO.Size;
		spriteRenderer.sprite = BoardObjectSO.Sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;
		health = BoardObjectSO.Health;
		boardObjectIndex = _boardObjectIndex;

		// Moves the spriteObject local position to the bottom left corner
		spriteObject.transform.localPosition = new Vector3(Mathf.CeilToInt(objectSize.x / 2f) + (objectSize.x % 2f == 0 ? 0 : -0.5f), Mathf.CeilToInt(objectSize.y / 2f) + (objectSize.y % 2f == 0 ? 0 : -0.5f), 0);
	}
}
