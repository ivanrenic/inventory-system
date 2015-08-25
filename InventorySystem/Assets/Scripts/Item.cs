using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH};

public class Item : MonoBehaviour {

	public ItemType type;
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public int maxStack;

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
}
