using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;
/// <summary>
/// DEPRECATED
/// </summary>
public class MapGenerator : MonoBehaviour // Cr�ation d'un objet salle pour simplification de la g�n�ation ?? 
{
    //CONDISTION 
    public int NombreRoomsCheminPricpal;
    public int NombreCheminAuxilaire;
    


   // VAR Traking n informations  
    GameObject[] Rooms;
    List<PtnConnexion> PtnCoDisponible;

    private int CheminPrincipal_nbRoom;

    // Var TAMPON
    List<GameObject> RoomDispo = new List<GameObject>() ;
    int LastIndexRoom;
    GameObject LastRoom;
    int indexLastPoint;


    public void Awake()
    {
        // Rerssource folder => alimentation de liste 
        Rooms = Resources.LoadAll<GameObject>("Map/Rooms") ;
        CheminPrincipal_nbRoom = 0;
        // liste des couloir VAR global 

        // Choisir une salle de d�part 
        FirstRoom();
    }

    // tools 
    private void AlimentationList(GameObject RoomOnBoard)
    {
        RoomDispo.Add(RoomOnBoard);
        int lol = 32;
        //foreach (Transform ptnCo in RoomForChild.GetComponentsInChildren<Transform>())
        //{
        //    PtnCoDisponible.Add(ptnCo.GetComponent<PtnConnexion>());
        //    RoomDispo.Add(ptnCo.GetComponent<PtnConnexion>());
        //}
    }

    private bool Emplacementcompatibilite(Emplacement lastRoom , Emplacement newRoom,Direction direction)
    {
        bool result = false;

        if (direction == Direction.Y) // Haut et bas 
        {
            if (newRoom == Emplacement.Haut)
            {
                if (lastRoom == Emplacement.Bas)
                {
                    result = true;
                }
            }
            if (newRoom == Emplacement.Bas)
            {
                if (lastRoom == Emplacement.Haut)
                {
                    result = true;
                }
            }
            
        }

        if (direction == Direction.X) // gauche droite
        {
            if (newRoom == Emplacement.Gauche)
            {
                if (lastRoom == Emplacement.Droite)
                {
                    result = true;
                }
            }
            if (newRoom == Emplacement.Droite)
            {
                if (lastRoom == Emplacement.Gauche)
                {
                    result = true;
                }
            }
        }


        return result ; 
    }

    // la g�n�ration
    public void FirstRoom()
    {
        int RandoomIndex = Random.Range(0, Rooms.Length);
        //Transform firstRoomForChild = Rooms[RandoomIndex].GetComponent<Transform>();
        GameObject firstRoom = Rooms[RandoomIndex];


        //AlimentationList(firstRoom);

        LastRoom = Instantiate(firstRoom, Vector2.zero,Quaternion.identity);
    }

