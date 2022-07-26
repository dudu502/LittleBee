using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ButtonTypes {
	NotDefined,
	Previous,
	Next
}

public class PEButtonScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler {

	private Button myButton;
	public ButtonTypes ButtonType = ButtonTypes.NotDefined;

	// Use this for initialization
	void Start () {
		myButton = gameObject.GetComponent<Button> ();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		// Used for Tooltip
		UICanvasManager.GlobalAccess.MouseOverButton = true;
		UICanvasManager.GlobalAccess.UpdateToolTip (ButtonType);
	}

	public void OnPointerExit(PointerEventData eventData) {
		// Used for Tooltip
		UICanvasManager.GlobalAccess.MouseOverButton = false;
		UICanvasManager.GlobalAccess.ClearToolTip ();
	}

	public void OnButtonClicked () {
		// Button Click Actions
		UICanvasManager.GlobalAccess.UIButtonClick(ButtonType);
	}
}
