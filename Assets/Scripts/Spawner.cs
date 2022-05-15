using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;

public class Spawner : MonoBehaviour
{
    private timerCZ m_timer;

    [SerializeField]
    private int m_level;
    private float m_speed;

    [SerializeField]
    private Elements m_element;

    [SerializeField]
    private GameObject m_enemy;

    [SerializeField]
    private int m_enemyChara;

    private bool m_isActive = false;
 

    // Update is called once per frame
    /// <summary>
    /// if player is near from the spawner it gets activated else it gets unactivated
    /// </summary>
    void Update()
    {
        if(!m_isActive && Vector3.Distance(FindObjectOfType<PlayerBehavior>().transform.position, transform.position) < 6)
        {
            m_timer.StartTimer();
            m_isActive = true;
        }else if(m_isActive && Vector3.Distance(FindObjectOfType<PlayerBehavior>().transform.position, transform.position) > 6)
        {
            m_timer.StopTimer();
            m_isActive = false;
        }
    }

    /// <summary>
    /// Set caracteristics of teh spawner and enemies spawned
    /// </summary>
    /// <param name="level"></param>
    /// <param name="speed">move speed</param>
    /// <param name="cooldown">amount of time between each enemy spawned</param>
    /// <param name="anElement">element of the enemy to spawn</param>
    public void SetCara(int level, float speed, float cooldown, Elements anElement = Elements.Void)
    {
        m_level = level;
        m_element = anElement;
        m_speed = speed;
        m_timer = new timerCZ(cooldown);
        m_timer.m_isFinallyTheEnd += Spawn;
        if(m_isActive)
            m_timer.StartTimer();
    }

    /// <summary>
    /// Function called at the end of a timer
    /// Spawn an enemy
    /// the enemy levels is = spawner level +- 1
    /// restart the timer when enemy is spawned
    /// </summary>
    public void Spawn()
    {
        int enemyLevel = Random.Range(0, 100);
        if (enemyLevel < 5)
        {
            enemyLevel = -1;
        }
        else if (enemyLevel < 95)
        {
            enemyLevel = 0;
        }
        else
        {
            enemyLevel = 1;
        }
        enemyLevel += m_level;
        int life = Random.Range(150, 300) + 100 * enemyLevel;
        //we needs to have global coordinate T_T
        GameObject enemy = Instantiate(m_enemy,transform.position,Quaternion.identity);
        enemy.GetComponent<Personnage>().SetChara(m_speed, enemyLevel, life, m_element, enemyLevel);
        if(m_isActive)
            m_timer.StartTimer();
    }
}
