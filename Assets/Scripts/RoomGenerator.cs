using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    private int m_lvl;
    [SerializeField]
    private float[] m_levelPowerValue = { 50, 60, 75, 85, 110 };
    [SerializeField]
    private List<GameObject> m_objects;
    [SerializeField]
    private List<GameObject> m_connexionsObj;

    [SerializeField]
    private GameObject m_exit;

    [SerializeField]
    private List<bool> m_connexion;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateRoom();
    }

    /// <summary>
    /// /// Generate a room using 2 parameters
    /// m_lvl = power of the room, the amount of enemies and traps the room can handle
    /// m_objects = list of all the different objects a room can handle
    /// (they are pre selected and pre positioned for each room in order to controll the quality of each room generated)
    /// </summary>
    /// <param name="lvl"></param>
    public void GenerateRoom(int lvl)
    {
        m_lvl = lvl;
        float roomValue = m_levelPowerValue[m_lvl];
        int objID;
        //continue to add element to the room until there is no more points
        while(roomValue > 0)
        {
            if (m_objects.Count == 0)
                break;
            objID = Random.Range(0, m_objects.Count-1);
            m_objects[objID].SetActive(true);
            roomValue -= setObjCara(m_objects[objID].GetComponent<RoomObject>().GetObjType(), objID);
            m_objects.RemoveAt(objID);
        }

        //set up room connexions
        //0 = north, 1 = east, 2 = south, 3 = west
        m_connexion = new List<bool>();
        for(int i = 0; i < 4; i++)
        {
            m_connexion.Add(true);
        }
    }

    /// <summary>
    /// When an object is activated in the room, it's caracteristics must be initiate
    /// </summary>
    /// <param name="objType">type of the object to activate, 0 = wall, 1 = spikes, 2 = spikes controller, 3 = arrow, 4 = trap, 5 = spawner, 6 = chest</param>
    /// <param name="objID">id of the object to set in the list</param>
    /// <returns>power of the object must be retrieve from the amount of point a room have</returns>
    private float setObjCara(int objType, int objID)
    {
        float returnValue = 0;
        if(objType == 0)
        {
            returnValue = 10;
        }else if (objType == 1)
        {
            returnValue = 4 - (0.5f * m_lvl);
            m_objects[objID].GetComponent<Spikes>().SetSpikes(returnValue);
            returnValue = 15f - (5-m_lvl)*2;
        }
        else if (objType == 2)
        {
            //m_objects[objID].GetComponent<SpikesActivator>().SetSpikes(GetAllActivatedSpikes());
            returnValue = 5 * m_objects[objID].GetComponent<SpikesActivator>().GetNbSpikesControlled();
            if (returnValue == 0)
            {
                m_objects[objID].SetActive(false);
            }
        }
        else if (objType == 3)
        {
            float castSpeed = 2 - 0.3f * m_lvl;
            int damage = Random.Range(5, 10) * m_lvl;
            int projSpeed = Random.Range(1, 4) * (m_lvl +1) / 2;
            int scale = Random.Range(1, m_lvl);
            m_objects[objID].GetComponent<spells>().SetEnvironmentalSpell(castSpeed, damage, projSpeed, scale);
            returnValue = (10 - castSpeed * 2) + damage/4 + projSpeed + scale;
        }
        else if (objType == 4)
        {
            float speed = 4 - m_lvl / 2.0f; 
            m_objects[objID].GetComponent<MovingTrap>().SetCara(speed, m_lvl/3.0f);
            returnValue = 15 + m_lvl * 5;
        }
        else if (objType == 5)
        {
            float speed = Random.Range(0.25f, 0.65f) + m_lvl/2.0f;
            float cooldown = Random.Range(2.7f, 4.5f) - m_lvl/2.0f;
            m_objects[objID].GetComponent<Spawner>().SetCara(m_lvl, speed, cooldown, Elements.Radiation);
            returnValue = speed * 2 + (10 - cooldown)*2 + m_lvl;
        }
        else
        {
            //chest add a value < 0, it means that when a chest is activated,
            //a room can handle more dangers
            return -5 * m_lvl;
        }
        return returnValue;
    }

    /// <summary>
    /// Get All the activated spikes in order to link them to a spike activator
    /// </summary>
    /// <returns></returns>
    private List<Spikes> GetAllActivatedSpikes()
    {
        List<Spikes> spikes = new List<Spikes>();
        for(int i = 0; i < m_objects.Count; i++)
        {
            if (m_objects[i].GetComponent<RoomObject>().GetObjType() == 2)
                spikes.Add(m_objects[i].GetComponent<Spikes>());
        }
        return spikes;
    }

    /// <summary>
    /// set if a connexion must be active in this room
    /// connexion ids are 0 = north, 1 = east, 2 = south, 3 = west
    /// </summary>
    /// <param name="connexionID"></param>
    /// <param name="status"></param>
    /// <param name="keepWall"></param>
    public void SetConnexionAvailability(int connexionID, bool status, bool keepWall = false)
    {
        m_connexion[connexionID] = status;
        if (m_connexionsObj[connexionID].activeSelf == true && !keepWall)
            m_connexionsObj[connexionID].SetActive(false);
    }

    public bool GetConnexionAvailability(int connexionID)
    {
        return m_connexion[connexionID];
    }

    /// <summary>
    /// Activate exit gameObject on this room
    /// </summary>
    public void SetUpExit()
    {
        m_exit.SetActive(true);
    }
}
