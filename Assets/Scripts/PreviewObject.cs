using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : BuildingBase
{
	public override void Init(BoardObjectSO _boardObjectSO)
	{
		base.Init(_boardObjectSO);
		Color color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
	}
}
