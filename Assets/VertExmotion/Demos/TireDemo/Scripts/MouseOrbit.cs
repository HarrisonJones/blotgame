using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Kalagaan
{
    public class MouseOrbit : MonoBehaviour, IDragHandler
    {
        public Camera m_cam;
        public Transform target;
        public float xSpeed = 1f;
        public float ySpeed = 1f;


        public void OnDrag( PointerEventData ped )
        {
            if (target && Input.GetMouseButton(0))
            {
                m_cam.transform.RotateAround(target.transform.position, Vector3.up, ped.delta.x * xSpeed);
                m_cam.transform.RotateAround(target.transform.position, m_cam.transform.right, -ped.delta.y * ySpeed);
            }
        }

    }
}