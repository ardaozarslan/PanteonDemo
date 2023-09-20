using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the base class for all buildings
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
		col2d = GetComponentInChildren<BoxCollider2D>();
	}


	public virtual void Init(Vector2Int _buildingSize, Sprite _sprite)
	{
		buildingSize = _buildingSize;
		spriteRenderer.sprite = _sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;

		spriteObject.transform.localPosition = new Vector3(- _buildingSize.x / 2f, - _buildingSize.y / 2f, 0);
	}

}
