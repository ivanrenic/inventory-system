using UnityEngine;
using System.Collections;

public class WatchRadius : MonoBehaviour {
	private PolygonCollider2D collider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Trash") {
			other.gameObject.GetComponent<Item>().Glow();
			other.gameObject.GetComponent<Pickup>().Pickupable = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Trash") {
			other.gameObject.GetComponent<Item>().Unglow();
			other.gameObject.GetComponent<Pickup>().Pickupable = false;
		}
	}
}
