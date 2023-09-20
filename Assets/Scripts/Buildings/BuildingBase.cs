using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
	private Vector2Int buildingSize;
	public Vector2Int BuildingSize { get { return buildingSize; } }

	[SerializeField] protected GameObject spriteObject;
	protected SpriteRenderer spriteRenderer;
	protected BoxCollider2D col2d;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponent<BoxCollider2D>();
	}


	public virtual void Init(Vector2Int _buildingSize, Sprite _sprite)
	{
		buildingSize = _buildingSize;
		spriteRenderer.sprite = _sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;
	}

}
