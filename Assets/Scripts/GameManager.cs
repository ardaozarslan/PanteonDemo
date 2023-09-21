using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private List<BuildingSO> buildings;
	public List<BuildingSO> Buildings { get { return buildings ??= new(Resources.LoadAll<BuildingSO>("Buildings")); } }

	private void Start()
	{
		StartCoroutine(WaitUntilEndOfFrame());
	}

	// This is a workaround to make sure that the production menu is initialized after the UI is scaled
	IEnumerator WaitUntilEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		ProductionMenuManager.Instance.Init();
	}
}
