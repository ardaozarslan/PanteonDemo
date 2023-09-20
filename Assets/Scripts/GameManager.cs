using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private List<BuildingSO> buildings;
	public List<BuildingSO> Buildings { get { return buildings; } }

    private void Awake() {
		buildings = new(Resources.LoadAll<BuildingSO>("Buildings"));
	}

	private void Start() {
		ProductionMenuManager.Instance.Init();
	}
}
