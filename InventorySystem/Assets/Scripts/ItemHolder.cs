using UnityEngine;
using System.Collections;

public class ItemHolder : MonoBehaviour {

	private Item item;

	public Item Item {
		get { return item; }
		set {
			item = value;
		}
	}

	public void Use(Slot slot) {
		item.Use(slot, this);
	}

	public string GetTooltip() {
		return item.GetTooltip();
	}
}
