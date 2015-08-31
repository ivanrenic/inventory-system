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
	//public GameObject slotPrefab;
	private int hoverOffset = 20;

	public GameObject background;

	private EventSystem m_eventSystem;
	//private PanelManager PanelManager.Instance;

	private List<GameObject> allSlots;
	public int emptySlots;

	/*private Canvas canvas;

	public Canvas Canvas {
		get { return canvas; }
	}*/

	public int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	//private static Slot sourceSlot, destinationSlot;
	//public GameObject iconPrefab;
	//private static GameObject hoverObject;
	//private static GameObject clicked;

	//private static GameObject selectStackSizeObject;
	//public GameObject selectStackSize;
	//public Text stackSplitText;

	//private int splitAmount;
	//private int maxStackCount;
	//private static Slot movingSlot;

	private GameObject m_slotContainer;

	//public GameObject TooltipObject;
	//private static GameObject tooltip;
	//public Text sizeTextObject;
	//private static Text sizeText;
	//public Text visualTextObject;
	//private static Text visualText;

	//public GameObject dropItemPrefab;
	private static GameObject playerRef;

	/*private static Inventory instance;

	public static Inventory Instance {
		get { 
			if (instance == null) {
				instance = GameObject.FindObjectOfType<Inventory>();
			}
			return Inventory.instance; 
		}
	}*/

	/*private bool shown = true;

	public bool Shown {
		get { return shown; }
	}*/

	void Start () {
		//tooltip = TooltipObject;
		//sizeText = sizeTextObject;
		//visualText = visualTextObject;
		//selectStackSizeObject = selectStackSize;

		CreateLayout();

		playerRef = GameObject.Find("Player");

		PanelManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
	}

	void Awake() {
		m_slotContainer = transform.GetChild(0).gameObject;

		GameObject eventSystemObject = GameObject.Find("EventSystem");
		m_eventSystem = eventSystemObject.GetComponent<EventSystem>();
		//GameObject canvasObject = GameObject.Find("Canvas");
		//canvas = canvasObject.GetComponent<Canvas>();

		//m_panel = PanelManager.Instance;
	}

	void Update () {
		Debug.Log("empty slots: " + emptySlots.ToString());
		if (Input.GetKeyUp(KeyCode.Return)) {
			SetSlots(slots + 5);
		}

		if (Input.GetKeyUp (KeyCode.Backspace)) {
			SetSlots(slots - 5);
		}

		if (Input.GetMouseButtonUp(0)) {
			if (!PanelManager.Instance.eventSystem.IsPointerOverGameObject(-1) && PanelManager.Instance.Source != null) {
				PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;

				// Dropping the items
				/*foreach (var item in sourceSlot.Items) {
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

					Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

					v *= 3;

					GameObject droppedItem = Instantiate(dropItemPrefab, playerRef.transform.position - v, Quaternion.identity) as GameObject;
					droppedItem.GetComponent<SpriteRenderer>().sprite = item.spriteNeutral;
				}*/
				//DropItems(PanelManager.Instance.Source.Items);
				PanelManager.Instance.Source.DropItems();

				PanelManager.Instance.Source.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				//PutItemBack();
				PanelManager.Instance.Destination = null;
				PanelManager.Instance.Source = null;
				emptySlots += 1;
			} else if (!PanelManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !PanelManager.Instance.MovingSlot.IsEmpty) {
				/*foreach (var item in movingSlot.Items) {
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
					
					Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
					
					v *= 3;
					
					GameObject droppedItem = Instantiate(dropItemPrefab, playerRef.transform.position - v, Quaternion.identity) as GameObject;
					droppedItem.GetComponent<SpriteRenderer>().sprite = item.spriteNeutral;
				}*/
				//DropItems(PanelManager.Instance.MovingSlot.Items);
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

	/*private void DropItems(Stack<ItemHolder> items) {
		foreach (var item in items) {
			float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
			
			Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
			
			v *= 3;
			
			GameObject droppedItem = Instantiate(PanelManager.Instance.dropItem, playerRef.transform.position - v, Quaternion.identity) as GameObject;
			//droppedItem.GetComponent<Item>().SetStats(item);
			droppedItem.AddComponent<ItemHolder>();
			droppedItem.GetComponent<ItemHolder>().Item = item.Item;
			droppedItem.GetComponent<SpriteRenderer>().sprite = item.Item.SpriteNeutral;
		}
	}*/

	/*public void ShowTooltip(GameObject slot) {
		Slot tempSlot = slot.GetComponent<Slot>();

		if (!tempSlot.IsEmpty && PanelManager.Instance.HoverObject == null && !PanelManager.Instance.selectStackSize.transform.gameObject.activeSelf) {
			PanelManager.Instance.visualTextObject.text = tempSlot.CurrentItem.GetTooltip();
			PanelManager.Instance.sizeTextObject.text = PanelManager.Instance.visualTextObject.text;

			PanelManager.Instance.tooltipObject.SetActive(true);

			float xPos = slot.transform.position.x + slotPaddingLeft;
			float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

			PanelManager.Instance.tooltipObject.transform.position = new Vector2(xPos, yPos);
		}
	}*/

	/*public void HideTooltip() {
		PanelManager.Instance.tooltipObject.SetActive(false);
	}*/

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
		//inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		//inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		for (int i = 0; i < slots; i++) {
			GameObject newSlot = Instantiate(PanelManager.Instance.slotPrefab) as GameObject;
			newSlot.transform.SetParent(m_slotContainer.transform, false);
			newSlot.name = "Slot[" + (i / 4) + "][" + (i % 4) + "]";

			RectTransform slotRect = newSlot.GetComponent<RectTransform>();
			//slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), -slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
			//slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), slotPaddingTop * ((i / 4) + 1) + (slotSize * (i / 4)));
			//slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), slotPaddingTop * ((i / 4) + 1) + (slotSize * (i / 4 + 1)));
			slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), - slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
			//slotRect.localPosition = new Vector3(inventoryWidth - slotPaddingLeft * ((i % 4) + 1) - (slotSize * (i % 4)), slotPaddingTop * ((i / 4) + 1) + (slotSize * (i / 4 + 1)));
			slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
			slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
			
			allSlots.Add(newSlot);
		}

		/*for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				GameObject newSlot = Instantiate(slotPrefab) as GameObject;
				//newSlot.transform.SetParent(this.transform, false);
				newSlot.transform.SetParent(m_slotContainer.transform, false);
				newSlot.name = "Slot[" + i + "][" + j + "]";

				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				//slotRect.localPosition = new Vector3(-inventoryWidth + slotPaddingLeft * (j + 1) + (slotSize * j),inventoryHeight -slotPaddingTop * (i + 1) - (slotSize * i));
				slotRect.localPosition = new Vector3(slotPaddingLeft * (j + 1) + (slotSize * j), -slotPaddingTop * (i + 1) - (slotSize * i));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

				allSlots.Add(newSlot);
			}
		}*/
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
				//slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), -slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
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
		//Stack<ItemHolder> itemsToRemove = new Stack<ItemHolder>();

		for (int i = slots-1; i > slots - slotsToRemove - 1; i--) {
			GameObject tempSlotObject = allSlots[i];
			Slot tempSlot = allSlots[i].GetComponent<Slot>();
			tempSlot.DropItems();
			/*if (!tempSlot.IsEmpty) {
				int count = tempSlot.Items.Count;
				for (int j = 0; j < count; j++) {
					itemsToRemove.Push(tempSlot.Items.Pop());	
				}
			}*/

			allSlots.RemoveAt(i);
			DestroyObject(tempSlotObject);
			if (emptySlots > 0)
				emptySlots--;
		}

		//DropItems(itemsToRemove);
	}

	private void RepositionSlots() {
		for (int i = 0; i < slots; i++) {
			RectTransform slotRect = allSlots[i].GetComponent<RectTransform>();
			slotRect.localPosition = new Vector3(slotPaddingLeft * ((i % 4) + 1) + (slotSize * (i % 4)), - slotPaddingTop * ((i / 4) + 1) - (slotSize * (i / 4)));
		}
	}

	public bool AddItem(ItemHolder item)
	{
		if (item.Item.MaxStack == 1)
		{
			PlaceEmpty(item);
			return true;
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

	/*private void PutItemBack() {
		if (PanelManager.Instance.Source != null) {
			Destroy(GameObject.Find("Hover"));
			PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;
			PanelManager.Instance.Source = null;
		} else if (!PanelManager.Instance.MovingSlot.IsEmpty) {
			Destroy(GameObject.Find("Hover"));
			foreach (var item in PanelManager.Instance.MovingSlot.Items) {
				PanelManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
			}

			PanelManager.Instance.MovingSlot.ClearSlot();
		}

		PanelManager.Instance.selectStackSize.SetActive(false);
		//selectStackSize.SetActive(false);
	}*/

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
				if (PanelManager.Instance.Source.type == SlotType.INVENTORY)
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
				/*Stack<ItemHolder> temp = new Stack<ItemHolder>(destinationSlot.Items);
				destinationSlot.AddItems(sourceSlot.Items);
				destinationSlot.UpdateStackText();

				if (temp.Count == 0) {
					sourceSlot.ClearSlot();
				} else {
					sourceSlot.AddItems(temp);
					sourceSlot.UpdateStackText();
				}*/
			}

			PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;
			PanelManager.Instance.Source = null;
			PanelManager.Instance.Destination = null;
			Destroy (GameObject.Find ("Hover"));
			
		}
	}

	/*private void CreateHoverIcon(){
		PanelManager.Instance.HoverObject = Instantiate(PanelManager.Instance.iconPrefab) as GameObject;
		PanelManager.Instance.HoverObject.GetComponent<Image>().sprite = PanelManager.Instance.Clicked.GetComponent<Image>().sprite;
		PanelManager.Instance.HoverObject.name = "Hover";
		
		RectTransform hoverTransform = PanelManager.Instance.HoverObject.GetComponent<RectTransform>();
		RectTransform clickedTransform = PanelManager.Instance.Clicked.GetComponent<RectTransform>();
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x * GameObject.Find("Inventory").GetComponent<RectTransform>().localScale.x);
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y * GameObject.Find("Inventory").GetComponent<RectTransform>().localScale.y);
		
		//hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
		PanelManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
		PanelManager.Instance.HoverObject.transform.localScale = PanelManager.Instance.Clicked.gameObject.transform.localScale;

		if (PanelManager.Instance.Source == null || PanelManager.Instance.Source.IsEmpty)
			PanelManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = PanelManager.Instance.MovingSlot.Items.Count > 1 ? PanelManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
		else
			PanelManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = PanelManager.Instance.Source.Items.Count > 1 ? PanelManager.Instance.Source.Items.Count.ToString() : string.Empty;
	}*/

	/*public void SetStackInfo(int maxStackCount) {
		PanelManager.Instance.selectStackSize.SetActive(true);
		PanelManager.Instance.tooltipObject.SetActive(false);
		PanelManager.Instance.SplitAmount = 0;
		PanelManager.Instance.MaxStackCount = maxStackCount;
		PanelManager.Instance.stackText.text = PanelManager.Instance.SplitAmount.ToString();
	}

	public void ChangeStackText(int i) {
		PanelManager.Instance.SplitAmount += i;

		if (PanelManager.Instance.SplitAmount < 0) {
			PanelManager.Instance.SplitAmount = 0;
		}
		if (PanelManager.Instance.SplitAmount > PanelManager.Instance.MaxStackCount) {
			PanelManager.Instance.SplitAmount = PanelManager.Instance.MaxStackCount;
		}

		PanelManager.Instance.stackText.text = PanelManager.Instance.SplitAmount.ToString();
		//stackSplitText.text = splitAmount.ToString();
	}*/

	public void SplitStack() {
		PanelManager.Instance.selectStackSize.SetActive(false);
		//PanelManager.Instance.selectStackSize.transform.parent.SetActive(false);

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
	
	/*private void Hide() {
		if (shown) {
			shown = false;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x + 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);
			//gameObject.GetComponent<RectTransform>().position.x += inventoryWidth;
		}
	}

	private void Show() {
		if (!shown) {
			shown = true;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x - 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);
			//gameObject.GetComponent<RectTransform>().position.x -= inventoryWidth;
		}
	}*/
}
