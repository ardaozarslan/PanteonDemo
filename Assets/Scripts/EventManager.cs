using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void OnShowInformation(GameObject shownGameObject);
	public event OnShowInformation OnShowInformationEvent;

	public void ShowInformation(GameObject shownGameObject)
	{
		OnShowInformationEvent?.Invoke(shownGameObject);
	}
}