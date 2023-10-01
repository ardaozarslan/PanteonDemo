using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : Singleton<ObjectPlacer>
{
	private List<GameObject> placedGameObjects = new();
	[SerializeField] private GameObject boardObjectsParent;

	public int PlaceObject(GameObject prefab, BoardObjectSO selectedBoardObjectSO, Vector3 objectPosition)
	{
		GameObject newBoardObject = Instantiate(prefab, Vector3Int.zero, Quaternion.identity, boardObjectsParent.transform);
		newBoardObject.transform.position = objectPosition;

		// if (selectedBoardObjectSO is BuildingSO)
		// {
		// 	newBoardObject.GetComponent<BuildingBase>().Init(selectedBoardObjectSO as BuildingSO);
		// }
		// else if (selectedBoardObjectSO is SoldierSO)
		// {
		// 	newBoardObject.GetComponent<SoldierBase>().Init(selectedBoardObjectSO as SoldierSO);
		// }
		// else
		// {
		// 	throw new Exception("Unknown BoardObjectSO type");
		// }
		newBoardObject.GetComponent<BuildingBase>().Init(selectedBoardObjectSO);
		placedGameObjects.Add(newBoardObject);
		return placedGameObjects.Count - 1;
	}

	public void RemoveObjectAt(int gameObjectIndex)
	{
		if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
		{
			return;
		}
		Destroy(placedGameObjects[gameObjectIndex]);
		placedGameObjects[gameObjectIndex] = null;
	}

	public GameObject GetObjectAt(int gameObjectIndex)
	{
		if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
		{
			return null;
		}
		return placedGameObjects[gameObjectIndex];
	}
}
