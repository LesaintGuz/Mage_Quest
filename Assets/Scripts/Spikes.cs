using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private float m_cooldown;
    private bool m_state = false;
    private timerCZ m_timer;

    [SerializeField]
    private Sprite m_notActivateSprite;
    [SerializeField]
    private Sprite m_activateSprite;
    
    /// <summary>
    /// Set Spikes caracteristics and initiate it's cooldown
    /// </summary>
    /// <param name="cooldown">amount of time between each spikes changement of state</param>
    public void SetSpikes(float cooldown)
    {
        m_timer = new timerCZ(cooldown);
        m_timer.m_isFinallyTheEnd += ChangeStateTime;
        m_timer.StartTimer();
    }

    /// <summary>
    /// Change the spikes state
    /// </summary>
    /// <param name="state">true if spikes get activated and can killed personnage else spikes get desactivated</param>
    public void ChangeState(bool state)
    {
        if (state)
        {
            m_state = true;
            GetComponent<SpriteRenderer>().sprite = m_activateSprite;
            m_timer.StopTimer();
        }
        else
        {
            m_state = false;
            GetComponent<SpriteRenderer>().sprite = m_notActivateSprite;
            m_timer.StartTimer();
        }
        GetComponent<Collider2D>().enabled = m_state;
    }

    /// <summary>
    /// Function called at the end of a cooldown
    /// Change spikes state and restart timer
    /// </summary>
    private void ChangeStateTime()
    {
        ChangeState(!m_state);
        m_timer.StartTimer();
    }

    /// <summary>
    /// Check if spikes are in collision with enemy or player if true kill them
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
            Destroy(collision.gameObject);
    }
}
