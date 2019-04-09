using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Target Objects")]
    public GameObject playerCharacter;
    public GameObject otherTargetCharacter;

    [Header("Following On The X Axis")]
    public float followSpeedX;
    public float xRange;

    [Header("Following Target On The y Axis")]
    public float followSpeedY;
    private float cameraYPos;

    [Header("Player Respawn Position")]
    public Transform playerRespawnPosition;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowPlayerCharacter();
	}

    void OnEnable()
    {
        ResetPlayerPosition._playerRespawn += RespawnCamera;
    }

    void OnDisable()
    {
        ResetPlayerPosition._playerRespawn -= RespawnCamera;
    }


    /*
    ========================================================================================================================================================================================================
    Regular Camera Movement
    ========================================================================================================================================================================================================
    */
    private void FollowPlayerCharacter()
    {
        if (GetPlayerState() != MovementState.DISABLED)
        {
            Vector3 newPosition = this.transform.position;

            //Getting Player Information
            if (GetPlayerState() == MovementState.ON_GROUND)
            {
                cameraYPos = playerCharacter.transform.position.y;
            }
            //Following the Target On The Y Axis
            newPosition.y = cameraYPos;

            //Following the Target On The X Axis
            float xDifference = this.transform.position.x - playerCharacter.transform.position.x;
            if (xDifference < 0)
            {
                xDifference *= -1;
            }

            if (xDifference > xRange)
            {
                newPosition.x = playerCharacter.transform.position.x;
            }

            //Lerping The Camera To The Target
            this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeedY * Time.deltaTime);
        }
    }
    /*
    ========================================================================================================================================================================================================
    Getting Player Information
    ========================================================================================================================================================================================================
    */
    public void RespawnCamera()
    {
        this.transform.position = playerRespawnPosition.position;
    }


    /*
    ========================================================================================================================================================================================================
    Getting Player Information
    ========================================================================================================================================================================================================
    */
    private MovementState GetPlayerState()
    {
        return playerCharacter.GetComponent<PlayerMovement2D>()._mState;
    }


    /*
    ========================================================================================================================================================================================================
    Debugging
    ========================================================================================================================================================================================================
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        //Drawing Camera Range Base Line
        Vector3 startBase = new Vector3((this.transform.position.x - xRange), (this.transform.position.y - 0.5f), this.transform.position.z);
        Vector3 endBase = new Vector3((this.transform.position.x + xRange), (this.transform.position.y - 0.5f), this.transform.position.z);
        Gizmos.DrawLine(startBase, endBase);

        //Drawing Camera Range Right Edge Line
        Vector3 startRight = new Vector3((this.transform.position.x + xRange), (this.transform.position.y - 0.5f + (xRange * 2)), this.transform.position.z);
        Vector3 endRight = new Vector3((this.transform.position.x + xRange), (this.transform.position.y - 0.5f), this.transform.position.z);
        Gizmos.DrawLine(startRight, endRight);

        //Drawing Camera Range Left Edge Line
        Vector3 startLeft = new Vector3((this.transform.position.x - xRange), (this.transform.position.y - 0.5f + (xRange * 2)), this.transform.position.z);
        Vector3 endLeft = new Vector3((this.transform.position.x - xRange), (this.transform.position.y - 0.5f), this.transform.position.z);
        Gizmos.DrawLine(startLeft, endLeft);
    }
}
