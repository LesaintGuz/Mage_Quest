using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesActivator : MonoBehaviour
{
    [SerializeField]
    private List<Spikes> m_spikes;

    /// <summary>
    /// Set the list of the spikes controlled by the activator
    /// </summary>
    /// <param name="spikes"></param>
    public void SetSpikes(List<Spikes> spikes)
    {
        m_spikes = spikes;
    }

    /// <summary>
    /// When an enemy or the player walk by the activator, the spikes get activated
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            for(int i = 0; i < m_spikes.Count; i++)
                m_spikes[i].ChangeState(true);
        }
            
    }

    /// <summary>
    /// When an enemy or the player walk off the activator
    /// spikes controlled by it are activated
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < m_spikes.Count; i++)
                m_spikes[i].ChangeState(false);
        }
    }

    /// <summary>
    /// return all spikes controlled by the activator
    /// </summary>
    /// <returns></returns>
    public int GetNbSpikesControlled()
    {
        int result = 0;
        for(int i =0; i < m_spikes.Count; i++)
        {
            if (m_spikes[i].isActiveAndEnabled)
                result++;
        }
        return result;
    } 
}
