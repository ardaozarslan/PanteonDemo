using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject representing a soldier in the game.
/// </summary>
[CreateAssetMenu(fileName = "Soldier", menuName = "ScriptableObjects/Soldier", order = 1)]
public class SoldierSO : BoardObjectSO
{
	[SerializeField] private int _damage;

	public int Damage { get { return _damage; } }
}
