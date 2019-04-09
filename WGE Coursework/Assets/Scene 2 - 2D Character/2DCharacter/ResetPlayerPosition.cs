using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    //Position For The Player To Reset To
    public Transform resetTransform;

    
    //Personal Addition
    public delegate void PlayerRespawn();
    public static event PlayerRespawn _playerRespawn;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.transform.position = resetTransform.position;

            _playerRespawn();
        }
    }
}
