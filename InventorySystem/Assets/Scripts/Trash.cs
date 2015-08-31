using UnityEngine;
using System.Collections;

//public enum TrashType {ALUMINIJ, PLASTIKA, BATERIJA};

public class Trash : Item {
	//public TrashType TrashType;
	public int Speed;
	public int Radius;

	public override void Use(Slot slot, ItemHolder item) {
		Debug.Log("Used usable " + ItemName);
	}

	void Awake() {
		ItemHolder itemHolder = gameObject.GetComponent<ItemHolder>();

		if (itemHolder != null)
			itemHolder.Item = gameObject.GetComponent<Trash>();
	}

	/*public void Glow() {
		gameObject.GetComponent<SpriteRenderer>().sprite = SpriteDropPickupable;
	}

	public void Unglow() {
		gameObject.GetComponent<SpriteRenderer>().sprite = SpriteDrop;
	}*/

	public override string GetTooltip() {
		string stats = string.Empty;  //Resets the stats info
		string color = string.Empty;  //Resets the color info
		string newLine = string.Empty; //Resets the new line
		
		if (ItemDescription != string.Empty) //Creates a newline if the item has a description, this is done to makes sure that the headline and the describion isn't on the same line
		{
			newLine = "\n";
		}
		
		return string.Format("<color=white><size=16><b>{0}</b></size></color><size=14><i><color=#940084ff>" + newLine + "{1}</color></i><color=white>\n{2}</color></size>", ItemName, ItemDescription, char.ToUpper(Type.ToString()[0]) + Type.ToString().Substring(1).ToLower());
	}
}
