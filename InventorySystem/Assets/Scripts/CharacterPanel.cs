using UnityEngine;
using System.Collections;
using System;

public class CharacterPanel : Panel {
	public Slot[] equipmentSlots;

	private static CharacterPanel instance;

	public static CharacterPanel Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType<CharacterPanel>();

			return CharacterPanel.instance;
		}
	}

	void Awake() {
		//playerRef = GameObject.Find("Player");
		equipmentSlots = transform.GetComponentsInChildren<Slot>();
	}

	public void EquipItem(Slot slot, ItemHolder item) {
		Slot.SwapItems(slot, Array.Find (equipmentSlots, x => x.canContain == item.Item.Type));
	}

	public void UnequipItem(Slot slot, ItemHolder item) {
		Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

		if (inventory.emptySlots > 0) {
			inventory.AddItem(item);
			slot.ClearSlot();
		}

	}

	public bool CanEquip(ItemHolder item) {
		Slot tempSlot = Array.Find(equipmentSlots, x => x.canContain == item.Item.Type);
		return tempSlot.IsEmpty;
	}

	public void CalculateStats() {
	}

	public override void Toggle() {
		if (shown) {
			shown = false;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x + 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);

			if (GameObject.Find("Hover") && PanelManager.Instance.Source != null && PanelManager.Instance.Source.type == SlotType.CHARACTER) {
				PutItemBack();
			}
			/*if (GameObject.Find("Hover") && !GameObject.Find("Inventory").GetComponent<Inventory>().Shown) {
				PutItemBack();
			}*/
		} else {
			shown = true;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x - 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);
		}
	}
}
