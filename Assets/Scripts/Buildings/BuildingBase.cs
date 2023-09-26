using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all buildings in the game. Provides functionality for initializing building properties and positioning the building sprite.
/// </summary>
public abstract class BuildingBase : MonoBehaviour
{
	private Vector2Int buildingSize;
	public Vector2Int BuildingSize { get { return buildingSize; } }

	[SerializeField] protected GameObject spriteObject;
	protected SpriteRenderer spriteRenderer;
	protected BoxCollider2D col2d;
	protected BuildingSO buildingSO;

	private void Awake()
	{
		spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
		col2d = GetComponentInChildren<BoxCollider2D>();
	}

	/// <summary>
	/// Initializes the building with the given BuildingSO.
	/// </summary>
	/// <param name="_buildingSO">The BuildingSO to use for initialization.</param>
	public virtual void Init(BuildingSO _buildingSO)
	{
		buildingSO = _buildingSO;
		buildingSize = buildingSO.Size;
		spriteRenderer.sprite = buildingSO.Sprite;
		col2d.size = spriteRenderer.sprite.bounds.size;

		// Moves the spriteObject local position to the bottom left corner
		spriteObject.transform.localPosition = new Vector3(Mathf.CeilToInt(buildingSize.x / 2f) + (buildingSize.x % 2f == 0 ? 0 : -0.5f), Mathf.CeilToInt(buildingSize.y / 2f) + (buildingSize.y % 2f == 0 ? 0 : -0.5f), 0);
	}

}
