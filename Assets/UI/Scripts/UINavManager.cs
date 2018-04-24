using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InterfaceMovement;
using InControl;
using BindingsExample;

// Script adapted from InControl's ButtonManager script. Manages the selection/focus and navigation of buttons, based on the input of the players.
// By Peter Liang.
public class UINavManager : MonoBehaviour
{
	public UINav focusedButton;				// Tracks the active button.

	UIActions uiActions;					// Provides the keyboard inputs.
	TwoAxisInputControl filteredDirection;	// Provides the joystick/controller inputs.

	float cooloff = 0;						// Timer to prevent button navigation loop if the button/axis is held down. Locks the navigation until release or another button/axis is pressed.
	PlayerAction lastAction;				// Tracks the actively held down button/axi, to assist with the cooloff timer.


	// Use this for initialization. Occurs before Start(), will execute even if script is disabled (but not if gameObject is disabled).
	void Awake()
	{
		filteredDirection = new TwoAxisInputControl();
		filteredDirection.StateThreshold = 0.5f;

		uiActions = UIActions.CreateWithDefaultBindings();
	}

	// Update is called once per frame
	void Update()
	{
		// Use last device which provided input.
		var inputDevice = InputManager.ActiveDevice;
		filteredDirection.Filter(inputDevice.Direction, Time.deltaTime);

		// Move focus with directional inputs.
		if (filteredDirection.Up.WasPressed || uiActions.Up)
		{
			if (cooloff <= 0 || lastAction != uiActions.Up)
				MoveFocusTo(focusedButton.up);

			cooloff = Time.deltaTime * 2;
			lastAction = uiActions.Up;
		}

		if (filteredDirection.Down.WasPressed || uiActions.Down)
		{
			if (cooloff <= 0 || lastAction != uiActions.Down)
				MoveFocusTo(focusedButton.down);

			cooloff = Time.deltaTime * 2;
			lastAction = uiActions.Down;
		}

		if (filteredDirection.Left.WasPressed || uiActions.Left)
		{
			if (cooloff <= 0 || lastAction != uiActions.Left)
				MoveFocusTo(focusedButton.left);

			cooloff = Time.deltaTime * 2;
			lastAction = uiActions.Left;
		}

		if (filteredDirection.Right.WasPressed || uiActions.Right)
		{
			if (cooloff <= 0 || lastAction != uiActions.Right)
				MoveFocusTo(focusedButton.right);

			cooloff = Time.deltaTime * 2;
			lastAction = uiActions.Right;
		}

		// Fire/Invoke the events specified on the Unity UI Button's onClick List.
		if (inputDevice.Action1.IsPressed || uiActions.Fire)
			focusedButton.button.onClick.Invoke();

		// Continually counts down the cooloff timer to release the UI navigation lock.
		if (cooloff > 0)
			cooloff -= Time.deltaTime;
	}

	// Method to set the focus to another UINav, usually based on the active UINav's direction relative to the current axis input.
	public void MoveFocusTo(UINav newFocusedButton)
	{
		if (newFocusedButton != null)
		{
			focusedButton = newFocusedButton;
		}
	}
}

// Adapted from InControl's PlayerActions script, under Examples/Bindings. Used to provide default keyboard bindings for the UI events.
public class UIActions : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerAction Fire;

	// Object Constructor.
	public UIActions()
	{
		Left = CreatePlayerAction( "Move Left" );
		Right = CreatePlayerAction( "Move Right" );
		Up = CreatePlayerAction( "Move Up" );
		Down = CreatePlayerAction( "Move Down" );
		Fire = CreatePlayerAction( "Fire" );
	}

	// Assigns default keyboard bindings. Assumes arrow keys & WASD bindings.
	public static UIActions CreateWithDefaultBindings()
	{
		var uiActions = new UIActions();

		uiActions.Up.AddDefaultBinding( Key.UpArrow );
		uiActions.Down.AddDefaultBinding( Key.DownArrow );
		uiActions.Left.AddDefaultBinding( Key.LeftArrow );
		uiActions.Right.AddDefaultBinding( Key.RightArrow );

		uiActions.Up.AddDefaultBinding( Key.W );
		uiActions.Down.AddDefaultBinding( Key.S );
		uiActions.Left.AddDefaultBinding( Key.A );
		uiActions.Right.AddDefaultBinding( Key.D );

		uiActions.Fire.AddDefaultBinding( Key.Return );
		uiActions.Fire.AddDefaultBinding( Key.PadEnter );

		uiActions.ListenOptions.OnBindingFound = ( action, binding ) => {
			if (binding == new KeyBindingSource( Key.Escape ))
			{
				action.StopListeningForBinding();
				return false;
			}
			return true;
		};

		uiActions.ListenOptions.OnBindingAdded += ( action, binding ) => {
			Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
		};

		uiActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) => {
			Debug.Log( "Binding rejected... " + reason );
		};

		return uiActions;
	}
}