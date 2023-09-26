using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject representing a soldier in the game.
/// </summary>
[CreateAssetMenu(fileName = "Soldier", menuName = "ScriptableObjects/Soldier", order = 1)]
public class SoldierSO : ScriptableObject
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private string _name;
	[SerializeField] private Vector2Int _size;
	[SerializeField] private Sprite _sprite;
	[SerializeField] private int _health = 10;
	[SerializeField] private int _damage;

	public GameObject Prefab { get { return _prefab; } }
	public string Name { get { return _name; } }
	public Vector2Int Size { get { return _size; } }
	public Sprite Sprite { get { return _sprite; } }
	public int Health { get { return _health; } }
	public int Damage { get { return _damage; } }
}
