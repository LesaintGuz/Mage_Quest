using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField]
    private float m_max_x;
    [SerializeField]
    private float m_min_x;
    [SerializeField]
    private bool m_isOnX = false;
    private bool m_isReturning = false;

    [SerializeField]
    private float m_speed;

    private Vector3 m_target;

    private bool m_isActive = false;

    private Vector3 m_startPosition;

    public void Start()
    {
        if (m_isOnX)
        {
            m_max_x += transform.localPosition.x;
            m_min_x += transform.localPosition.x;
        }
        else
        {
            m_max_x += transform.localPosition.y;
            m_min_x += transform.localPosition.y;
        }
    }

    /// <summary>
    /// Set the caracteristics of a moving trap
    /// must be call when the room is created
    /// </summary>
    /// <param name="speed">movespeed of the trap</param>
    /// <param name="lvlDelta">offset added to the trap's max and min position</param>
    public void SetCara(float speed, float lvlDelta)
    {
        m_speed = speed;
        m_min_x -= lvlDelta;
        m_max_x += lvlDelta;
        //trap can move on x or y axis
        if (m_isOnX)
        {
            m_target = new Vector3(m_max_x, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            m_target = new Vector3(transform.localPosition.x, m_max_x, transform.localPosition.z);
        }
        m_isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        float pos;
        //trap can be inactive
        if (!m_isActive)
            return;

        //change trap target (direction) according to it's position
        if (m_isOnX)
        {
            pos = transform.localPosition.x;
        }
        else
        {
            pos = transform.localPosition.y;
        }
        if(m_isReturning && pos < m_min_x + 0.1f)
        {
            if (m_isOnX)
            {
                m_target = new Vector3(m_max_x, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                m_target = new Vector3(transform.localPosition.x, m_max_x, transform.localPosition.z);
            }
            m_isReturning = false;
        }
        else if(!m_isReturning && pos > m_max_x - 0.1f)
        {
            if (m_isOnX)
            {
                m_target = new Vector3(m_min_x, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                m_target = new Vector3(transform.localPosition.x, m_min_x, transform.localPosition.z);
            }
            m_isReturning = true;
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_target, Time.deltaTime * m_speed);
        transform.Rotate(0, 0, 100 * Time.deltaTime * m_speed);
    }
}
