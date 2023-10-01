using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
	public delegate void OnShowInformation(GameObject shownGameObject);
	public event OnShowInformation OnShowInformationEvent;

	public delegate void OnSpawnProduct(BoardObjectSO boardObjectSO, GameObject spawnerGameObject);
	public event OnSpawnProduct OnSpawnProductEvent;

	public void ShowInformation(GameObject shownGameObject)
	{
		OnShowInformationEvent?.Invoke(shownGameObject);
	}

	public void SpawnProduct(BoardObjectSO boardObjectSO, GameObject spawnerGameObject)
	{
		OnSpawnProductEvent?.Invoke(boardObjectSO, spawnerGameObject);
	}
}