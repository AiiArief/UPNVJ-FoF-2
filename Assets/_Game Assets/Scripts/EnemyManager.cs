using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Player m_playerTarget;

    [SerializeField] float m_timerRespawn = 1.0f;
    [SerializeField] Enemy m_enemyPrefab;
    [SerializeField] Transform m_enemiesParent;
    [SerializeField] Transform[] m_spawners;

    private void OnEnable()
    {
        StartCoroutine(SpawnerEnumerator());
    }

    private IEnumerator SpawnerEnumerator()
    {
        while(m_playerTarget && !m_playerTarget.isDead)
        {
            yield return new WaitForSeconds(m_timerRespawn);

            int rndSpawnId = Random.Range(0, m_spawners.Length);
            var enemy = Instantiate(m_enemyPrefab, m_enemiesParent, true);
            enemy.playerTarget = m_playerTarget;
            enemy.transform.position = m_spawners[rndSpawnId].transform.position;
        }
    }
}
