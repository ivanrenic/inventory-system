using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType {INVENTORY,CHARACTER};

public class Slot : MonoBehaviour, IPointerClickHandler {

	public SlotType type;
	private Stack<ItemHolder> items;
	public Text stackText;
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	public ItemType canContain;

	public Color defaultStackColor;
	public Color maxStackColor;

	public bool IsEmpty
	{
		get { return items.Count == 0; }
	}

	public ItemHolder CurrentItem {
		get { return items.Peek(); }
	}

	public bool IsAvailableForStacking {
		get { return CurrentItem.Item.MaxStack > items.Count; }
	}

	public Stack<ItemHolder> Items {
		get { return items; }
		set { items = value; }
	}

	void Start () {
		items = new Stack<ItemHolder>();
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform textRect = stackText.gameObject.GetComponent<RectTransform>();

		int textScaleFactor = (int) (slotRect.sizeDelta.x * 0.60f);
		stackText.resizeTextMaxSize = textScaleFactor;
		stackText.resizeTextMinSize = textScaleFactor;

		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}

	public void AddItem(ItemHolder item) {
		items.Push(item);

		if (items.Count > 1) {
			stackText.text = items.Count.ToString();
		}

		if (items.Count == CurrentItem.Item.MaxStack) {
			stackText.color = new Color(maxStackColor.r, maxStackColor.g, maxStackColor.b);
		}

		ChangeSprite(item.Item.SpriteNeutral, item.Item.SpriteHighlighted);
	}

	public void AddItems(Stack<ItemHolder> items) {
		this.items = new Stack<ItemHolder>(items);
		UpdateStackText();
		ChangeSprite(CurrentItem.Item.SpriteNeutral, CurrentItem.Item.SpriteHighlighted);
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
			items.Peek().Use(this);

			UpdateStackText();

			if (IsEmpty) {
				ChangeSprite(slotEmpty, slotHighlight);
				PanelManager.Instance.tooltipObject.SetActive(false);
			}
		}
	}

	public void UpdateStackText() {
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		if (items.Count > 0) {
			if (items.Count == CurrentItem.Item.MaxStack) {
				stackText.color = new Color(maxStackColor.r, maxStackColor.g, maxStackColor.b);
			} else {
				stackText.color = new Color(defaultStackColor.r, defaultStackColor.g, defaultStackColor.b);
			}
		}
	}

	public void DropItems() {
		foreach (var item in items) {
			float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
			
			Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
			
			v *= 3;
			
			GameObject droppedItem = Instantiate(PanelManager.Instance.dropItem, PanelManager.Instance.Player.transform.position - v, Quaternion.identity) as GameObject;
			droppedItem.transform.localScale.Set(2, 2, 1);
			droppedItem.GetComponent<ItemHolder>().Item = item.Item;
			droppedItem.GetComponent<SpriteRenderer>().sprite = item.Item.SpriteDrop;
			droppedItem.GetComponent<Item>().SpriteDrop = item.Item.SpriteDrop;
			droppedItem.GetComponent<Item>().SpriteDropPickupable = item.Item.SpriteDropPickupable;
			droppedItem.GetComponent<Item>().SpriteNeutral = item.Item.SpriteNeutral;
			droppedItem.GetComponent<Item>().SpriteHighlighted = item.Item.SpriteHighlighted;
		}
	}

	public void ClearSlot() {
		items.Clear();
		ChangeSprite(slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	}

	public Stack<ItemHolder> RemoveItems(int amount) {
		Stack<ItemHolder> tempStack = new Stack<ItemHolder>();

		for (int i = 0; i < amount; i++) {
			tempStack.Push(items.Pop ());
		}

		UpdateStackText();

		return tempStack;
	}

	public ItemHolder RemoveItem() {
		ItemHolder tempItem;

		tempItem = items.Pop();
		UpdateStackText();

		return tempItem;
	}

	public static void SwapItems(Slot source, Slot destination) {
		ItemType movingType = source.CurrentItem.Item.Type;

		if (source != null && destination != null) {
			bool needToRecalculateStats = source.transform.parent == CharacterPanel.Instance.transform || destination.transform.parent == CharacterPanel.Instance.transform;

			if(destination.canContain == ItemType.GENERIC || movingType == destination.canContain) {
				Stack<ItemHolder> tempDestination = new Stack<ItemHolder>(destination.Items);

				destination.AddItems(source.Items);

				if(tempDestination.Count == 0) {
					if (source.type == SlotType.CHARACTER && destination.type == SlotType.INVENTORY)
						GameObject.Find("Inventory").GetComponent<Inventory>().EmptySlots--;
					else if (source.type == SlotType.INVENTORY && destination.type == SlotType.CHARACTER)
						GameObject.Find("Inventory").GetComponent<Inventory>().EmptySlots++;
					source.ClearSlot();
				} else {
					source.AddItems(tempDestination);
				}
			}

			if (needToRecalculateStats) {
				CharacterPanel.Instance.CalculateStats();
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find ("Hover")) {
			UseItem();
		} else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && items.Count > 1 && !GameObject.Find("Hover")) {
			Vector2 position;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(PanelManager.Instance.canvas.transform as RectTransform, Input.mousePosition, PanelManager.Instance.canvas.worldCamera, out position);

			PanelManager.Instance.selectStackSize.SetActive(true);
			PanelManager.Instance.selectStackSize.transform.position = PanelManager.Instance.canvas.transform.TransformPoint(position);

			PanelManager.Instance.SetStackInfo(items.Count);
		}
	}
}
