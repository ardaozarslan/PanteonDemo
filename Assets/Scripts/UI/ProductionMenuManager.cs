using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuManager : Singleton<ProductionMenuManager>
{
	[SerializeField] private GameObject productionMenu;
	[SerializeField] private RectTransform productionMenuContent;
	[SerializeField] private RectTransform productionMenuViewPort;
	[SerializeField] private RectTransform productionMenuItemPrefab;

	[SerializeField] private BetterAxisAlignedLayoutGroup layoutGroup;
	[SerializeField] private ScrollRect scrollRect;


	private List<BuildingSO> buildings => GameManager.Instance.Buildings;

	private List<RectTransform> items = new List<RectTransform>();
	private Vector2 itemSize;

	private Vector2 oldVelocity;
	private bool isUpdated;


	private void Start()
	{
		isUpdated = false;
		oldVelocity = Vector2.zero;

		// Create initial items.
		for (int i = 0; i < buildings.Count; i++)
		{
			var building = buildings[i];
			var productionMenuItem = Instantiate(productionMenuItemPrefab, productionMenuContent);
			productionMenuItem.GetComponent<ProductionMenuItem>().Init(building);
			productionMenuItem.name = building.name;
			items.Add(productionMenuItem);
		}

	}

	public void Init()
	{
		// Calculate the number of items needed to fill the viewport
		int itemsToAdd = Mathf.CeilToInt(productionMenuViewPort.rect.height / (items[0].rect.height + layoutGroup.spacing));

		// Add items to fill the viewport to the bottom
		for (int i = 0; i < itemsToAdd; i++)
		{
			RectTransform newItem = Instantiate(items[i % items.Count], productionMenuContent);
			newItem.SetAsLastSibling();
		}

		// Add items to fill the viewport to the top
		for (int i = 0; i < itemsToAdd; i++)
		{
			int num = items.Count - 1 - i;
			while (num < 0)
			{
				num += items.Count;
			}
			RectTransform newItem = Instantiate(items[num], productionMenuContent);
			newItem.SetAsFirstSibling();
		}

		// Set the content position to the middle of the list
		productionMenuContent.localPosition = new Vector3(productionMenuContent.localPosition.x, (items[0].rect.height + layoutGroup.spacing) * itemsToAdd, productionMenuContent.localPosition.z);
	}

	private void Update()
	{
		// This is a hack to fix the scroll rect bug
		if (isUpdated) {
			scrollRect.velocity = oldVelocity;
			isUpdated = false;
		}
		// Check if the content position is out of bounds
		if (productionMenuContent.localPosition.y > (items[0].rect.height + layoutGroup.spacing) * items.Count)
		{
			Canvas.ForceUpdateCanvases();
			oldVelocity = scrollRect.velocity;
			// Move the content position to the top
			productionMenuContent.localPosition -= new Vector3(0, (items[0].rect.height + layoutGroup.spacing) * items.Count, 0);
			isUpdated = true;
		}
		else if (productionMenuContent.localPosition.y < 0)
		{
			Canvas.ForceUpdateCanvases();
			oldVelocity = scrollRect.velocity;
			// Move the content position to the bottom
			productionMenuContent.localPosition += new Vector3(0, (items[0].rect.height + layoutGroup.spacing) * items.Count, 0);
			isUpdated = true;
		}
	}
}
