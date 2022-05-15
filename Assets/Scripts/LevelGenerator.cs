using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_roomType;

    [SerializeField]
    private int m_roomAmountX;

    [SerializeField]
    private int m_roomAmountY;

    [SerializeField]
    private int m_roomSize;

    [SerializeField]
    private int m_level;

    private List<RoomGenerator> m_rooms;

    // Start is called before the first frame update
    void Start()
    {
        m_rooms = new List<RoomGenerator>();
        Vector3 roomPos;
        int roomTypeIndex;
        int exitID;
        List<int> connexionsID;
        bool idValid = false;
        int currentRoomID = 0;
        int moreConnexions;
        List<int> roomIDs = new List<int>();
        int randomRoomID;
        //create a map
        for (int i = 0; i < m_roomAmountY; i++)
        {
            for (int j = 0; j < m_roomAmountX; j++)
            {
                roomPos = new Vector3(j * m_roomSize, i * m_roomSize, 0);
                roomTypeIndex = Random.Range(0, m_roomType.Count - 1);
                m_rooms.Add(Instantiate(m_roomType[roomTypeIndex], roomPos, transform.rotation).GetComponent<RoomGenerator>());
                m_rooms[m_rooms.Count - 1].GenerateRoom(m_level);
                if (i == 0)
                {
                    m_rooms[m_rooms.Count - 1].SetConnexionAvailability(2, false, true);
                }
                else if (i == m_roomAmountY - 1)
                    m_rooms[m_rooms.Count - 1].SetConnexionAvailability(0, false, true);
                if (j == 0)
                {
                    m_rooms[m_rooms.Count - 1].SetConnexionAvailability(3, false, true);
                } else if (j == m_roomAmountX - 1)
                    m_rooms[m_rooms.Count - 1].SetConnexionAvailability(1, false, true);
            }
        }

        //set up the amount of room untill the exit is joined exit
        exitID = Random.Range(m_roomAmountY * m_roomAmountX / 5, m_roomAmountY * m_roomAmountX / 3);

        //create a path between rooms to join the exit
        //each room have 4 exit/entry, that can be true or false
        for (int i = 0; i < exitID; i++)
        {
            connexionsID = new List<int> { 0, 1, 2, 3 };
            idValid = false;
            while (!idValid)
            {
                if (connexionsID.Count == 0)
                    idValid = true;
                idValid = CheckIfConnexionIsOk(ref currentRoomID, ref connexionsID);
            }
            

        }

        //activate exit
        m_rooms[currentRoomID].SetUpExit();

        //generate more connexions
        moreConnexions = Random.Range(m_roomAmountX * m_roomAmountY / 6, m_roomAmountX * m_roomAmountY / 4);
        int protection = 0;
        for (int i = 0; i < m_roomAmountY * m_roomAmountX; i++)
            roomIDs.Add(i);
        for(int i = 0; i < moreConnexions; i++)
        {
            randomRoomID = Random.Range(0, m_roomAmountX * m_roomAmountY - 1);
            currentRoomID = roomIDs[randomRoomID];
            connexionsID = new List<int> { 0, 1, 2, 3 };
            idValid = false;
            while (!idValid && protection < 15)
            {
                protection++;
                if (connexionsID.Count == 0)
                {
                    roomIDs.RemoveAt(randomRoomID);
                    randomRoomID = Random.Range(0, m_roomAmountX * m_roomAmountY - 1);
                    currentRoomID = roomIDs[randomRoomID];
                    connexionsID = new List<int> { 0, 1, 2, 3 };
                }
                    
                idValid = CheckIfConnexionIsOk(ref currentRoomID, ref connexionsID);

            }
        }

        //set up ai path
        AstarPath scanner = FindObjectOfType<AstarPath>();
        scanner.Scan();
    }

    /// <summary>
    /// Select a random conenxion point and check if is ok to est a connexion between 2 room there
    /// </summary>
    /// <param name="currentRoomID"></param>
    /// <param name="connexionsID">connexion ids are 0 = north, 1 = east, 2 = south, 3 = west</param>
    /// <returns></returns>
    private bool CheckIfConnexionIsOk(ref int currentRoomID, ref List<int> connexionsID)
    {
        bool idValid;
        int selectedConnexionID = Random.Range(0, connexionsID.Count - 1);
        if (m_rooms[currentRoomID].GetConnexionAvailability(connexionsID[selectedConnexionID]))
        {
            if (connexionsID[selectedConnexionID] == 0)
            {
                m_rooms[currentRoomID].SetConnexionAvailability(0, false);
                currentRoomID += m_roomAmountX;
                m_rooms[currentRoomID].SetConnexionAvailability(2, false);
            }
            else if (connexionsID[selectedConnexionID] == 1)
            {
                m_rooms[currentRoomID].SetConnexionAvailability(1, false);
                currentRoomID++;
                m_rooms[currentRoomID].SetConnexionAvailability(3, false);
            }
            else if (connexionsID[selectedConnexionID] == 2)
            {
                m_rooms[currentRoomID].SetConnexionAvailability(2, false);
                currentRoomID -= m_roomAmountX;
                m_rooms[currentRoomID].SetConnexionAvailability(0, false);
            }
            else if (connexionsID[selectedConnexionID] == 3)
            {
                m_rooms[currentRoomID].SetConnexionAvailability(3, false);
                currentRoomID--;
                m_rooms[currentRoomID].SetConnexionAvailability(1, false);
            }
            idValid = true;
        }
        else
        {
            connexionsID.RemoveAt(selectedConnexionID);
            idValid = false;
        }
        return idValid;
    }
}
