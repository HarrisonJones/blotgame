	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour {

	public static List<Color> ColoursInUse = new List<Color>();

	public Color ClearColour;
	public RenderTexture PaintTarget;
	private RenderTexture TempRenderTarget;
	private Material ThisMaterial;

	void Init()
	{
		if (ThisMaterial == null)
			ThisMaterial = this.GetComponent<Renderer>().material;

		// Only set up the object we're painting on once
		if (PaintTarget != null )
		if (ThisMaterial.mainTexture == PaintTarget)
			return;

		// Set up a new paint target every time, otherwise it'll persist in debug builds.
		if (ThisMaterial.mainTexture != null)
		{
			if (PaintTarget == null)
				PaintTarget = new RenderTexture(ThisMaterial.mainTexture.width, ThisMaterial.mainTexture.height, 0);
			Graphics.Blit(ThisMaterial.mainTexture, PaintTarget);
			ThisMaterial.mainTexture = PaintTarget;
		}
	}

	public Vector2 Dispense(Vector3 currentPosition, Vector2 lastKnownTextureCoord, float currentSpeedFactor, Material paintingMaterial)
	{
		RaycastHit hitInfo = new RaycastHit();
		Ray ray = new Ray(currentPosition + Vector3.up, Vector3.down);

		if (Physics.Raycast(ray, out hitInfo))
		{
			Vector2 LocalHit2 = hitInfo.textureCoord;

			if (lastKnownTextureCoord != Vector2.zero)
				PaintAt(LocalHit2, lastKnownTextureCoord, currentSpeedFactor, paintingMaterial);
			
			return LocalHit2;
		}

		return Vector2.zero;
	}

	void PaintAt(Vector2 Uv, Vector2 OldUv, float currentSpeedFactor, Material paintingMaterial)
	{
		// Set the player's current speed, as well as the UV they were on during our last and current calculations
		paintingMaterial.SetFloat ("CharacterSpeedFactor", currentSpeedFactor);
		paintingMaterial.SetVector("PaintUv", Uv);
		paintingMaterial.SetVector ("OldPaintUv", OldUv);

		Init();

		if ( TempRenderTarget == null )
		{
			TempRenderTarget = new RenderTexture(PaintTarget.width, PaintTarget.height, 0);
		}

		Graphics.Blit(PaintTarget, TempRenderTarget);
		Graphics.Blit(TempRenderTarget,PaintTarget, paintingMaterial);
	}
}
