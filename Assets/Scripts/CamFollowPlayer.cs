using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Camera myCam ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myCam.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,myCam.transform.position.z); 
    }
}
