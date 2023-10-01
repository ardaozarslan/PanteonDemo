using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all buildings in the game. Provides functionality for initializing building properties and positioning the building sprite.
/// </summary>
public abstract class BuildingBase : BoardObjectBase
{
	public BuildingSO BuildingSO { get { return boardObjectSO as BuildingSO; } }

	/// <summary>
	/// Initializes the building with the given BuildingSO.
	/// </summary>
	/// <param name="_buildingSO">The BuildingSO to use for initialization.</param>
	public override void Init(BoardObjectSO _buildingSO)
	{
		base.Init(_buildingSO);
	}

}
