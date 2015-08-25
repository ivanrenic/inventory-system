using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;

	public EventSystem eventSystem;

	private List<GameObject> allSlots;
	private static int emptySlots;

	public Canvas canvas;

	public static int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	private static Slot sourceSlot, destinationSlot;
	public GameObject iconPrefab;
	private static GameObject hoverObject;

	void Start () {
		CreateLayout();
	}

	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			if (!eventSystem.IsPointerOverGameObject(-1) && sourceSlot != null) {
				sourceSlot.GetComponent<Image>().color = Color.white;
				sourceSlot.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				destinationSlot = null;
				sourceSlot = null;
				hoverObject = null;
			}
		}

		if (hoverObject != null) {
			Vector2 position = Vector2.zero;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, new Vector3(Input.mousePosition.x, Input.mousePosition.y - 1, Input.mousePosition.z), canvas.worldCamera, out position);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}
	}

	private void CreateLayout() {
		allSlots = new List<GameObject>();
		emptySlots = slots;

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				GameObject newSlot = Instantiate(slotPrefab) as GameObject;
				newSlot.transform.SetParent(this.transform, false);
				newSlot.name = "Slot[" + i + "][" + j + "]";

				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				slotRect.localPosition = new Vector3(-inventoryWidth + slotPaddingLeft * (j + 1) + (slotSize * j),inventoryHeight -slotPaddingTop * (i + 1) - (slotSize * i));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

				allSlots.Add(newSlot);
			}
		}
	}

	public bool AddItem(Item item)
	{
		if (item.maxStack == 1)
		{
			PlaceEmpty(item);
			return true;
		} else {
			foreach (GameObject slot in allSlots) {
				Slot slotScript = slot.GetComponent<Slot>();

				if (!slotScript.IsEmpty) {
					if (slotScript.CurrentItem.type == item.type && slotScript.IsAvailableForStacking) {
						slotScript.AddItem(item);
						return true;
					}
				}
			}

			if (emptySlots > 0) {
				PlaceEmpty(item);
			}
		}

		return false;
	}

	private bool PlaceEmpty(Item item) {
		if (emptySlots > 0) {
			foreach (GameObject slot in allSlots) {
				Slot slotScript = slot.GetComponent<Slot>();

				if (slotScript.IsEmpty) {
					slotScript.AddItem(item);
					emptySlots -= 1;
					return true;
				}
			}
		}

		return false;
	}

	public void MoveItem(GameObject clicked) {
		if (sourceSlot == null) {
			if (!clicked.GetComponent<Slot>().IsEmpty) {
				sourceSlot = clicked.GetComponent<Slot>();
				sourceSlot.GetComponent<Image>().color = Color.gray;

				hoverObject = Instantiate(iconPrefab) as GameObject;
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";

				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

				//hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
				hoverObject.transform.SetParent(GameObject.Find("Inventory").transform, false);
				hoverObject.transform.localScale = sourceSlot.gameObject.transform.localScale;
			}
		} else if (destinationSlot == null) {
			destinationSlot = clicked.GetComponent<Slot>();
			Destroy (GameObject.Find ("Hover"));
		}

		if (destinationSlot != null && sourceSlot != null) {
			Stack<Item> temp = new Stack<Item>(destinationSlot.Items);
			destinationSlot.AddItems(sourceSlot.Items);
			destinationSlot.UpdateStackText();

			if (temp.Count == 0) {
				sourceSlot.ClearSlot();
			} else {
				sourceSlot.AddItems(temp);
				sourceSlot.UpdateStackText();
			}

			sourceSlot.GetComponent<Image>().color = Color.white;
			sourceSlot = null;
			destinationSlot = null;
			hoverObject = null;
		}
	}
}
