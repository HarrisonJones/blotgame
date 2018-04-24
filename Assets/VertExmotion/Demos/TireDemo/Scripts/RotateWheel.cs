using UnityEngine;
using System.Collections;

public class RotateWheel : MonoBehaviour {

    public float m_speed = 10f;
    public float m_jumpForce = 10f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(transform.right, m_speed * Time.deltaTime);

	}

    public void Jump()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity.magnitude < .1f )
            rb.AddForce(Vector3.up * m_jumpForce);
    }
}
