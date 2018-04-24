using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kalagaan
{
    public class ScaleAndRotationFX : MonoBehaviour
    {
        public VertExmotionSensor m_sensor;

        public void SetScaleX( Slider s )
        {
            if (m_sensor == null)
                return;
            Vector3 v = m_sensor.m_params.scale;
            v.x = s.value;
            m_sensor.m_params.scale = v;
        }


        public void SetScaleZ(Slider s)
        {
            if (m_sensor == null)
                return;
            Vector3 v = m_sensor.m_params.scale;
            v.z = s.value;
            m_sensor.m_params.scale = v;
        }


        public void SetRotationAngle(Slider s)
        {
            if (m_sensor == null)
                return;
            m_sensor.m_params.rotation.axis = Vector3.up;
            m_sensor.m_params.rotation.angle = s.value;
        }
    }

}