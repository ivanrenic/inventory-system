using UnityEngine;
using System.Collections;

public class Equipment : Item {
	private int speed;

	public int Speed {
		get { return speed; }
	}

	private int radius;

	public int Radius {
		get { return radius; }
	}

	public override void Use(Slot slot, ItemHolder item) {
		if (slot.type == SlotType.INVENTORY)
			CharacterPanel.Instance.EquipItem(slot, item);
		else if (slot.type == SlotType.CHARACTER)
			CharacterPanel.Instance.UnequipItem(slot, item);
	}

	public override string GetTooltip()
	{
		string stats = string.Empty;
		
		if (Speed > 0) {
			stats += "\n+" + Speed.ToString() + " Speed";
		}
		if (Radius > 0) {
			stats += "\n+" + Radius.ToString() + " Radius";
		}

		string itemTip = base.GetTooltip();

		return string.Format("{0}" + "<size=14>{1}</size>", itemTip, stats);
	}
}
