using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all soldiers in the game. Provides functionality for initializing soldier properties and positioning the soldier sprite.
/// </summary>
public abstract class SoldierBase : MonoBehaviour
{
	private Vector2Int soldierSize;
	public Vector2Int SoldierSize { get { return soldierSize; } }

	[SerializeField] protected GameObject spriteObject;
	protected SpriteRenderer spriteRenderer;
	protected BoxCollider2D col2d;
	protected SoldierSO soldierSO;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponentInChildren<BoxCollider2D>();
	}

	/// <summary>
	/// Initializes the soldier with the given SoldierSO.
	/// </summary>
	/// <param name="_soldierSO">The SoldierSO to use for initialization.</param>
	public virtual void Init(SoldierSO _soldierSO)
	{
		soldierSO = _soldierSO;
		soldierSize = soldierSO.Size;
		spriteRenderer.sprite = soldierSO.Sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;

		// Moves the spriteObject local position to the bottom left corner
		spriteObject.transform.localPosition = new Vector3(Mathf.CeilToInt(soldierSize.x / 2f) + (soldierSize.x % 2f == 0 ? 0 : -0.5f), Mathf.CeilToInt(soldierSize.y / 2f) + (soldierSize.y % 2f == 0 ? 0 : -0.5f), 0);
	}

}
