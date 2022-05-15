using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField]
    private int m_type;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>object's type : 0 = wall, 1 = spikes, 2 = spikes controller, 3 = arrow, 4 = trap, 5 = spawner, 6 = chest</returns>
    public int GetObjType()
    {
        return m_type;
    }
}
