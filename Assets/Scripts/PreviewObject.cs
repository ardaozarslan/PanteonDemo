using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : BoardObjectBase
{
	public override void Init(BoardObjectSO _buildingSO, int _boardObjectIndex = -1)
	{
		base.Init(_buildingSO, _boardObjectIndex);
		Color color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
	}

	public void UpdatePosition(Vector3 position)
	{
		transform.position = position;
	}
}
