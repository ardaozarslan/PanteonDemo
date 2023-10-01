using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ScriptableObject representing a building in the game.
/// </summary>
[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingSO : BoardObjectSO
{
	[ReorderableList]
	[SerializeField] private List<BoardObjectSO> _products;

	public override List<BoardObjectSO> Products { get { return _products; } }
}
