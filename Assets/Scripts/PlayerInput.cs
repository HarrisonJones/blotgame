using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInput : MonoBehaviour {
	
	[Header("PowerUps")]
	
	private GameObject freezer;
	public bool freezeOn;
	private float speedRate;
	private float speedShoes;
	private float speedShoesMax = 600;

    [Header("Things I have added")]

    [Header("Is the live system active")]
    public bool arelives;

    [Header("Player Lives")]
    public int lives = 3;

    [Header("Live Sprites")]
    public GameObject[] live_sprites;
    public bool live_delay = false;
    private bool invinibility_delay = false;

    [Header("Particle Systems")]
    public ParticleSystem invinciblity;
    public ParticleSystem SpeedBoost;

    [Header("Things here originally")]
	
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

		freezer = GameObject.Find("FreezeManagement");
		
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

	//Time based powerups are reduced per frame rather than through deltaTime.
	void FixedUpdate ()
	{
		
		if (freezeOn)
		{
			speedRate = 0;		
		}
		else if (speedShoes > 0)
		{
			speedRate = 1.5f;
			speedShoes -= Time.deltaTime;
		}		
		else
		{
				speedRate = 1f;
				speedShoes = 0;
		}
	}
	
	void Update ()
	{
		//freezeOn = freezer.gameObject.GetComponent("FreezeManagement").freeze();
		
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
					sprintModifier = Mathf.Min((sprintModifier + 0.0175f) * speedRate, 1);

				else
					sprintModifier = Mathf.Max((sprintModifier - 0.064f) * speedRate, 0f);
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
				if ((pixelColor.r < greyTolerance || pixelColor.g < greyTolerance || pixelColor.b < greyTolerance) && !live_delay && !invinibility_delay)
                {
                    float min_red_range = currentColor.r - 0.002f;
                    float max_red_range = currentColor.r + 0.002f;

                    if (pixelColor.r < min_red_range && pixelColor.r > max_red_range)
                    {
                        Debug.Log("Diff " + "Min: " + min_red_range + " Max: " + max_red_range);
                    }
                    else
                    {
                        Debug.Log("Same: " + "Ori: " + currentColor.r + " Min: " + min_red_range + " Max: " + max_red_range);
                    }

                    //// determines whether to kill or reduce a life based on whether lives are active
                    //if (arelives)
                    //{
                    //    Live_Loss(pixelColor);
                    //}
                    //else
                    //{
                    //    Die();
                    //}
                }
                //else if ((pixelColor.r < greyTolerance || pixelColor.g < greyTolerance || pixelColor.b < greyTolerance))
                //{
                //    Debug.Log("Yeeted to: " + pixelColor.ToString() + "and my colour is: " + currentColor.ToString());
                //}
			}
		}

        // Used to display lives if they are active
        if (arelives)
        {
            switch (lives)
            {
                case 3:
                    live_sprites[2].SetActive(true);
                    live_sprites[1].SetActive(false);
                    live_sprites[0].SetActive(false);
                    break;
                case 2:
                    live_sprites[2].SetActive(false);
                    live_sprites[1].SetActive(true);
                    live_sprites[0].SetActive(false);
                    break;
                case 1:
                    live_sprites[2].SetActive(false);
                    live_sprites[1].SetActive(false);
                    live_sprites[0].SetActive(true);
                    break;
            }

            if (lives <= 0)
            {
                Die();
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

    // Function played when hit and lives are active
    void Live_Loss(Color pixelColor)
    {
        Debug.Log("Live lost to: " + pixelColor.ToString() + "and my colour is: " + currentColor.ToString());
        live_delay = true;
        lives--;
        StartCoroutine("Live_Delay");
    }

    // Delay after being hit causing invulnerability
    IEnumerator Live_Delay()
    {
        yield return new WaitForSeconds(2.0f);
        live_delay = false;
    }

    // Function played if player is hit without lives active or if lives reach zero
    void Die()
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(playerDeath);
        characterActions.Left.ClearBindings();
        characterActions.Right.ClearBindings();
        DestroyObject(gameObject);
    }

    // Function called when powerup is aquired
    public void PowerUp(powerup_types powerup_type, float powerup_time, float sprint_modifier)
    {
        Debug.Log("Powerup: " + powerup_type);

        // SpeedBoost
        if (powerup_type == powerup_types.SpeedBoost)
        {
            // Setting old variables to be reverted back to
            float original_sprint_modifier = sprintModifier;

            // Sets new variables of powerup
            sprintModifier = sprint_modifier;

            // Plays ParticleSystem
            SpeedBoost.Play();

            // Starts powerup timer
            StartCoroutine(Powerup_Timer(powerup_time, original_sprint_modifier));
        }

        // Extra Life
        if (powerup_type == powerup_types.ExtraLife)
        {
            lives++;
        }

        // Invincibility
        if (powerup_type == powerup_types.Invincible)
        {
            // Setting old variables to be reverted back to
            float original_sprint_modifier = sprintModifier;
            invinibility_delay = true;

            // Sets new variables of powerup
            sprintModifier = sprint_modifier;

            // Plays ParticleSystem
            invinciblity.Play();

            // Starts powerup timer
            StartCoroutine(Powerup_Timer(powerup_time, original_sprint_modifier));
        }

        // Nuke (Just for goofs)
        if (powerup_type == powerup_types.Nuke)
        {
            // Setting old variables to be reverted back to
            float original_sprint_modifier = sprintModifier;
            live_delay = true;

            // Sets new variables of powerup
            sprintModifier = sprint_modifier;

            // Starts powerup timer
            StartCoroutine(Powerup_Timer(powerup_time, original_sprint_modifier));
        }
    }

    // Timer for the powerups
    IEnumerator Powerup_Timer(float timer, float original_sprint_modifier)
    {
        yield return new WaitForSeconds(timer);
        // Plays ParticleSystem
        invinciblity.Stop();
        SpeedBoost.Stop();
        sprintModifier = original_sprint_modifier;
        invinibility_delay = false;
    }
}
