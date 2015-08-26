using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH};
public enum Quality {COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT};

public class Item : MonoBehaviour {

	public ItemType type;
	public Quality quality;
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public int maxStack;

	public float strength, intellect, agility, stamina;
	public string itemName;
	public string itemDescription;

	public void Use() {
		switch(type)
		{
			case ItemType.HEALTH:
				Debug.Log ("Used a health potion!");
				break;
			case ItemType.MANA:
				Debug.Log ("Used a mana potion!");
				break;
			default:
				break;
		}
	}

	public string GetTooltip() {
		string stats = string.Empty;
		string color = string.Empty;
		string newLine = string.Empty;

		if (itemDescription != string.Empty) {
			newLine = "\n";
		}

		switch (quality) {
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

		if (strength > 0) {
			stats += "\n+" + strength.ToString() + " Strength";
		}

		if (intellect > 0) {
			stats += "\n+" + intellect.ToString() + " Intellect";
		}

		if (agility > 0) {
			stats += "\n+" + agility.ToString() + " Agility";
		}

		if (stamina > 0) {
			stats += "\n+" + stamina.ToString() + " Stamina";
		}

		return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i>{2}</size>", itemName, itemDescription, stats);
	}
}
