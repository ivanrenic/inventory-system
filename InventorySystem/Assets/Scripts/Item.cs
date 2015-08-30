using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH, CONSUMEABLE, MAINHAND, TWOHAND, OFFHAND, HEAD, NECK, CHEST, RING, LEGS, BRACERS, BOOTS, TRINKET, SHOULDERS, BELT, GENERIC, GENERICWEAPON};
public enum Quality {COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT};

public abstract class Item : MonoBehaviour {

	public ItemType Type;
	public Quality Quality;
	public Sprite SpriteNeutral;
	public Sprite SpriteHighlighted;
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

		switch (Quality) {
		case Quality.COMMON:
			color = "black";
			break;
		case Quality.UNCOMMON:
			color = "lime";
			break;
		case Quality.RARE:
			color = "navy";
			break;
		case Quality.EPIC:
			color = "magenta";
			break;
		case Quality.LEGENDARY:
			color = "orange";
			break;
		case Quality.ARTIFACT:
			color = "red";
			break;
		}

		return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i>\n{2}</size>", ItemName, ItemDescription, Type.ToString().ToLower());
	}
}
