using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using InterfaceMovement;

// Script adapted from InControl's Button script. Represents each button element, to be managed by the ButtonManager,
// yet also abide by Unity's UI functionality.
// By Peter Liang.
public class UINav : MonoBehaviour
{
	public UnityEngine.UI.Button button { get; set; }
	public GameObject text;

	UINavManager navManager;				// The manager of the UINav component,
	StandaloneInputModuleV2 eventSystem;	// Custom input module to allow the gameObject under a cursor to be retrieved.

	// Represents which UINav should be focused on when user makes a directional input on the specific active UINav.
	public UINav up = null;
	public UINav down = null;
	public UINav left = null;
	public UINav right = null;

	// Use this for initialization
	void Start()
	{
		button = GetComponent<UnityEngine.UI.Button>();
		navManager = transform.root.GetComponent<UINavManager>();		// Navmanager should be the root of the transform hierarchy
		eventSystem = FindObjectOfType<StandaloneInputModuleV2>();
	}

	// Update is called once per frame
	void Update()
	{
		// Used to manually set focus to this if the mouse pointer is hovering over the text gameObject.
		if (eventSystem.GameObjectUnderPointer() == text)
			navManager.MoveFocusTo(this);

		// Find out if we're the focused button.
		var hasFocus = navManager.focusedButton == this;

		// Sets the button focus to be on this button. Focus management is controlled by Unity's UI components
		if (hasFocus)
			button.Select();
	}
}
