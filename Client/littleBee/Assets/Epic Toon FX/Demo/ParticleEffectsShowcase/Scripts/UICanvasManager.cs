using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour {
	public static UICanvasManager GlobalAccess;
	void Awake () {
		GlobalAccess = this;
	}

	public bool MouseOverButton = false;
	public Text PENameText;
	public Text ToolTipText;

	// Use this for initialization
	void Start () {
		if (PENameText != null)
			PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
	}
	
	// Update is called once per frame
	void Update () {
	
		// Mouse Click - Check if mouse over button to prevent spawning particle effects while hovering or using UI buttons.
		if (!MouseOverButton) {
			// Left Button Click
			if (Input.GetMouseButtonUp (0)) {
				// Spawn Currently Selected Particle System
				SpawnCurrentParticleEffect();
			}
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			SelectPreviousPE ();
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			SelectNextPE ();
		}
	}

	public void UpdateToolTip(ButtonTypes toolTipType) {
		if (ToolTipText != null) {
			if (toolTipType == ButtonTypes.Previous) {
				ToolTipText.text = "Select Previous Particle Effect";
			}
			else if (toolTipType == ButtonTypes.Next) {
				ToolTipText.text = "Select Next Particle Effect";
			}
		}
	}
	public void ClearToolTip() {
		if (ToolTipText != null) {
			ToolTipText.text = "";
		}
	}

	private void SelectPreviousPE() {
		// Previous
		ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
		if (PENameText != null)
			PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
	}
	private void SelectNextPE() {
		// Next
		ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
		if (PENameText != null)
			PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
	}

	private RaycastHit rayHit;
	private void SpawnCurrentParticleEffect() {
		// Spawn Particle Effect
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (mouseRay, out rayHit)) {
			ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect (rayHit.point);
		}
	}

	/// <summary>
	/// User interfaces the button click.
	/// </summary>
	/// <param name="buttonTypeClicked">Button type clicked.</param>
	public void UIButtonClick(ButtonTypes buttonTypeClicked) {
		switch (buttonTypeClicked) {
		case ButtonTypes.Previous:
			// Select Previous Prefab
			SelectPreviousPE();
			break;
		case ButtonTypes.Next:
			// Select Next Prefab
			SelectNextPE();
			break;
		default:
			// Nothing
			break;
		}
	}
}
