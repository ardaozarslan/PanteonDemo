using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the production menu UI, including scrolling and item initialization.
/// </summary>
public class ProductionMenuManager : Singleton<ProductionMenuManager>
{
	[SerializeField] private GameObject productionMenu;
	[SerializeField] private RectTransform productionMenuContent;
	[SerializeField] private RectTransform productionMenuViewPort;
	[SerializeField] private RectTransform productionMenuItemPrefab;

	[SerializeField] private BetterAxisAlignedLayoutGroup layoutGroup;
	[SerializeField] private ScrollRect scrollRect;


	private List<BuildingSO> Buildings => GameManager.Instance.Buildings;

	private readonly List<RectTransform> items = new();

	private Vector2 oldVelocity;
	private bool isInitialized = false;
	private bool isUpdated;


	/// <summary>
	/// Called before the first frame update. Initializes the production menu items with the buildings defined in the Buildings list.
	/// </summary>
	private void Start()
	{
		isUpdated = false;
		oldVelocity = Vector2.zero;

		// Create initial items.
		for (int i = 0; i < Buildings.Count; i++)
		{
			var building = Buildings[i];
			var productionMenuItem = Instantiate(productionMenuItemPrefab, productionMenuContent);
			productionMenuItem.GetComponent<ProductionMenuItem>().Init(building);
			productionMenuItem.name = building.name;
			items.Add(productionMenuItem);
		}
	}

	/// <summary>
	/// Initializes the production menu by adding items to fill the viewport and setting the content position to the middle of the list.
	/// </summary>
	public void Init()
	{
		if (isInitialized)
		{
			return;
		}
		isInitialized = true;
		// Calculate the number of items needed to fill the viewport
		int itemsToAdd = Mathf.CeilToInt(productionMenuViewPort.rect.height / (items[0].rect.height + layoutGroup.spacing));

		// Add items to fill the viewport to the bottom
		for (int i = 0; i < itemsToAdd; i++)
		{
			RectTransform newItem = Instantiate(items[i % items.Count], productionMenuContent);
			newItem.GetComponent<ProductionMenuItem>().Init(items[i % items.Count].GetComponent<ProductionMenuItem>().BuildingSO, items[i % items.Count].GetComponent<ProductionMenuItem>());
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
			newItem.GetComponent<ProductionMenuItem>().Init(items[num].GetComponent<ProductionMenuItem>().BuildingSO, items[num].GetComponent<ProductionMenuItem>());
			newItem.SetAsFirstSibling();
		}

		// Set the content position to the middle of the list
		productionMenuContent.localPosition = new Vector3(productionMenuContent.localPosition.x, (items[0].rect.height + layoutGroup.spacing) * itemsToAdd, productionMenuContent.localPosition.z);
	}

	/// <summary>
	/// This method is called every frame. It checks if the content position is out of bounds and moves it accordingly.
	/// </summary>
	private void Update()
	{
		if (!isInitialized)
		{
			return;
		}
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
