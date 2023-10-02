using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InformationMenuManager : Singleton<InformationMenuManager>
{
	private GameObject selectedGameObject;
	private BoardObjectSO selectedObjectSO;
	private BoardObjectSO lastSelectedObjectSO;
	private CanvasGroup canvasGroup;

	[SerializeField] private RectTransform informationMenuContent;
	[SerializeField] private RectTransform informationMenuProductPrefab;
	[SerializeField] private RectTransform productsHeader;
	[SerializeField] private RectTransform productsScroll;
	[SerializeField] private TextMeshProUGUI selectedObjectNameText;
	[SerializeField] private Image selectedObjectImage;

	private int lastWindowHeight;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		ShowPanel(false);
	}

	private void Start()
	{
		EventManager.Instance.OnShowInformationEvent += ShowInformation;
	}

	private void ShowPanel(bool show)
	{
		canvasGroup.alpha = show ? 1 : 0;
		canvasGroup.blocksRaycasts = show;
		canvasGroup.interactable = show;
	}

	private void ShowInformation(GameObject shownGameObject)
	{
		if (shownGameObject == null)
		{
			ShowPanel(false);
			return;
		}
		ShowPanel(true);
		selectedGameObject = shownGameObject;
		selectedObjectSO = shownGameObject.GetComponent<BoardObjectBase>().BoardObjectSO;
		informationMenuContent.position = new Vector3(informationMenuContent.position.x, productsScroll.position.y, informationMenuContent.position.z);
		if (selectedObjectSO == lastSelectedObjectSO)
		{
			return;
		}

		selectedObjectNameText.text = selectedObjectSO.Name;
		selectedObjectImage.sprite = selectedObjectSO.Sprite;

		foreach (Transform child in informationMenuContent.transform)
		{
			Destroy(child.gameObject);
		}

		if (selectedObjectSO.Products.Count == 0)
		{
			productsHeader.gameObject.SetActive(false);
		}
		else
		{
			productsHeader.gameObject.SetActive(true);
		}
		foreach (BoardObjectSO productSO in selectedObjectSO.Products)
		{
			var newInformationMenuProduct = Instantiate(informationMenuProductPrefab, informationMenuContent);
			newInformationMenuProduct.GetComponent<InformationMenuProduct>().Init(productSO);
			newInformationMenuProduct.name = productSO.name;
		}
		informationMenuContent.position = new Vector3(informationMenuContent.position.x, productsScroll.position.y, informationMenuContent.position.z);

		lastSelectedObjectSO = selectedObjectSO;
	}

	public void SpawnProduct(BoardObjectSO productSO)
	{
		if (selectedGameObject == null)
		{
			return;
		}
		EventManager.Instance.SpawnProduct(productSO, selectedGameObject);
	}

	private void Update()
	{
		if (Screen.height != lastWindowHeight)
		{
			Canvas.ForceUpdateCanvases();
			lastWindowHeight = Screen.height;
			productsScroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, productsScroll.position.y - 25);
		}
	}
}
