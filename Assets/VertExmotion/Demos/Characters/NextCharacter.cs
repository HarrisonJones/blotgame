using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kalagaan
{
    public class NextCharacter : MonoBehaviour
    {

        [System.Serializable]
        public class Duo
        {
            public GameObject a;
            public GameObject b;
        }

        public List<Duo> m_duoLst = new List<Duo>();
        int current = 0;

        // Use this for initialization
        void Start()
        {

            for (int i = 0; i < m_duoLst.Count; ++i)
            {
                m_duoLst[i].a.SetActive(i == current);
                m_duoLst[i].b.SetActive(i == current);
            }

        }

        // Update is called once per frame
        void OnGUI()
        {

            bool change = false;
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Previous", GUILayout.Width(Screen.width / 2f), GUILayout.Height(Screen.height / 16f)))
            {
                current = current == 0 ? m_duoLst.Count - 1 : current - 1;
                change = true;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Next", GUILayout.Width(Screen.width / 2f), GUILayout.Height(Screen.height / 16f)))
            {
                current++;
                current = current % m_duoLst.Count;
                change = true;
            }

            GUILayout.EndHorizontal();


            if (change)
                for (int i = 0; i < m_duoLst.Count; ++i)
                {
                    m_duoLst[i].a.SetActive(i == current);
                    m_duoLst[i].b.SetActive(i == current);
                }

        }
    }
}