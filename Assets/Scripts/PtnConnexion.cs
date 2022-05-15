using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;


/* Script permettant de donné l'information sur la direction des point de connexion qui sont le entré et sortie des salles*/
public class PtnConnexion : MonoBehaviour
{

    public bool isDispo = true;

    public Direction DirectionPointDeConnexion;

    public Transform PTN_transform;
    public Vector3 PTN_position;
    public Quaternion Ptn_rotation;

    private void Start()
    {
        PTN_transform = gameObject.transform;
        PTN_position = gameObject.transform.position;
        Ptn_rotation = gameObject.transform.rotation;
    }
}
