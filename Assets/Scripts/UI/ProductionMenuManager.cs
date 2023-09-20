using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenuManager : Singleton<ProductionMenuManager>
{
	[SerializeField] private GameObject _productionMenu;
	[SerializeField] private RectTransform _productionMenuContent;
	[SerializeField] private RectTransform _productionMenuItemPrefab;

	private List<BuildingSO> buildings => GameManager.Instance.Buildings;

	public float spacing = 10f;

	private List<RectTransform> items = new List<RectTransform>();
	private Vector2 itemSize;

	public void Init()
	{
		// Create initial items.
		for (int i = 0; i < buildings.Count; i++)
		{
			var building = buildings[i];
			var productionMenuItem = Instantiate(_productionMenuItemPrefab, _productionMenuContent);
			productionMenuItem.GetComponent<ProductionMenuItem>().Init(building);
			productionMenuItem.name = building.name;
			items.Add(productionMenuItem);
		}
	}

	// private void Update()
	// {
	// 	// Check for loop condition on each frame.
	// 	for (int i = 0; i < items.Count; i++)
	// 	{
	// 		var item = items[i];
	// 		var itemPosition = item.anchoredPosition;
	// 		var contentPosition = _productionMenuContent.anchoredPosition;

	// 		if (itemPosition.y + contentPosition.y < -itemSize.y)
	// 		{
	// 			// Item is off the bottom edge, move it to the top.
	// 			item.anchoredPosition += new Vector2(0f, items.Count * itemSize.y);
	// 		}
	// 		else if (itemPosition.y + contentPosition.y > items.Count * itemSize.y)
	// 		{
	// 			// Item is off the top edge, move it to the bottom.
	// 			item.anchoredPosition -= new Vector2(0f, items.Count * itemSize.y);
	// 		}
	// 	}
	// }
}
