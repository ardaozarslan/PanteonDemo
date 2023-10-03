using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class responsible for placing and removing objects on the game board.
/// </summary>
public class ObjectPlacer : Singleton<ObjectPlacer>
{
	private List<GameObject> placedGameObjects = new();
	[SerializeField] private GameObject boardObjectsParent;

	/// <summary>
	/// Instantiates a new game object from the given prefab, places it at the specified position, and initializes it with the selected board object scriptable object.
	/// </summary>
	/// <param name="prefab">The prefab to instantiate.</param>
	/// <param name="selectedBoardObjectSO">The selected board object scriptable object.</param>
	/// <param name="objectPosition">The position to place the new game object.</param>
	/// <returns>The index of the newly placed game object in the list of placed game objects.</returns>
	public int PlaceObject(GameObject prefab, BoardObjectSO selectedBoardObjectSO, Vector3 objectPosition)
	{
		GameObject newBoardObject = Instantiate(prefab, Vector3Int.zero, Quaternion.identity, boardObjectsParent.transform);
		newBoardObject.transform.position = objectPosition;
		placedGameObjects.Add(newBoardObject);
		newBoardObject.GetComponent<BoardObjectBase>().Init(selectedBoardObjectSO, placedGameObjects.Count - 1);
		return placedGameObjects.Count - 1;
	}

	/// <summary>
	/// Removes the game object at the specified index from the list of placed game objects.
	/// </summary>
	/// <param name="gameObjectIndex">The index of the game object to remove.</param>
	public void RemoveObjectAt(int gameObjectIndex)
	{
		if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
		{
			return;
		}
		StartCoroutine(WaitUntilEndOfFrame(gameObjectIndex));
	}

	/// <summary>
	/// Waits until the end of the frame and then destroys the game object at the specified index in the placedGameObjects array.
	/// </summary>
	/// <param name="gameObjectIndex">The index of the game object to destroy.</param>
	IEnumerator WaitUntilEndOfFrame(int gameObjectIndex)
	{
		yield return new WaitForEndOfFrame();
		Destroy(placedGameObjects[gameObjectIndex]);
		placedGameObjects[gameObjectIndex] = null;
	}

	/// <summary>
	/// Returns the game object at the specified index in the list of placed game objects.
	/// </summary>
	/// <param name="gameObjectIndex">The index of the game object to retrieve.</param>
	/// <returns>The game object at the specified index, or null if the index is out of range or the game object has been destroyed.</returns>
	public GameObject GetObjectAt(int gameObjectIndex)
	{
		if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
		{
			return null;
		}
		return placedGameObjects[gameObjectIndex];
	}
}
