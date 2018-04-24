using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class JellyfishSpawn : MonoBehaviour {

	public GameObject m_jellyfishPrefab;
	public Transform m_spawnRoot;
	public Transform[] m_spawnPoints;

	public float m_spawnRate = 1f;
	public int m_maxJelly = 20;
	public float m_destroyYLimit = 10f;
	float m_lastSpawnTime = 0f;

	public List<GameObject> m_jellySpawned = new List<GameObject>();

	void Update () 
	{
		if( Time.time - m_lastSpawnTime > m_spawnRate && m_jellySpawned.Count < m_maxJelly )
		{
			Vector3 point = m_spawnPoints[0].position;
			for( int i=1; i<m_spawnPoints.Length; ++i )
				point = Vector3.Lerp( point, m_spawnPoints[i].position, Random.value );

			GameObject go = Instantiate( m_jellyfishPrefab, point, Quaternion.identity ) as GameObject;


			//check for nearest jellyfish
			for (int i=0; i< m_jellySpawned.Count; ++i)
			{
				if( Vector3.Distance( go.transform.position, m_jellySpawned[i].transform.position ) < 2f )
					go.transform.position = ( go.transform.position - m_jellySpawned[i].transform.position ).normalized * 2f + m_jellySpawned[i].transform.position;
			}


			m_jellySpawned.Add( go );

			go.transform.parent = m_spawnRoot;
			//go.transform.localScale *= Random.Range(.1f, 1.2f);



	
			m_lastSpawnTime = Time.time;
		}

		int idToDestroy = -1;
		for (int i=0; i< m_jellySpawned.Count; ++i)
			if (m_jellySpawned [i].transform.position.y > m_destroyYLimit )
				idToDestroy = i;

		if( idToDestroy != -1 )
		{
			Destroy(m_jellySpawned [idToDestroy]);
			m_jellySpawned.RemoveAt(idToDestroy);
		}
	}


	void OnDrawGizmos()
	{
		
		for( int i=0; i<m_spawnPoints.Length; ++i )
			Gizmos.DrawSphere (m_spawnPoints[i].position, .1f);

	}
}
