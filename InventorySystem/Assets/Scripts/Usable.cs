using UnityEngine;
using System.Collections;

public class Usable : Item {
	public int Speed;
	public int Radius;

	public override void Use(Slot slot, ItemHolder item) {
		Debug.Log("Used usable " + ItemName);
	}

	void Awake() {
		ItemHolder itemHolder = gameObject.GetComponent<ItemHolder>();

		if (itemHolder != null)
			itemHolder.Item = gameObject.GetComponent<Usable>();
	}

	public override string GetTooltip() {
		string stats = string.Empty;  //Resets the stats info
		string color = string.Empty;  //Resets the color info
		string newLine = string.Empty; //Resets the new line
		
		if (ItemDescription != string.Empty) //Creates a newline if the item has a description, this is done to makes sure that the headline and the describion isn't on the same line
		{
			newLine = "\n";
		}
		
		switch (Quality) //Sets the color accodring to the quality of the item
		{
		case Quality.COMMON:
			color = "white";
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
