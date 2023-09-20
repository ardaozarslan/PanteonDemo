using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingSO : ScriptableObject
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private Vector2Int _size;
	[SerializeField] private Sprite _sprite;

	public GameObject Prefab { get { return _prefab; } }
	public Vector2Int Size { get { return _size; } }
	public Sprite Sprite { get { return _sprite; } }
}