    int limitateurDeBoucle = 10;
    int limitBoucle;
    public void GenerationContinu()
    {
        // clacos la boucle au bout de 1� essai de place un template 
        
        CheminPrincipal_nbRoom++;
        bool isSimilaire = false;
        int RandoomIndex;

        Transform NewRoomForChild = null;
        GameObject NewRoom = null;
        PtnConnexion PtnChoisi_newRoom = null;
        PtnConnexion PtnChoisi_lastRoom ; //  ptn de co de la lastroom
        int indexLastRoomChoisi = 0;
        
        // choisi point de connexion al�atoire de la DERNIERE ROOM 
        RandoomIndex = Random.Range(0, LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion.Length);
        PtnChoisi_lastRoom = LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[RandoomIndex].GetComponent<PtnConnexion>();
        indexLastRoomChoisi = RandoomIndex;
        Debug.Log(PtnChoisi_lastRoom.isDispo);
        if (PtnChoisi_lastRoom.isDispo == false)
        {
            bool flagIsDispo = false;
            List<int> listTampon = new List<int>() ; 
            for (int i = 0; i < LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion.Length; i++)
            {
                listTampon.Add(i);
            }
            while (flagIsDispo == false)
            {
                limitBoucle++;
                if (limitBoucle > limitateurDeBoucle)
                {
                    break;
                }
                RandoomIndex = Random.Range(0, listTampon.Count-1);
                PtnChoisi_lastRoom = LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[listTampon[RandoomIndex]].GetComponent<PtnConnexion>();
                if (PtnChoisi_lastRoom.isDispo == false)
                {
                    listTampon.RemoveAt(RandoomIndex);
                }
                else
                {
                    indexLastRoomChoisi = listTampon[RandoomIndex];
                    flagIsDispo = true;
                }
            }
        }
        

        while (isSimilaire == false)
        {
            limitBoucle++;
            /*if (limitBoucle > limitateurDeBoucle)
            {
                break;
            }*/
            //while (RandoomIndex == LastIndexRoom) // �vite une salle similaire 
            //{
            //    RandoomIndex = Random.Range(0, Rooms.Length);
            //}

            // choisi une nouvelle room dans la liste des room 
            RandoomIndex = Random.Range(0, Rooms.Length);//impossible
            NewRoomForChild = Rooms[RandoomIndex].GetComponent<Transform>();
            NewRoom = Rooms[RandoomIndex];
            for (int i = 0; i < NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion.Length; i++)
                NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[i].GetComponent<PtnConnexion>().isDispo = true;

            // set des var pour les test de compatibilit� 
            Direction Direction_lastRoom = PtnChoisi_lastRoom.DirectionPointDeConnexion; // x ou y 
           
            RandoomIndex = Random.Range(0, LastRoom.GetComponent<ManagerPTNConnexion>().EmplacementPtnConnexion.Length);
            Emplacement Emplacement_LastRoom = LastRoom.GetComponent<ManagerPTNConnexion>().EmplacementPtnConnexion[RandoomIndex];

            // nouvelle room check si Direction Similaire 
            // check si Emplacement opos� dispo si haut et bas / gauche et droite 
            for (int i = 0; i < NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion.Length; i++)
            {
                if (NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[i].GetComponent<PtnConnexion>().isDispo == true) 
                {
                    Direction Direction_newRoom = NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[i].GetComponent<PtnConnexion>().DirectionPointDeConnexion; // Y ou X 
                    if (Direction_newRoom == Direction_lastRoom) // direction similaire avec le point de connexion choisir de la last room 
                    {
                        Emplacement Emplacement_newRoom = NewRoom.GetComponent<ManagerPTNConnexion>().EmplacementPtnConnexion[i];
                        bool isCompatible = Emplacementcompatibilite(Emplacement_LastRoom, Emplacement_newRoom, Direction_newRoom);
                        if (isCompatible)
                        {
                            isSimilaire = true;
                            PtnChoisi_newRoom = NewRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[i].GetComponent<PtnConnexion>();
                            /*PtnChoisi_newRoom.isDispo = false;
                            PtnChoisi_lastRoom.isDispo = false;*/
                            limitateurDeBoucle = 0;
                            break;
                        }
                    }
                }
            }
        }

        // fin on alimente les trackeur de stat et on place la nouvelle room 
        //AlimentationList(NewRoom); // pour ptn dispo => rendre juste les room dispo existante 


        Vector3 lastRoom_ptnConnexion = PtnChoisi_lastRoom.transform.position;
        Vector3 newRoomPrnCo = PtnChoisi_newRoom.transform.position;
        
        Vector2 destination = lastRoom_ptnConnexion + PtnChoisi_newRoom.transform.localPosition;
        LastRoom = Instantiate(NewRoom,destination,Quaternion.identity);
        for(int i = 0; i < LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion.Length; i++)
            LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[i].GetComponent<PtnConnexion>().isDispo = true;
        LastRoom.GetComponent<ManagerPTNConnexion>().ListePointDeConnexion[indexLastRoomChoisi].GetComponent<PtnConnexion>().isDispo = false;
    }

    private void Start()
    {
        FirstRoom();
        NombreRoomsCheminPricpal = 5;
    }


    private void Update()
    {
        if (CheminPrincipal_nbRoom <= NombreRoomsCheminPricpal)
        {
            GenerationContinu();
        }
        
    }


    // G�n�ration 
    // Choisi une room al�atoire dans la liste sauf la derni�re utiis� => var global sur derni�re salle use => index de la liste comme ID 
    // Rotation de 90 � n fois Ou n neu peut pas d�pass� 3 car sinon cela fait juste un 360 a la position de d�part DONC NUL => �vit� les truc en diagonal pour � 90 => Range 0 a 3

    // ajout de la salle en fonction des point de connextion similaire ==> check la liste dans ManagerConnexion des 2 pi�cess met dans une liste les points similaires puis al�atoire => puis placement de la seconde ROOM en f() du point de connexion des 2 room 
    // int�gration dans un liste les points de connexion dispo (ne pas oublier de retirer de la listes les points de connexion utilis� pr�c�dament)

    //1
    // al�atoire sur les points de connexion de la salle ajout� 
    // Check de condition pour la finission du chemin vers la salle de bosse 
    // si OK Pose la salle du BOSS 
    // sinon on continue la g�n�ration 

    // 2 on peut check les points de connexion disponible => stocker dans une liste ++ Check condition sur le nombre de chemin auxiliaire menant null part 
    // Random sur le nombre de salle Auxiliare a ajout� 
    // Cration des salle et check Condition 


    // PROBLEME A REGLER 

    // PB sur une superpositon de salle qui ne sont pas align� sur la meme grille 
    // cela pourrais cr�a des chemin qui m�nerais vers d'autre salle du chemin principale mais non si la grille n'est pas align� correcttement 

    // Check si lors de placement de salle si la salle pos� ne va pas superpos� une aurte salle ou se connect� a une autre salle si on arrive a align� correctement la grille 

}
