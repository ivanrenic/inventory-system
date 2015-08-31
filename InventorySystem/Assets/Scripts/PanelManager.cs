using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour {

	private static PanelManager m_instance;

	public static PanelManager Instance {
		get {
			if (m_instance == null)
				m_instance = FindObjectOfType<PanelManager>();

			return m_instance;
		}
	}

	public GameObject Player;
	public GameObject slotPrefab;
	public GameObject iconPrefab;
	private GameObject m_hoverObject;
	public GameObject dropItem;
	public GameObject tooltipObject;
	public Text sizeTextObject;
	public Text visualTextObject;
	public Canvas canvas;
	private Slot m_source;
	private Slot m_destination;
	private Slot m_movingSlot;
	private GameObject m_clicked;
	public Text stackText;
	public GameObject selectStackSize;
	private int m_splitAmount;
	private int m_maxStackCount;
	public EventSystem eventSystem;

	public Slot Source
	{
		get { return m_source; }
		set { m_source = value; }
	}
	
	public Slot Destination
	{
		get { return m_destination; }
		set { m_destination = value; }
	}
	
	public GameObject Clicked
	{
		get { return m_clicked; }
		set { m_clicked = value; }
	}
	
	public int SplitAmount
	{
		get { return m_splitAmount; }
		set { m_splitAmount = value; }
	}
	
	public int MaxStackCount
	{
		get { return m_maxStackCount; }
		set { m_maxStackCount = value; }
	}
	
	public Slot MovingSlot
	{
		get { return m_movingSlot; }
		set { m_movingSlot = value; }
	}
	
	public GameObject HoverObject
	{
		get { return m_hoverObject; }
		set { m_hoverObject = value; }
	}

	public void SetStackInfo(int maxStackCount)
	{
		selectStackSize.SetActive(true);
		tooltipObject.SetActive(false);
		

		m_splitAmount = 0;

		m_maxStackCount = maxStackCount;

		stackText.text = m_splitAmount.ToString();
	}
}
