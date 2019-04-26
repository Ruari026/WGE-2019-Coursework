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

    [Header("Zooming Camera")]
    public float zoomedInSize = 3.5f;
    public float zoomedOutSize = 7;
    public float zoomTime = 1;

    [Header("Camera Shake")]
    public float maxShakeTime = 1;
    private float currentShakeTime = 0;
    public float shakeMagnitude = 1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowPlayerCharacter();

        HandleCameraShake();
	}

    void OnEnable()
    {
        ResetPlayerPosition._playerRespawn += RespawnCamera;

        NpcDialogueHandler.OnEventDialogueStart += ZoomCameraIn;
        NpcDialogueHandler.OnEventDialogueEnd += ZoomCameraOut;
    }

    void OnDisable()
    {
        ResetPlayerPosition._playerRespawn -= RespawnCamera;

        NpcDialogueHandler.OnEventDialogueStart -= ZoomCameraIn;
        NpcDialogueHandler.OnEventDialogueEnd -= ZoomCameraOut;
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
    Camera Shake
    ========================================================================================================================================================================================================
    */
    private void HandleCameraShake()
    {
        if (GetPlayerState() == MovementState.IN_AIR)
        {
            currentShakeTime += Time.deltaTime;

            if (currentShakeTime > maxShakeTime)
            {
                currentShakeTime = maxShakeTime;
            }
        }

        if (GetPlayerState() == MovementState.ON_GROUND)
        {
            if (currentShakeTime > 0)
            {
                StartCoroutine(CameraShake());
            }
        }
    }

    private IEnumerator CameraShake()
    {
        float t = 0;

        while (currentShakeTime > 0)
        {
            currentShakeTime -= Time.deltaTime;
            t += Time.deltaTime;

            Vector3 shakePos = new Vector3(Mathf.PerlinNoise(t, 0), Mathf.PerlinNoise(0, t), -5);
            shakePos.x = ((shakePos.x * 2) - 1) * shakeMagnitude;
            shakePos.y = ((shakePos.y * 2) - 1) * shakeMagnitude;

            Camera.main.transform.localPosition = shakePos;

            if (currentShakeTime < 0)
            {
                currentShakeTime = 0;
                Camera.main.transform.localPosition = new Vector3(0, 0, -5);
            }

            yield return null;
        }
    }

    /*
    ========================================================================================================================================================================================================
    Dialogue Camera Movement
    ========================================================================================================================================================================================================
    */
    public void ZoomCameraIn()
    {
        //Zooming Camera Size
        StartCoroutine(ZoomCameraSize(zoomedOutSize, zoomedInSize));

        //Moving Camera To Between The Player & NPC
        Vector3 distanceBetweenActors = playerCharacter.transform.position - otherTargetCharacter.transform.position;
        StartCoroutine(LerpToPosition(this.transform.position, playerCharacter.transform.position - (distanceBetweenActors / 2)));
    }

    public void ZoomCameraOut()
    {
        //Zooming Camera Size
        StartCoroutine(ZoomCameraSize(zoomedInSize, zoomedOutSize));
    }

    private IEnumerator ZoomCameraSize(float startSize, float endSize)
    {
        float t = 0;
        while (t < 1)
        {
            //Getting lerp step position
            t += (Time.deltaTime / zoomTime);
            float i = CalculateStep(t, "SmoothStep");

            //Setting camera orthographic size
            Camera.main.orthographicSize = Mathf.Lerp(startSize, endSize, i);

            //Ending the lerp
            if (t >= 1)
            {
                Camera.main.orthographicSize = endSize;
            }

            yield return null;
        }
    }

    private IEnumerator LerpToPosition(Vector3 startPos, Vector3 endPos)
    {
        float t = 0;
        while (t < 1)
        {
            //Getting lerp step position
            t += (Time.deltaTime / zoomTime);
            float i = CalculateStep(t, "SmoothStep");

            //Setting camera orthographic size
            this.transform.position = Vector3.Lerp(startPos, endPos, i);

            //Ending the lerp
            if (t >= 1)
            {
                this.transform.position = endPos;
            }

            yield return null;
        }
    }

    private float CalculateStep(float t, string zoomType)
    {
        float i = t;

        if (zoomType == "Linear")
        {
            i = t;
        }
        else if (zoomType == "SmoothStart")
        {
            i = (i * i);
        }
        else if (zoomType == "SmoothEnd")
        {
            i = (1 - (1 - i) * (1 - i));
        }
        else if (zoomType == "SmoothStep")
        {
            float sStart = (i * i);
            float sEnd = (1 - (1 - i) * (1 - i));

            i = Mathf.Lerp(sStart, sEnd, t);
        }

        return i;
    }

    /*
    ========================================================================================================================================================================================================
    Resetting The Camera
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
