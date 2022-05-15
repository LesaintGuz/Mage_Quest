using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private int m_power = 50;

    /// <summary>
    /// Set a potion power
    /// </summary>
    /// <param name="power">amount of life the player will get walking on the potion</param>
    public void SetPower(int power)
    {
        m_power = power;
    }

    /// <summary>
    /// Check if the potion is in collisoin with the player
    /// if true destroy the potion and add life to player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Personnage>().AddLife(m_power);
            Destroy(this.gameObject);
        }
            
    }
}
