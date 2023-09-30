using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Represents a production menu item to display the buildings in the UI.
/// </summary>
public class ProductionMenuItem : MonoBehaviour
{
	[SerializeField] private Image buildingImage;
	private BuildingSO buildingSO;
	public BuildingSO BuildingSO { get { return buildingSO; } }

	public ProductionMenuItem cloneOf;

	[SerializeField] private TextMeshProUGUI buildingNameText;
	[SerializeField] private TextMeshProUGUI buildingSizeText;

	/// <summary>
	/// Initializes the ProductionMenuItem with the given BuildingSO and optional clone.
	/// </summary>
	/// <param name="_buildingSO">The BuildingSO to initialize with.</param>
	/// <param name="_cloneOf">An optional ProductionMenuItem to clone from.</param>
	public void Init(BuildingSO _buildingSO, ProductionMenuItem _cloneOf = null)
	{
		if (buildingSO == null) {
			if (_cloneOf != null) {
				buildingSO = _cloneOf.buildingSO;
			}
			else {
				buildingSO = _buildingSO;
			}
		}
		buildingImage.sprite = _buildingSO.Sprite;
		buildingImage.preserveAspect = true;
		buildingNameText.text = _buildingSO.Name;
		buildingSizeText.text = _buildingSO.Size.x + "x" + _buildingSO.Size.y;
	}

	/// <summary>
	/// Handles the click event for the production menu item. If this item is a clone, it will call the OnClick method of its original item.
	/// Otherwise, it will start the placement of the building associated with this item.
	/// </summary>
	public void OnClick()
	{
		if (cloneOf != null)
		{
			cloneOf.OnClick();
			return;
		}
		PlacementSystem.Instance.StartPlacement(buildingSO);
	}
}
