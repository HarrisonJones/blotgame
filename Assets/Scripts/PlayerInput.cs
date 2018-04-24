using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInput : MonoBehaviour {

	public float moveSpeed;
	public float minSpeed;
	public float turnSpeed;
	public float minTurnSpeed;
	public float sprintModifier;
	public bool useAcceleration;
	public Material paintingMaterial;
	public Color currentColor = Color.black;
	public AudioClip playerDeath;

	bool allowMovement = true;
	int playerNumber = 0;
	float currentSpeed;
	float greyTolerance = 0.7f;

	private Vector2 lastKnownTextureCoord;

	Painter painter;

	public class CharacterActions : PlayerActionSet
	{
		public PlayerAction Left;
		public PlayerAction Right;

		public CharacterActions()
		{
			Left = CreatePlayerAction("Move Left");
			Right = CreatePlayerAction("Move Right");
		}
	}
	CharacterActions characterActions;

	void Start ()
	{
		DontDestroyOnLoad(this);

		if (GameObject.FindObjectOfType<Painter>())
			painter = GameObject.FindObjectOfType<Painter>();

		ChangeColor(Color.black);

		currentSpeed = minSpeed;
		if (useAcceleration)
			sprintModifier  = 0;

		if (PlayerBounds.S == null)
		{
			GameObject bounds = new GameObject("Bounds");
			bounds.AddComponent<PlayerBounds>().Awake();
		}

		if (!PlayerBounds.S.lockedObjects.Contains(transform))
			PlayerBounds.Bind(transform);
	}

	void Update ()
	{
		if (PlayerBounds.S == null)
		{
			GameObject bounds = new GameObject("Bounds");
			bounds.AddComponent<PlayerBounds>().Awake();
		}

		if (allowMovement)
		{
			if (useAcceleration)
			{
				if (characterActions.Left.IsPressed && characterActions.Right.IsPressed)
					sprintModifier = Mathf.Min(sprintModifier + 0.0175f, 1);

				else
					sprintModifier = Mathf.Max(sprintModifier - 0.064f, 0f);
				currentSpeed = Mathf.Lerp(minSpeed, moveSpeed, sprintModifier);

				transform.Translate(new Vector3(0, 0, currentSpeed * Time.deltaTime), Space.Self);
			}
			else
			{
				if (characterActions.Left.IsPressed && characterActions.Right.IsPressed)
					transform.Translate(new Vector3(0, 0, sprintModifier * moveSpeed * Time.deltaTime), Space.Self);
				else
					transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime), Space.Self);
			}

			if (characterActions.Left.IsPressed)
			{
				transform.Rotate(new Vector3(0, -turnSpeed * Time.deltaTime, 0));
			}
			if (characterActions.Right.IsPressed)
			{
				transform.Rotate(new Vector3(0, turnSpeed * Time.deltaTime, 0));
			}

			// This dictates the width of our character's line to the shader
			// It's important to know your last position as well as your current position so you can fill in the gaps between
			if (painter) {

				lastKnownTextureCoord = painter.Dispense (transform.position, lastKnownTextureCoord, 0.15f * moveSpeed/currentSpeed, paintingMaterial);
				paintingMaterial.SetFloat ("OldCharacterSpeedFactor", 0.15f * moveSpeed/currentSpeed);
				//lastKnownTextureCoord = painter.Dispense (transform.position, lastKnownTextureCoord, currentSpeed/moveSpeed, paintingMaterial);
				//paintingMaterial.SetFloat ("OldCharacterSpeedFactor", currentSpeed/moveSpeed);
			}

			//Cast a ray just in front of the character that's pointing towards the floor.
			//Unity's update frequency can cause inaccuracy with this raycasting, so we check further ahead at higher speeds
			RaycastHit hitInfo = new RaycastHit();

			var speedFactor = 1 + currentSpeed / moveSpeed;
			var startPos = transform.position + (transform.forward.normalized * 0.25f * speedFactor) + Vector3.up * 0.5f;

			Ray ray = new Ray (startPos, Vector3.down);

			var hit = Physics.Raycast (ray, out hitInfo, 10);

			//Make sure we're raycasting against the Arena floor
			if (hit && hitInfo.collider.tag == "ArenaFloor" && painter.PaintTarget != null) {
				var tex2 = new Texture2D (1, 1);

				RenderTexture.active = painter.PaintTarget;

				//Texture coordinates are measured on a scale from 0 to 1. 
				//Multiply that value by the width of the texture we're drawing on to get the actual position
				var pixelX = hitInfo.textureCoord2.x * painter.PaintTarget.width;
				var pixelY = hitInfo.textureCoord2.y * painter.PaintTarget.height;

				//For some reason, Direct3D thinks it's a great idea to invert the Y coordinates of whatever we're hitting.
				if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D11 ||
					SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D12)
					pixelY = painter.PaintTarget.height -pixelY;

				//Copy a 1x1 texture from the arena floor into a temporary Texture2D
				tex2.ReadPixels(new Rect(pixelX, pixelY, pixelX + 1, pixelY + 1), 0, 0);
				tex2.Apply();

				var pixelColor = tex2.GetPixelBilinear (0.5f, 0.5f);

				// We handle death by measuring our colour against the colour of the floor we're standing on
				// Theoretically, you could also use a grey tolerance of ~0.3f if you wanted to use a mostly black map
				if ((pixelColor.r < greyTolerance || pixelColor.g < greyTolerance || pixelColor.b < greyTolerance) && pixelColor != currentColor) {
					Debug.Log ("Died to: " + pixelColor.ToString () + "and my colour is: " + currentColor.ToString ());
					Die ();
				}
			}
		}
	}

	public void SetControls(CharacterActions characterActionsIN)
	{
		characterActions = characterActionsIN;
	}

	public void ReadyUp(int playerNumberIN)
	{
		painter = GameObject.FindObjectOfType<Painter>();
		playerNumber = playerNumberIN;
		switch (playerNumber)
		{
			case 0:
				transform.position = new Vector3(-3, 0, 3f);
				break;
			case 1:
				transform.position = new Vector3(-3, 0, -3f);
				break;
			case 2:
				transform.position = new Vector3(3, 0, 3);
				break;
			case 3:
				transform.position = new Vector3(3, 0, -3f);
				break;
		}
		transform.LookAt(Vector3.zero);
		allowMovement = false;
		//Start countdown. At end of countdown, allowMovement = true
		StartCoroutine(Countdown());
	}

	IEnumerator Countdown()
	{
		yield return new WaitForSeconds(3);
		allowMovement = true;
	}

	public void ChangeColor(Color newColor)
	{
		MeshRenderer[] myMaterials = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < myMaterials.Length; ++i)
		{
			Material newMat = myMaterials[i].material;
			newMat.color = newColor;
			myMaterials[i].material = newMat;

			newMat = new Material(paintingMaterial);
			newMat.SetColor("PaintBrushColour", newColor);
			paintingMaterial = newMat;

			if (currentColor != Color.black)
				Painter.ColoursInUse.Remove (currentColor);

			if (currentColor != Color.white)
				Painter.ColoursInUse.Add (newColor);

			currentColor = newColor;
		}
	}

	void OnCollisionEnter(Collision collision )
	{
		if (collision.collider.gameObject.tag == "Player")
		{
			transform.Rotate(0, 180, 0);
		}
		else if (collision.collider.gameObject.tag == "Obstacle")
		{
			Vector3 mainNormal = collision.contacts[0].normal;
			Vector3 mod1 = Quaternion.AngleAxis(90, Vector3.up) * mainNormal;
			Vector3 mod2 = Quaternion.AngleAxis(90, Vector3.up) * mainNormal;

			if ((transform.rotation.eulerAngles.normalized - mod1).magnitude < (transform.rotation.eulerAngles.normalized - mod2).magnitude)
			{
				transform.rotation = Quaternion.LookRotation(mod1, Vector3.up);
			}
			else
				transform.rotation = Quaternion.LookRotation(mod2, Vector3.up);
		}
	}

	void Die()
	{
		Camera.main.GetComponent<AudioSource>().PlayOneShot(playerDeath);
		characterActions.Left.ClearBindings();
		characterActions.Right.ClearBindings();
		DestroyObject(gameObject);
	}

}
