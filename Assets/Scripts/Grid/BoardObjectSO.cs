using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using NaughtyAttributes;

/// <summary>
/// ScriptableObject representing a building in the game.
/// </summary>
public class BoardObjectSO : ScriptableObject
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private string _name;
	[SerializeField] private Vector2Int _size;
	[SerializeField] private Sprite _sprite;
	[SerializeField] private int _health;

	public GameObject Prefab { get { return _prefab; } }
	public string Name { get { return _name; } }
	public Vector2Int Size { get { return _size; } }
	public Sprite Sprite { get { return _sprite; } }
	public int Health { get { return _health; } }
	public virtual List<BoardObjectSO> Products { get { return new(); } }
}
