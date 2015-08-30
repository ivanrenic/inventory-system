using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Pickup : MonoBehaviour {
	void Awake() {
		gameObject.GetComponent<ItemHolder>().Item = gameObject.GetComponent<Item>();
	}

	void OnMouseUp() {
		GameObject.Find("Inventory").GetComponent<Inventory>().AddItem(gameObject.GetComponent<ItemHolder>());
		Destroy(gameObject);
	}
}
