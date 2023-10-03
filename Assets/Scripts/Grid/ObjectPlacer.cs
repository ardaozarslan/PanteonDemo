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
		placedGameObjects.Add(newBoardObject);
		newBoardObject.GetComponent<BoardObjectBase>().Init(selectedBoardObjectSO, placedGameObjects.Count - 1);
		return placedGameObjects.Count - 1;
	}

	public void RemoveObjectAt(int gameObjectIndex)
	{
		if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
		{
			return;
		}
		StartCoroutine(WaitUntilEndOfFrame(gameObjectIndex));
	}

	IEnumerator WaitUntilEndOfFrame(int gameObjectIndex)
	{
		yield return new WaitForEndOfFrame();
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
