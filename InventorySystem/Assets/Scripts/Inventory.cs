using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : Panel {

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;
	public int slots;
	public int columns;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;

	public GameObject background;

	private List<GameObject> allSlots;
	public int emptySlots;

	public int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	private GameObject m_slotContainer;

	void Start () {
		CreateLayout();

		PanelManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
	}

	void Awake() {
		m_slotContainer = transform.GetChild(0).gameObject;
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Return)) {
			SetSlots(slots + 5);
		}

		if (Input.GetKeyUp (KeyCode.Backspace)) {
			SetSlots(slots - 5);
		}

		if (Input.GetMouseButtonUp(0)) {
			if (!PanelManager.Instance.eventSystem.IsPointerOverGameObject(-1) && PanelManager.Instance.Source != null) {
				PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;

				PanelManager.Instance.Source.DropItems();

				PanelManager.Instance.Source.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				//PutItemBack();
				PanelManager.Instance.Destination = null;
				PanelManager.Instance.Source = null;
				emptySlots += 1;
			} else if (!PanelManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !PanelManager.Instance.MovingSlot.IsEmpty) {
				PanelManager.Instance.MovingSlot.DropItems();

				PanelManager.Instance.MovingSlot.ClearSlot();
				Destroy (GameObject.Find("Hover"));
			}
		}

		if (PanelManager.Instance.tooltipObject.activeSelf) {
			Vector2 position = Input.mousePosition;
			Rect rect = PanelManager.Instance.tooltipObject.GetComponent<RectTransform>().rect;
			position.Set(position.x - rect.width * 2f, position.y + rect.height * 4f);
			PanelManager.Instance.tooltipObject.transform.position = position;
		}

		if (PanelManager.Instance.HoverObject != null) {
			Vector2 position = Vector2.zero;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(PanelManager.Instance.canvas.transform as RectTransform, new Vector3(Input.mousePosition.x, Input.mousePosition.y - 1, Input.mousePosition.z), PanelManager.Instance.canvas.worldCamera, out position);
			PanelManager.Instance.HoverObject.transform.position = PanelManager.Instance.canvas.transform.TransformPoint(position);
		}
	}

	public float GetSlotSize() {
		return allSlots[0].gameObject.GetComponent<RectTransform>().sizeDelta.x;
	}

	public virtual void CreateLayout() {
		allSlots = new List<GameObject>();
		emptySlots = slots;

		int rows = (int)Mathf.Ceil((float)slots / columns);

		inventoryWidth = columns * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = m_slotContainer.GetComponent<RectTransform>();

		for (int i = 0; i < slots; i++) {
			GameObject newSlot = Instantiate(PanelManager.Instance.slotPrefab) as GameObject;
			newSlot.transform.SetParent(m_slotContainer.transform, false);
			newSlot.name = "Slot[" + (i / 4) + "][" + (i % 4) + "]";

			RectTransform slotRect = newSlot.GetComponent<RectTransform>();
			slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), - slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
			slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
			slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
			
			allSlots.Add(newSlot);
		}
	}

	public void SetSlots(int numberOfSlots) {
		if (numberOfSlots < 0)
			numberOfSlots = 0;

		int newSlots = numberOfSlots - slots;
		int oldSlots = slots;


		int rows = (int)Mathf.Ceil((float)numberOfSlots / columns);

		inventoryWidth = columns * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
		


		if (newSlots > 0) {
			for (int i = oldSlots; i < numberOfSlots; i++) {
				GameObject newSlot = Instantiate(PanelManager.Instance.slotPrefab) as GameObject;
				newSlot.transform.SetParent(m_slotContainer.transform, false);
				newSlot.name = "Slot[" + (i / 4) + "][" + (i % 4) + "]";
				
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), - slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
				
				allSlots.Add(newSlot);
				emptySlots++;
			}
		} else if (newSlots < 0) {
			RemoveSlots(-newSlots);
		}

		slots = numberOfSlots;
		inventoryRect = m_slotContainer.GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
		RepositionSlots();

	}

	private void RemoveSlots(int slotsToRemove) {
		for (int i = slots-1; i > slots - slotsToRemove - 1; i--) {
			GameObject tempSlotObject = allSlots[i];
			Slot tempSlot = allSlots[i].GetComponent<Slot>();
			tempSlot.DropItems();

			allSlots.RemoveAt(i);
			DestroyObject(tempSlotObject);
			if (emptySlots > 0)
				emptySlots--;
		}
	}

	private void RepositionSlots() {
		for (int i = 0; i < slots; i++) {
			RectTransform slotRect = allSlots[i].GetComponent<RectTransform>();
			slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), - slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
		}
	}

	public bool AddItem(ItemHolder item)
	{
		if (item.Item.Type == ItemType.BRIEFCASE) {
			SetSlots(slots + 4);
			return true;
		}

		if (item.Item.MaxStack == 1)
		{
			return PlaceEmpty(item);
		} else {
			foreach (GameObject slot in allSlots) {
				Slot slotScript = slot.GetComponent<Slot>();

				if (!slotScript.IsEmpty) {
					if (slotScript.CurrentItem.Item.Type == item.Item.Type && slotScript.IsAvailableForStacking) {
						if (!PanelManager.Instance.MovingSlot.IsEmpty && PanelManager.Instance.Clicked.GetComponent<Slot>() == slotScript.GetComponent<Slot>()) {
							continue;
						} else {
							slotScript.AddItem(item);
							return true;
						}
					}
				}
			}

			if (emptySlots > 0) {
				return PlaceEmpty(item);
			}
		}

		return false;
	}

	private bool PlaceEmpty(ItemHolder item) {
		if (emptySlots > 0) {
			foreach (GameObject slot in allSlots) {
				Slot slotScript = slot.GetComponent<Slot>();

				if (slotScript.IsEmpty) {
					slotScript.AddItem(item);
					if (item.Item.Type != ItemType.ALUMINIJ && item.Item.Type != ItemType.BATERIJA && item.Item.Type != ItemType.PLASTIKA) {
						if (CharacterPanel.Instance.CanEquip(item)) {
							item.Use(slotScript);
						}
					}
					emptySlots -= 1;
					return true;
				}
			}
		}

		return false;
	}

	public void MoveItem(GameObject clicked) {
		PanelManager.Instance.Clicked = clicked;
		Slot tempSlot = clicked.GetComponent<Slot>();

		if (!PanelManager.Instance.MovingSlot.IsEmpty) {
			if (tempSlot.IsEmpty) {
				tempSlot.AddItems(PanelManager.Instance.MovingSlot.Items);
				PanelManager.Instance.MovingSlot.Items.Clear();
				Destroy (GameObject.Find("Hover"));
			} else if (!tempSlot.IsEmpty && PanelManager.Instance.MovingSlot.CurrentItem.Item.Type == tempSlot.CurrentItem.Item.Type && tempSlot.IsAvailableForStacking) {
				MergeStacks(PanelManager.Instance.MovingSlot, tempSlot);
			}
		} else if (PanelManager.Instance.Source == null && Input.GetKey(KeyCode.LeftAlt)) {
			if (!tempSlot.IsEmpty && !GameObject.Find("Hover")) {
				tempSlot.DropItems();
				tempSlot.ClearSlot();
				if (tempSlot.type == SlotType.INVENTORY)
					GameObject.Find("Inventory").GetComponent<Inventory>().emptySlots += 1;
			}
		} else if (PanelManager.Instance.Source == null && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)) {
			if (!tempSlot.IsEmpty && !GameObject.Find("Hover")) {
				PanelManager.Instance.Source = clicked.GetComponent<Slot>();
				PanelManager.Instance.Source.GetComponent<Image>().color = Color.gray;

				CreateHoverIcon();
			}
		} else if (PanelManager.Instance.Destination == null && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)) {
			PanelManager.Instance.Destination = tempSlot;
			Destroy (GameObject.Find ("Hover"));
		}

		if (PanelManager.Instance.Destination != null && PanelManager.Instance.Source != null) {
			if (!PanelManager.Instance.Destination.IsEmpty && PanelManager.Instance.Source.CurrentItem.Item.Type == PanelManager.Instance.Destination.CurrentItem.Item.Type && PanelManager.Instance.Destination.IsAvailableForStacking) {
				MergeStacks(PanelManager.Instance.Source, PanelManager.Instance.Destination);
			} else {
				Slot.SwapItems(PanelManager.Instance.Source, PanelManager.Instance.Destination);
			}

			PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;
			PanelManager.Instance.Source = null;
			PanelManager.Instance.Destination = null;
			Destroy (GameObject.Find ("Hover"));
			
		}
	}

	public void SplitStack() {
		PanelManager.Instance.selectStackSize.SetActive(false);

		if (PanelManager.Instance.SplitAmount == PanelManager.Instance.MaxStackCount) {
			MoveItem(PanelManager.Instance.Clicked);
		} else if (PanelManager.Instance.SplitAmount > 0) {
			PanelManager.Instance.MovingSlot.Items = PanelManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(PanelManager.Instance.SplitAmount);
			CreateHoverIcon();
		}
	}

	public void HideSplitScreen() {
		PanelManager.Instance.selectStackSize.SetActive(false);
	}

	public void MergeStacks(Slot sourceSlot, Slot destinationSlot) {
		int max = destinationSlot.CurrentItem.Item.MaxStack - destinationSlot.Items.Count;
		int count = sourceSlot.Items.Count < max ? sourceSlot.Items.Count : max;

		for (int i = 0; i < count; i++) {
			destinationSlot.AddItem(sourceSlot.RemoveItem());
			PanelManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = PanelManager.Instance.MovingSlot.Items.Count.ToString();
		}

		if (sourceSlot.Items.Count == 0) {
			sourceSlot.ClearSlot();
			Destroy(GameObject.Find("Hover"));
		}
	}

	public override void Toggle() {
		if (shown) {
			shown = false;
			background.SetActive(false);
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x + 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);

			if (GameObject.Find("Hover") && !GameObject.Find("Inventory").GetComponent<Inventory>().Shown && PanelManager.Instance.Source != null && PanelManager.Instance.Source.type == SlotType.INVENTORY) {
				PutItemBack();
			}
		} else {
			shown = true;
			background.SetActive(true);
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x - 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);
		}
	}
}
