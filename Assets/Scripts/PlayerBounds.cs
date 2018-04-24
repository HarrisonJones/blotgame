using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerBounds : MonoBehaviour
{
	public Vector2 bounds = new Vector2(20f, 11f);
	public List<Transform> lockedObjects = new List<Transform>();

	public static PlayerBounds S;

	public void Awake()
	{
		S = this;
		lockedObjects.AddRange(FindObjectsOfType<PlayerInput>().Select(i => i.transform).ToList());
	}

	public static void Bind(Transform obj)
	{
		S.lockedObjects.Add(obj);
	}

	// Update is called once per frame
	void Update()
	{
		foreach (var obj in lockedObjects)
		{
			if (obj)
			{
				Vector3 pos = obj.transform.position;

				float x = transform.position.x;
				float y = transform.position.z;

				pos.x = Mathf.Clamp(pos.x, -bounds.x / 2f + x, bounds.x / 2f + x);
				pos.z = Mathf.Clamp(pos.z, -bounds.y / 2f + y, bounds.y / 2f + y);

				obj.transform.position = pos;
			}
		}
	}

	void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(bounds.x, 1, bounds.y));
    }
}
