using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuItem : MonoBehaviour
{
	[SerializeField] Image _buildingImage;

    public void Init(BuildingSO building) {
		_buildingImage.sprite = building.Sprite;
		_buildingImage.preserveAspect = true;
	}
}
