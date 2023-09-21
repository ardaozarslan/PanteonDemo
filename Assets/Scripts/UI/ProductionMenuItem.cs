using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will be used to display the building in the production menu
public class ProductionMenuItem : MonoBehaviour
{
	[SerializeField] Image _buildingImage;

    public void Init(BuildingSO building) {
		_buildingImage.sprite = building.Sprite;
		_buildingImage.preserveAspect = true;
	}
}
