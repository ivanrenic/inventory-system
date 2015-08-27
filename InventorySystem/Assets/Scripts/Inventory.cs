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
	private static GameObject clicked;

	private static GameObject selectStackSizeObject;
	public GameObject selectStackSize;
	public Text stackSplitText;

	private int splitAmount;
	private int maxStackCount;
	private static Slot movingSlot;

	public GameObject TooltipObject;
	private static GameObject tooltip;
	public Text sizeTextObject;
	private static Text sizeText;
	public Text visualTextObject;
	private static Text visualText;

	public GameObject dropItemPrefab;
	private static GameObject playerRef;

	private static Inventory instance;

	public static Inventory Instance {
		get { 
			if (instance == null) {
				instance = GameObject.FindObjectOfType<Inventory>();
			}
			return Inventory.instance; 
		}
	}

	private bool shown = true;

	void Start () {
		tooltip = TooltipObject;
		sizeText = sizeTextObject;
		visualText = visualTextObject;
		selectStackSizeObject = selectStackSize;
		CreateLayout();

		playerRef = GameObject.Find("Player");

		movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
	}

	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			if (!eventSystem.IsPointerOverGameObject(-1) && sourceSlot != null) {
				sourceSlot.GetComponent<Image>().color = Color.white;

				// Dropping the items
				/*foreach (var item in sourceSlot.Items) {
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

					Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

					v *= 3;

					GameObject droppedItem = Instantiate(dropItemPrefab, playerRef.transform.position - v, Quaternion.identity) as GameObject;
					droppedItem.GetComponent<SpriteRenderer>().sprite = item.spriteNeutral;
				}*/
				DropItems(sourceSlot.Items);

				sourceSlot.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				//PutItemBack();
				destinationSlot = null;
				sourceSlot = null;
				Destroy (GameObject.Find ("Hover"));
				emptySlots += 1;
			} else if (!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.IsEmpty) {
				/*foreach (var item in movingSlot.Items) {
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
					
					Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
					
					v *= 3;
					
					GameObject droppedItem = Instantiate(dropItemPrefab, playerRef.transform.position - v, Quaternion.identity) as GameObject;
					droppedItem.GetComponent<SpriteRenderer>().sprite = item.spriteNeutral;
				}*/
				DropItems(movingSlot.Items);

				movingSlot.ClearSlot();
				Destroy (GameObject.Find("Hover"));
			}
		}

		if (hoverObject != null) {
			Vector2 position = Vector2.zero;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, new Vector3(Input.mousePosition.x, Input.mousePosition.y - 1, Input.mousePosition.z), canvas.worldCamera, out position);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}

		if (Input.GetKeyDown(KeyCode.I)) {
			if (shown) {
				tooltip.SetActive(false);
				Hide ();
				if (GameObject.Find("Hover")) {
					PutItemBack();
					/*sourceSlot.GetComponent<Image>().color = Color.white;
					sourceSlot = null;
					destinationSlot = null;
					Destroy (GameObject.Find ("Hover"));*/
				}
			} else Show ();
		}
	}

	private void DropItems(Stack<Item> items) {
		foreach (var item in items) {
			float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
			
			Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
			
			v *= 3;
			
			GameObject droppedItem = Instantiate(dropItemPrefab, playerRef.transform.position - v, Quaternion.identity) as GameObject;
			droppedItem.GetComponent<Item>().SetStats(item);
			droppedItem.GetComponent<SpriteRenderer>().sprite = item.spriteNeutral;
		}
	}

	public void ShowTooltip(GameObject slot) {
		Slot tempSlot = slot.GetComponent<Slot>();

		if (!tempSlot.IsEmpty && hoverObject == null && !selectStackSizeObject.activeSelf) {
			visualText.text = tempSlot.CurrentItem.GetTooltip();
			sizeText.text = visualText.text;

			tooltip.SetActive(true);

			float xPos = slot.transform.position.x + slotPaddingLeft;
			float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

			tooltip.transform.position = new Vector2(xPos, yPos);
		}
	}

	public void HideTooltip() {
		tooltip.SetActive(false);
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
						if (!movingSlot.IsEmpty && clicked.GetComponent<Slot>() == slotScript.GetComponent<Slot>()) {
							continue;
						} else {
							slotScript.AddItem(item);
							return true;
						}
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

	private void PutItemBack() {
		if (sourceSlot != null) {
			Destroy(GameObject.Find("Hover"));
			sourceSlot.GetComponent<Image>().color = Color.white;
			sourceSlot = null;
		} else if (!movingSlot.IsEmpty) {
			Destroy(GameObject.Find("Hover"));
			foreach (var item in movingSlot.Items) {
				clicked.GetComponent<Slot>().AddItem(item);
			}

			movingSlot.ClearSlot();
		}

		selectStackSize.SetActive(false);
	}

	public void MoveItem(GameObject clicked) {
		Inventory.clicked = clicked;

		if (!movingSlot.IsEmpty) {
			Slot tempSlot = clicked.GetComponent<Slot>();

			if (tempSlot.IsEmpty) {
				tempSlot.AddItems(movingSlot.Items);
				movingSlot.Items.Clear();
				Destroy (GameObject.Find("Hover"));
			} else if (!tempSlot.IsEmpty && movingSlot.CurrentItem.type == tempSlot.CurrentItem.type && tempSlot.IsAvailableForStacking) {
				MergeStacks(movingSlot, tempSlot);
			}
		} else if (sourceSlot == null && !Input.GetKey(KeyCode.LeftShift)) {
			if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover")) {
				sourceSlot = clicked.GetComponent<Slot>();
				sourceSlot.GetComponent<Image>().color = Color.gray;

				CreateHoverIcon();
			}
		} else if (destinationSlot == null && !Input.GetKey(KeyCode.LeftShift)) {
			destinationSlot = clicked.GetComponent<Slot>();
			Destroy (GameObject.Find ("Hover"));
		}

		if (destinationSlot != null && sourceSlot != null) {
			if (!destinationSlot.IsEmpty && sourceSlot.CurrentItem.type == destinationSlot.CurrentItem.type && destinationSlot.IsAvailableForStacking) {
				MergeStacks(sourceSlot, destinationSlot);
			} else {
				Stack<Item> temp = new Stack<Item>(destinationSlot.Items);
				destinationSlot.AddItems(sourceSlot.Items);
				destinationSlot.UpdateStackText();

				if (temp.Count == 0) {
					sourceSlot.ClearSlot();
				} else {
					sourceSlot.AddItems(temp);
					sourceSlot.UpdateStackText();
				}
			}

			sourceSlot.GetComponent<Image>().color = Color.white;
			sourceSlot = null;
			destinationSlot = null;
			Destroy (GameObject.Find ("Hover"));
			
		}
	}

	private void CreateHoverIcon(){
		hoverObject = Instantiate(iconPrefab) as GameObject;
		hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
		hoverObject.name = "Hover";
		
		RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
		RectTransform clickedTransform = Inventory.clicked.GetComponent<RectTransform>();
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
		
		//hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
		hoverObject.transform.SetParent(GameObject.Find("Inventory").transform, false);
		hoverObject.transform.localScale = clicked.gameObject.transform.localScale;

		if (sourceSlot == null || sourceSlot.IsEmpty)
			hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
		else
			hoverObject.transform.GetChild(0).GetComponent<Text>().text = sourceSlot.Items.Count > 1 ? sourceSlot.Items.Count.ToString() : string.Empty;
	}

	public void SetStackInfo(int maxStackCount) {
		selectStackSize.SetActive(true);
		tooltip.SetActive(false);
		splitAmount = 0;
		this.maxStackCount = maxStackCount;
		stackSplitText.text = splitAmount.ToString();
	}

	public void ChangeStackText(int i) {
		splitAmount += i;

		if (splitAmount < 0) {
			splitAmount = 0;
		}
		if (splitAmount > maxStackCount) {
			splitAmount = maxStackCount;
		}

		stackSplitText.text = splitAmount.ToString();
	}

	public void SplitStack() {
		selectStackSize.SetActive(false);

		if (splitAmount == maxStackCount) {
			MoveItem(clicked);
		} else if (splitAmount > 0) {
			movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmount);
			CreateHoverIcon();
		}
	}

	public void MergeStacks(Slot sourceSlot, Slot destinationSlot) {
		int max = destinationSlot.CurrentItem.maxStack - destinationSlot.Items.Count;
		int count = sourceSlot.Items.Count < max ? sourceSlot.Items.Count : max;

		for (int i = 0; i < count; i++) {
			destinationSlot.AddItem(sourceSlot.RemoveItem());
			hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count.ToString();
		}

		if (sourceSlot.Items.Count == 0) {
			sourceSlot.ClearSlot();
			Destroy(GameObject.Find("Hover"));
		}
	}
	
	private void Hide() {
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
	}
}
