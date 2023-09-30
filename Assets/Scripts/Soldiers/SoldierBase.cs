using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all soldiers in the game. Provides functionality for initializing soldier properties and positioning the soldier sprite.
/// </summary>
public abstract class SoldierBase : BoardObjectBase
{
	public SoldierSO SoldierSO { get { return boardObjectSO as SoldierSO; } }

	/// <summary>
	/// Initializes the soldier with the given SoldierSO.
	/// </summary>
	/// <param name="_soldierSO">The SoldierSO to use for initialization.</param>
	public virtual void Init(SoldierSO _soldierSO)
	{
		base.Init(_soldierSO);
	}

}
