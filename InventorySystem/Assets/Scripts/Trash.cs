using UnityEngine;
using System.Collections;

public class Trash : Item {
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

	public override string GetTooltip() {
		string newLine = string.Empty;
		
		if (ItemDescription != string.Empty)
		{
			newLine = "\n";
		}
		
		return string.Format("<color=white><size=16><b>{0}</b></size></color><size=14><i><color=#940084ff>" + newLine + "{1}</color></i><color=white>\n{2}</color></size>", ItemName, ItemDescription, char.ToUpper(Type.ToString()[0]) + Type.ToString().Substring(1).ToLower());
	}
}
