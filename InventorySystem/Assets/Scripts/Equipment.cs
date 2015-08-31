using UnityEngine;
using System.Collections;

public class Equipment : Item {
	public Sprite Equip;
	public int Speed;

	public int Radius;
	
	/*private RuntimeAnimatorController animator;

	public RuntimeAnimatorController Animator {
		get { return animator; }
	}*/

	void Awake() {
		//animator = gameObject.GetComponent<Animator>().runtimeAnimatorController;
	}

	public override void Use(Slot slot, ItemHolder item) {
		if (slot.type == SlotType.INVENTORY)
			CharacterPanel.Instance.EquipItem(slot, item, Equip);//, animator);
		else if (slot.type == SlotType.CHARACTER)
			CharacterPanel.Instance.UnequipItem(slot, item);
	}

	public override string GetTooltip()
	{
		string stats = string.Empty;
		
		if (Speed > 0) {
			stats += "\n+" + Speed.ToString() + " brzina";
		}
		if (Radius > 0) {
			stats += "\n+" + Radius.ToString() + " radijus";
		}

		string itemTip = base.GetTooltip();

		return string.Format("{0}" + "<size=14><color=white>{1}</color></size>", itemTip, stats);
	}
}
