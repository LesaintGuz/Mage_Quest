using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;

public class Bullet : MonoBehaviour
{
    private string m_type;
    private float m_speed;
    private int m_damage;
    private int m_bulletPierce;
    private Elements m_element;
    private bool m_isEnvironmental;

    private timerCZ m_timer;
    private timerCZ m_lifetime;
    Quaternion m_rotationToTarget;
    Transform m_playerTransform;
    Transform m_target = null;

    /// <summary>
    /// init bullet behaviour
    /// </summary>
    void Start()
    {
        if(m_type == "aoe" || m_type == "turnAround")
        {
            if(m_type == "aoe")
            {
                //set damage zone around the player
                float x = Random.Range(-5.0f, 5.0f);
                float y = Random.Range(-5.0f, 5.0f);
                transform.position += new Vector3(x, y, 0);
            }
            //aoe and turn around doesn't destroy on collision with enemy, they disapear at the end of tehri lifetime
            m_playerTransform = FindObjectOfType<PlayerBehavior>().transform;
            m_lifetime = new timerCZ(m_bulletPierce * 1.5f);
            m_lifetime.StartTimer();
            return;
        }
        else if (m_type == "back")
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }
        else if(m_type == "left")
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
        else if (m_type == "right")
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }else if(m_type == "face")
        {
            //bullet go straight in the direction they are looking at, so no needs to change anything for facing ones
        }
        else if(m_type == "boomerang")
        {
            //at the end of a short timer, boomerang bullet go back in the other direction
            m_timer = new timerCZ(0.35f);
            m_rotationToTarget = transform.rotation;
            m_timer.m_isFinallyTheEnd += ChangeBoomerangDirection;
            m_timer.StartTimer();
        }
        else if(m_type == "autoAim")
        {
            //autoAim bullet look for an enemy position if there is no enemy, they are destroyed
            m_target = FindObjectOfType<EnemyMovement>().transform;
            if(m_target != null)
            {
                m_rotationToTarget = m_target.transform.rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_rotationToTarget, 180);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
        }
        //every bullet are destroyed when there lifetime ends
        m_lifetime = new timerCZ(5f);
        m_lifetime.StartTimer();
    }

    /// <summary>
    /// Set the characteristic of a bullet
    /// </summary>
    /// <param name="type"> way the bullet move, must be [aoe] [turnAround] [back] [left] [right] [face] [boomerang] [autoAim]</param>
    /// <param name="speed"> speed of the bullet</param>
    /// <param name="damage"> damage done by the bullet on collision with one enemy</param>
    /// <param name="bulletPierce"> number of enemy a bullet can go through</param>
    /// <param name="element"> element of the bullet</param>
    /// <param name="isEnvironmental">true if the bullet is spawn by an enemy or the environment</param>
    public void SetBullet(string type, float speed, int damage, int bulletPierce, Elements element, bool isEnvironmental = false)
    {
        m_type = type;
        m_speed = speed;
        m_damage = damage;
        m_bulletPierce = bulletPierce;
        m_element = element;
        m_isEnvironmental = isEnvironmental;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_lifetime.End())
            Destroy(this.gameObject);

        if(m_type == "aoe")
        {
            return;
        }
        else if (m_type == "turnAround")
        {
            //bullet turn around player position
            transform.RotateAround(m_playerTransform.localPosition, new Vector3 (0,0,1), m_speed *25 * Time.deltaTime);
        }
        else if (m_type == "autoAim")
        {
            //if the bullet has no more target, it will search for another one
            if(m_target == null)
            {
                m_target = FindObjectOfType<EnemyMovement>().transform;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_rotationToTarget, 180);
            transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime * m_speed);
        }else
        {      
            transform.position += new Vector3(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180.0f), -Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180.0f), 0) * m_speed * 2 * Time.deltaTime;
        }
        
    }

    /// <summary>
    /// Used by boomerang bullet to change their direction at the end of a timer
    /// </summary>
    private void ChangeBoomerangDirection()
    {
        Vector3 angle = transform.rotation.eulerAngles;
        angle.z -= 180;
        m_rotationToTarget = Quaternion.Euler(angle);
        transform.rotation = m_rotationToTarget;
        m_timer.StopTimer();//bullet only change direction once
    }

    /// <summary>
    /// Check the objects who collide with the bullet if it's an enemy or the player if it's an environmental bullet, it will deal damage
    /// if the bullet collide with a wall, the bullet is destroyed
    /// after each collision bullet loose 1 pierce, if it's = 0, then the bullet is destroyed
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || (collision.gameObject.tag == "Player" && m_isEnvironmental))
        {
            collision.gameObject.GetComponent<Personnage>().TakeDamage(m_damage, m_element);
            if(m_type != "aoe" || m_type != "turnAround")
                m_bulletPierce--;
        }else if(collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (m_bulletPierce <= 0)
            Destroy(this.gameObject);
    }
}
