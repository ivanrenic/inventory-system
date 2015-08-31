using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Pickup : MonoBehaviour {
	public bool Pickupable = false;

	void Awake() {
		gameObject.GetComponent<ItemHolder>().Item = gameObject.GetComponent<Item>();
	}

	void OnMouseDown() {
		if (Pickupable) {
			if (GameObject.Find("Inventory").GetComponent<Inventory>().AddItem(gameObject.GetComponent<ItemHolder>()))
				Destroy(gameObject);
		}
	}
}
