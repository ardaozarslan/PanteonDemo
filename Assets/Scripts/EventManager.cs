using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The EventManager class is responsible for managing events related between UI and game objects.
/// </summary>
public class EventManager : Singleton<EventManager>
{
	public delegate void OnShowInformation(GameObject shownGameObject);
	public event OnShowInformation OnShowInformationEvent;

	public delegate void OnSpawnProduct(BoardObjectSO boardObjectSO, GameObject spawnerGameObject);
	public event OnSpawnProduct OnSpawnProductEvent;

	/// <summary>
	/// Invokes the OnShowInformationEvent with the given GameObject as a parameter.
	/// </summary>
	/// <param name="shownGameObject">The GameObject to be shown.</param>
	public void ShowInformation(GameObject shownGameObject)
	{
		OnShowInformationEvent?.Invoke(shownGameObject);
	}

	/// <summary>
	/// Spawns a product using the provided BoardObjectSO and spawner GameObject.
	/// </summary>
	/// <param name="boardObjectSO">The BoardObjectSO to use for spawning the product.</param>
	/// <param name="spawnerGameObject">The GameObject to use as the spawner for the product.</param>
	public void SpawnProduct(BoardObjectSO boardObjectSO, GameObject spawnerGameObject)
	{
		OnSpawnProductEvent?.Invoke(boardObjectSO, spawnerGameObject);
	}
}