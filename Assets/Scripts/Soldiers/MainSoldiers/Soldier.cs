using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : SoldierBase
{
    public override void Init(BoardObjectSO _soldierSO, int _boardObjectIndex = -1)
	{
		base.Init(_soldierSO, _boardObjectIndex);
	}
}
