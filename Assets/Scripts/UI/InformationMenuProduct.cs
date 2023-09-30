using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: clean up comments

/// <summary>
/// Represents a production menu item to display the buildings in the UI.
/// </summary>
public class InformationMenuProduct : MonoBehaviour
{
	[SerializeField] private Image boardObjectImage;
	private BoardObjectSO boardObjectSO;
	public BoardObjectSO BoardObjectSO { get { return boardObjectSO; } }

	[SerializeField] private TextMeshProUGUI boardObjectNameText;
	// [SerializeField] private TextMeshProUGUI boardObjectSizeText;

	/// <summary>
	/// Initializes the InformationMenuProduct with the given BoardObjectSO.
	/// </summary>
	/// <param name="_boardObjectSO">The BoardObjectSO to initialize with.</param>
	public void Init(BoardObjectSO _boardObjectSO)
	{
		boardObjectSO = _boardObjectSO;
		boardObjectImage.sprite = _boardObjectSO.Sprite;
		boardObjectImage.preserveAspect = true;
		boardObjectNameText.text = _boardObjectSO.Name;
		// boardObjectSizeText.text = _boardObjectSO.Size.x + "x" + _boardObjectSO.Size.y;
	}

	/// <summary>
	/// Handles the click event for the information menu product. It spawns the product associated with this item.
	/// </summary>
	public void OnClick()
	{
		// TODO: Spawn the product associated with this item.
	}
}
