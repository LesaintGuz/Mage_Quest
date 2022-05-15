using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform m_position;
    // Start is called before the first frame update
    /// <summary>
    /// When init an enemy look for the player transform and set it as a target
    /// </summary>
    void Start()
    {
        GetComponentInParent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    /// <summary>
    /// Change the enemy rotation according to it's movement
    /// </summary>
    private void Update()
    {
        transform.eulerAngles = new Vector3 (m_position.rotation.eulerAngles.x, m_position.rotation.eulerAngles.y, 0);
    }
}
