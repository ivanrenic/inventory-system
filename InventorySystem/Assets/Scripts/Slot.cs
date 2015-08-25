using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;
	public Text stackText;
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	public Color defaultStackColor;
	public Color maxStackColor;

	public bool IsEmpty
	{
		get { return items.Count == 0; }
	}

	public Item CurrentItem {
		get { return items.Peek(); }
	}

	public bool IsAvailableForStacking {
		get { return CurrentItem.maxStack > items.Count; }
	}

	public Stack<Item> Items {
		get { return items; }
		set { items = value; }
	}

	// Use this for initialization
	void Start () {
		items = new Stack<Item>();
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform textRect = stackText.gameObject.GetComponent<RectTransform>();

		int textScaleFactor = (int) (slotRect.sizeDelta.x * 0.60f);
		stackText.resizeTextMaxSize = textScaleFactor;
		stackText.resizeTextMinSize = textScaleFactor;

		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddItem(Item item) {
		items.Push(item);

		if (items.Count > 1) {
			stackText.text = items.Count.ToString();
		}

		if (items.Count == CurrentItem.maxStack) {
			stackText.color = new Color(maxStackColor.r, maxStackColor.g, maxStackColor.b);
		}

		ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
	}

	public void AddItems(Stack<Item> items) {
		this.items = new Stack<Item>(items);
		UpdateStackText();
		ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
	}

	private void ChangeSprite(Sprite neutralSprite, Sprite highlightSprite) {
		GetComponent<Image>().sprite = neutralSprite;

		SpriteState st = new SpriteState();
		st.highlightedSprite = highlightSprite;
		st.pressedSprite = neutralSprite;
		GetComponent<Button>().spriteState = st;
	}

	private void UseItem() {
		if (!IsEmpty) {
			items.Pop().Use();

			UpdateStackText();
			//stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

			if (IsEmpty) {
				ChangeSprite(slotEmpty, slotHighlight);
				Inventory.EmptySlots += 1;
			}
		}
	}

	public void UpdateStackText() {
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		if (items.Count > 0) {
			if (items.Count == CurrentItem.maxStack) {
				stackText.color = new Color(maxStackColor.r, maxStackColor.g, maxStackColor.b);
			} else {
				stackText.color = new Color(defaultStackColor.r, defaultStackColor.g, defaultStackColor.b);
			}
		}
	}

	public void ClearSlot() {
		items.Clear();
		ChangeSprite(slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	}

	public Stack<Item> RemoveItems(int amount) {
		Stack<Item> tempStack = new Stack<Item>();

		for (int i = 0; i < amount; i++) {
			tempStack.Push(items.Pop ());
		}

		UpdateStackText();

		return tempStack;
	}

	public Item RemoveItem() {
		Item tempItem;

		tempItem = items.Pop();
		UpdateStackText();

		return tempItem;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find ("Hover")) {
			UseItem();
		} else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && items.Count > 1 && !GameObject.Find("Hover")) {
			Vector2 position;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);

			Inventory.Instance.selectStackSize.SetActive(true);
			Inventory.Instance.selectStackSize.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);

			Inventory.Instance.SetStackInfo(items.Count);
		}
	}
}
