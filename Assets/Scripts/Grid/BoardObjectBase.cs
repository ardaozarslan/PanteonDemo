using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all board objects in the game. Provides functionality for initializing board object properties and positioning the board object sprite.
/// </summary>
public abstract class BoardObjectBase : MonoBehaviour
{
	protected Vector2Int objectSize;
	public Vector2Int ObjectSize { get { return objectSize; } }

	[SerializeField] protected GameObject spriteObject;
	[SerializeField] protected RectTransform healthBar;
	protected CanvasGroup healthBarCanvasGroup;
	[SerializeField] protected CanvasGroup healtBarFillCanvasGroup;
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
	private int maxHealth;

	private int boardObjectIndex;
	public int BoardObjectIndex
	{
		get { return boardObjectIndex; }
	}

	public Vector3Int GridPosition
	{
		get { return GridManager.Instance.Grid.WorldToCell(transform.position + new Vector3(0.1f, 0.1f, 0f)); }
	}

	public delegate void OnDestroyed();
	public event OnDestroyed OnDestroyedEvent;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponentInChildren<BoxCollider2D>();
		healthBarCanvasGroup = healthBar.GetComponent<CanvasGroup>();
		healthBarCanvasGroup.alpha = 0;
	}

	public void TakeDamage(int damage)
	{
		Health = Mathf.Max(Health - damage, 0);
		float healthBarRatio = (float)Health / maxHealth;
		healthBar.GetComponent<Slider>().value = healthBarRatio;
		healthBarCanvasGroup.alpha = 0.5f;
		if (Health <= 0)
		{
			healtBarFillCanvasGroup.alpha = 0;
			OnDestroyedEvent?.Invoke();
			GameManager.Instance.RemoveObjectAt(boardObjectIndex, GridPosition);
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
		maxHealth = health;
		boardObjectIndex = _boardObjectIndex;

		// Moves the spriteObject local position to the bottom left corner
		spriteObject.transform.localPosition = new Vector3(Mathf.CeilToInt(objectSize.x / 2f) + (objectSize.x % 2f == 0 ? 0 : -0.5f), Mathf.CeilToInt(objectSize.y / 2f) + (objectSize.y % 2f == 0 ? 0 : -0.5f), 0);
		healthBar.transform.localPosition = new Vector3(Mathf.CeilToInt(objectSize.x / 2f) + (objectSize.x % 2f == 0 ? 0 : -0.5f), objectSize.y + 0.1f * objectSize.x, 0);
		healthBar.transform.localScale = new Vector3(objectSize.x * 0.006f, objectSize.x * 0.006f, objectSize.x * 0.006f);
	}
}
