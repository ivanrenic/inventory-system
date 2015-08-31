using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float moveForce = 180f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 0.2f;				// The fastest the player can travel in the x axis.

	public Inventory inventory;
	public CharacterPanel characterPanel;

	public Animator playerAnimator;
	public Animator shirtAnimator;
	public Animator pantsAnimator;
	public Animator bootsAnimator;
	public Animator hatAnimator;

	public Text speedText;
	public Text radiusText;

	public GameObject pickupRadius;

	private Vector2 defaultRadiusSize;
	private float defaultMoveForce;
	private float defaultMaxSpeed;

	private Animator defaultShirtAnimator;
	private Animator defaultPantsAnimator;
	private Animator defaultBootsAnimator;

	const int STATE_IDLE_FRONT = 0;
	const int STATE_WALK_FRONT = 1;
	const int STATE_IDLE_LEFT = 2;
	const int STATE_WALK_LEFT = 3;
	const int STATE_IDLE_RIGHT = 4;
	const int STATE_WALK_RIGHT = 5;
	const int STATE_IDLE_BACK = 6;
	const int STATE_WALK_BACK = 7;

	private int m_currentAnimationState = STATE_IDLE_FRONT;

	public int CurrentAnimationState {
		get { return m_currentAnimationState; }
	}

	private int m_currentDirection = 0;

	// Use this for initialization
	void Start () {
		defaultBootsAnimator = bootsAnimator;
		defaultPantsAnimator = pantsAnimator;
		defaultShirtAnimator = shirtAnimator;

		defaultRadiusSize = pickupRadius.transform.localScale;
		defaultMoveForce = moveForce;
		defaultMaxSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			inventory.Toggle();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			characterPanel.Toggle();
		}

		if (Input.GetKey(KeyCode.S)) {
			ChangeState(STATE_WALK_FRONT);
			m_currentDirection = 0;
		} else if (Input.GetKey(KeyCode.A)) {
			ChangeState(STATE_WALK_LEFT);
			m_currentDirection = 2;
		} else if (Input.GetKey(KeyCode.D)) {
			ChangeState(STATE_WALK_RIGHT);
			m_currentDirection = 4;
		} else if (Input.GetKey (KeyCode.W)) {
			ChangeState(STATE_WALK_BACK);
			m_currentDirection = 6;
		} else {
			ChangeState(m_currentDirection);
		}

		GameObject.FindWithTag ("MainCamera").transform.position.Set(transform.position.x, transform.position.y, -10);
	}

	void FixedUpdate() {
		// Cache the horizontal input.
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(x * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * x * moveForce);

		if(y * GetComponent<Rigidbody2D>().velocity.y < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * y * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * maxSpeed);
	}

	private void ChangeState(int state) {
		if (state == m_currentAnimationState)
			return;

		switch (state) {
		case STATE_IDLE_FRONT:
			playerAnimator.SetInteger("state", STATE_IDLE_FRONT);
			shirtAnimator.SetInteger("state", STATE_IDLE_FRONT);
			pantsAnimator.SetInteger("state", STATE_IDLE_FRONT);
			bootsAnimator.SetInteger("state", STATE_IDLE_FRONT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_IDLE_FRONT);
			break;
		case STATE_WALK_FRONT:
			playerAnimator.SetInteger("state", STATE_WALK_FRONT);
			shirtAnimator.SetInteger("state", STATE_WALK_FRONT);
			pantsAnimator.SetInteger("state", STATE_WALK_FRONT);
			bootsAnimator.SetInteger("state", STATE_WALK_FRONT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_WALK_FRONT);
			break;
		case STATE_IDLE_LEFT:
			playerAnimator.SetInteger("state", STATE_IDLE_LEFT);
			shirtAnimator.SetInteger("state", STATE_IDLE_LEFT);
			pantsAnimator.SetInteger("state", STATE_IDLE_LEFT);
			bootsAnimator.SetInteger("state", STATE_IDLE_LEFT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_IDLE_LEFT);
			break;
		case STATE_WALK_LEFT:
			playerAnimator.SetInteger("state", STATE_WALK_LEFT);
			shirtAnimator.SetInteger("state", STATE_WALK_LEFT);
			pantsAnimator.SetInteger("state", STATE_WALK_LEFT);
			bootsAnimator.SetInteger("state", STATE_WALK_LEFT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_WALK_LEFT);
			break;
		case STATE_IDLE_RIGHT:
			playerAnimator.SetInteger("state", STATE_IDLE_RIGHT);
			shirtAnimator.SetInteger("state", STATE_IDLE_RIGHT);
			pantsAnimator.SetInteger("state", STATE_IDLE_RIGHT);
			bootsAnimator.SetInteger("state", STATE_IDLE_RIGHT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_IDLE_RIGHT);
			break;
		case STATE_WALK_RIGHT:
			playerAnimator.SetInteger("state", STATE_WALK_RIGHT);
			shirtAnimator.SetInteger("state", STATE_WALK_RIGHT);
			pantsAnimator.SetInteger("state", STATE_WALK_RIGHT);
			bootsAnimator.SetInteger("state", STATE_WALK_RIGHT);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_WALK_RIGHT);
			break;
		case STATE_IDLE_BACK:
			playerAnimator.SetInteger("state", STATE_IDLE_BACK);
			shirtAnimator.SetInteger("state", STATE_IDLE_BACK);
			pantsAnimator.SetInteger("state", STATE_IDLE_BACK);
			bootsAnimator.SetInteger("state", STATE_IDLE_BACK);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_IDLE_BACK);
			break;
		case STATE_WALK_BACK:
			playerAnimator.SetInteger("state", STATE_WALK_BACK);
			shirtAnimator.SetInteger("state", STATE_WALK_BACK);
			pantsAnimator.SetInteger("state", STATE_WALK_BACK);
			bootsAnimator.SetInteger("state", STATE_WALK_BACK);
			if (hatAnimator.gameObject.activeSelf)
				hatAnimator.SetInteger("state", STATE_WALK_BACK);
			break;
		default:
			break;
		}

		m_currentAnimationState = state;
	}

	public void SetStats(int radius, int speed) {
		//GameObject.Find("PickupRadius").GetComponent<Transform>().localScale.Set (10, 10, 1);
		//GameObject.Find("PickupRadius").GetComponent<Transform>().localScale = new Vector3(10, 10, 1);
		pickupRadius.transform.localScale = new Vector3(defaultRadiusSize.x * (1.0f + radius / 10.0f), defaultRadiusSize.y * (1.0f + radius / 10.0f), 1);
		//pickupRadius.transform.localScale.Set(10, 10, 1);
		moveForce = defaultMoveForce * (1.0f + speed / 10.0f);
		maxSpeed = defaultMaxSpeed * (1.0f + speed / 10.0f);

		radiusText.text = radius.ToString();
		speedText.text = speed.ToString();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		/*if(other.tag == "Item" || other.tag == "Dispenser") {
			if (inventory.AddItem(other.GetComponent<ItemHolder>()) && other.tag == "Item")
				Destroy(other.gameObject);
		}*/
	}
}
