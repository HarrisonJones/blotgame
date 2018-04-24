using UnityEngine;
using System.Collections;
namespace Kalagaan
{
    public class EyeDemo : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 lookpos = Camera.main.transform.position;
            lookpos.y = transform.position.y;
            transform.LookAt(lookpos, Vector3.up);
            transform.rotation *= Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
