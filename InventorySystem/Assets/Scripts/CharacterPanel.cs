using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CharacterPanel : Panel {
	public Slot[] equipmentSlots;

	private static CharacterPanel instance;

	public static CharacterPanel Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType<CharacterPanel>();

			return CharacterPanel.instance;
		}
	}

	public GameObject Player;
	public Image HatSprite;
	public Image ShirtSprite;
	public Image PantsSprite;
	public Image BootsSprite;

	private Sprite defaultShirt;
	private Sprite defaultPants;
	private Sprite defaultBoots;

	void Awake() {
		//playerRef = GameObject.Find("Player");
		equipmentSlots = transform.GetComponentsInChildren<Slot>();
		defaultShirt = ShirtSprite.sprite;
		defaultPants = PantsSprite.sprite;
		defaultBoots = BootsSprite.sprite;
	}

	public void EquipItem(Slot slot, ItemHolder item, Sprite equipSprite) {//, RuntimeAnimatorController animator) {
		//AnimatorStateInfo currentState;
		//Player playerScript = Player.GetComponent<Player>();
		Slot.SwapItems(slot, Array.Find (equipmentSlots, x => x.canContain == item.Item.Type));

		switch (item.Item.Type) {
		case ItemType.CIPELE:
			BootsSprite.sprite = equipSprite;
			/*currentState = playerScript.bootsAnimator.GetCurrentAnimatorStateInfo(-5);
			playerScript.bootsAnimator.runtimeAnimatorController = animator;
			playerScript.bootsAnimator.Play(playerScript.CurrentAnimationState, -5, currentState.normalizedTime);*/
			break;
		case ItemType.HLACE:
			PantsSprite.sprite = equipSprite;
			/*currentState = playerScript.pantsAnimator.GetCurrentAnimatorStateInfo(-4);
			playerScript.pantsAnimator.runtimeAnimatorController = animator;
			playerScript.pantsAnimator.Play(playerScript.CurrentAnimationState, -4, currentState.normalizedTime);*/
			break;
		case ItemType.MAJICA:
			ShirtSprite.sprite = equipSprite;
			/*currentState = playerScript.shirtAnimator.GetCurrentAnimatorStateInfo(-3);
			playerScript.shirtAnimator.runtimeAnimatorController = animator;
			playerScript.shirtAnimator.Play(playerScript.CurrentAnimationState, -3, currentState.normalizedTime);*/
			break;
		case ItemType.KAPA:
			HatSprite.sprite = equipSprite;
			HatSprite.gameObject.SetActive(true);
			/*currentState = playerScript.hatAnimator.GetCurrentAnimatorStateInfo(-2);
			playerScript.hatAnimator.runtimeAnimatorController = animator;
			playerScript.hatAnimator.Play(playerScript.CurrentAnimationState, -2, currentState.normalizedTime);*/
			break;
		default:
			break;
		}
	}

	public void UnequipItem(Slot slot, ItemHolder item) {
		Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

		if (inventory.emptySlots > 0) {
			inventory.AddItem(item);
			slot.ClearSlot();

			switch (item.Item.Type) {
			case ItemType.CIPELE:
				BootsSprite.sprite = defaultBoots;
				break;
			case ItemType.HLACE:
				PantsSprite.sprite = defaultPants;
				break;
			case ItemType.MAJICA:
				ShirtSprite.sprite = defaultShirt;
				break;
			case ItemType.KAPA:
				HatSprite.gameObject.SetActive(false);
				break;
			default:
				break;
			}

			CalculateStats();
		}

	}

	public bool CanEquip(ItemHolder item) {
		Slot tempSlot = Array.Find(equipmentSlots, x => x.canContain == item.Item.Type);
		return tempSlot.IsEmpty;
	}

	public void CalculateStats()
	{
		int radius = 0;
		int speed = 0;
		
		foreach (Slot slot in equipmentSlots)
		{
			if (!slot.IsEmpty)
			{
				Equipment item = (Equipment)slot.CurrentItem.Item;
				radius += item.Radius;
				speed += item.Speed;
			}
		}
		
		Player.GetComponent<Player>().SetStats(radius, speed);
	}
	
	public override void Toggle() {
		if (shown) {
			shown = false;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x + 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);

			if (GameObject.Find("Hover") && PanelManager.Instance.Source != null && PanelManager.Instance.Source.type == SlotType.CHARACTER) {
				PutItemBack();
			}
			/*if (GameObject.Find("Hover") && !GameObject.Find("Inventory").GetComponent<Inventory>().Shown) {
				PutItemBack();
			}*/
		} else {
			shown = true;
			gameObject.GetComponent<RectTransform>().position = new Vector3(
				gameObject.GetComponent<RectTransform>().position.x - 1000f,
				gameObject.GetComponent<RectTransform>().position.y,
				gameObject.GetComponent<RectTransform>().position.z);
		}
	}
}
