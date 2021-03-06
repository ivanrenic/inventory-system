﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Panel : MonoBehaviour {
	protected bool shown = true;
	
	public bool Shown {
		get { return shown; }
	}

	protected void PutItemBack() {
		if (PanelManager.Instance.Source != null) {
			Destroy(GameObject.Find("Hover"));
			PanelManager.Instance.Source.GetComponent<Image>().color = Color.white;
			PanelManager.Instance.Source = null;
		} else if (!PanelManager.Instance.MovingSlot.IsEmpty) {
			Destroy(GameObject.Find("Hover"));
			foreach (var item in PanelManager.Instance.MovingSlot.Items) {
				PanelManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
			}
			
			PanelManager.Instance.MovingSlot.ClearSlot();
		}
		
		PanelManager.Instance.selectStackSize.SetActive(false);
	}

	public void ShowTooltip(GameObject slot) {
		Slot tempSlot = slot.GetComponent<Slot>();
		
		if (!tempSlot.IsEmpty && PanelManager.Instance.HoverObject == null && !PanelManager.Instance.selectStackSize.transform.gameObject.activeSelf) {
			PanelManager.Instance.visualTextObject.text = tempSlot.CurrentItem.GetTooltip();
			PanelManager.Instance.sizeTextObject.text = PanelManager.Instance.visualTextObject.text;
			
			PanelManager.Instance.tooltipObject.SetActive(true);
			
			float xPos = slot.transform.position.x;
			float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y;
			
			PanelManager.Instance.tooltipObject.transform.position = new Vector2(xPos, yPos);
		}
	}

	public void HideTooltip() {
		PanelManager.Instance.tooltipObject.SetActive(false);
	}

	protected void CreateHoverIcon(){
		PanelManager.Instance.HoverObject = Instantiate(PanelManager.Instance.iconPrefab) as GameObject;
		PanelManager.Instance.HoverObject.GetComponent<Image>().sprite = PanelManager.Instance.Clicked.GetComponent<Image>().sprite;
		PanelManager.Instance.HoverObject.name = "Hover";
		
		RectTransform hoverTransform = PanelManager.Instance.HoverObject.GetComponent<RectTransform>();

		float slotSize = GameObject.Find("Inventory").GetComponent<Inventory>().GetSlotSize();
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * GameObject.Find("Inventory").GetComponent<RectTransform>().localScale.x);
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * GameObject.Find("Inventory").GetComponent<RectTransform>().localScale.y);

		PanelManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
		PanelManager.Instance.HoverObject.transform.localScale = PanelManager.Instance.Clicked.gameObject.transform.localScale;
		
		if (PanelManager.Instance.Source == null || PanelManager.Instance.Source.IsEmpty)
			PanelManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = PanelManager.Instance.MovingSlot.Items.Count > 1 ? PanelManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
		else
			PanelManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = PanelManager.Instance.Source.Items.Count > 1 ? PanelManager.Instance.Source.Items.Count.ToString() : string.Empty;
	}

	public void SetStackInfo(int maxStackCount) {
		PanelManager.Instance.selectStackSize.SetActive(true);
		PanelManager.Instance.tooltipObject.SetActive(false);
		PanelManager.Instance.SplitAmount = 0;
		PanelManager.Instance.MaxStackCount = maxStackCount;
		PanelManager.Instance.stackText.text = PanelManager.Instance.SplitAmount.ToString();

	}
	
	public void ChangeStackText(int i) {
		PanelManager.Instance.SplitAmount += i;
		
		if (PanelManager.Instance.SplitAmount < 0) {
			PanelManager.Instance.SplitAmount = 0;
		}
		if (PanelManager.Instance.SplitAmount > PanelManager.Instance.MaxStackCount) {
			PanelManager.Instance.SplitAmount = PanelManager.Instance.MaxStackCount;
		}
		
		PanelManager.Instance.stackText.text = PanelManager.Instance.SplitAmount.ToString();
	}

	public abstract void Toggle();
}
