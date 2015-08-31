using UnityEngine;
using System.Collections;

public enum ItemType {KAPA, MAJICA, HLACE, CIPELE, TRASH, ALUMINIJ, PLASTIKA, BATERIJA, BRIEFCASE, GENERIC};

public abstract class Item : MonoBehaviour {

	public ItemType Type;
	public Sprite SpriteNeutral;
	public Sprite SpriteHighlighted;
	public Sprite SpriteDrop;
	public Sprite SpriteDropPickupable;
	public int MaxStack;

	public string ItemName;
	public string ItemDescription;

	public abstract void Use(Slot slot, ItemHolder item);

	public virtual string GetTooltip() {
		string stats = string.Empty;
		string color = string.Empty;
		string newLine = string.Empty;

		if (ItemDescription != string.Empty) {
			newLine = "\n";
		}

		return string.Format("<color=white><size=16><b>{0}</b></size></color><size=14><i><color=#940084ff>" + newLine + "{1}</color></i>\n<color=white>{2}</color></size>", ItemName, ItemDescription, Type.ToString().ToLower());
	}

	public void Glow() {
		gameObject.GetComponent<SpriteRenderer>().sprite = SpriteDropPickupable;
	}
	
	public void Unglow() {
		gameObject.GetComponent<SpriteRenderer>().sprite = SpriteDrop;
	}
}
