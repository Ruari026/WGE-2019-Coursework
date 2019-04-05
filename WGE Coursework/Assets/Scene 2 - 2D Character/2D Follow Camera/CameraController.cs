using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Object To Follow")]
    public GameObject followTarget;

    [Header("Following On The X Axis")]
    public float followSpeedX;
    public float xRange;

    [Header("Following Target On The y Axis")]
    public float followSpeedY;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowTarget();
	}


    /*
    ========================================================================================================================================================================================================
    Regular Camera Movement
    ========================================================================================================================================================================================================
    */
    private void FollowTarget()
    {
        Vector3 newPosition = this.transform.position;

        //Following the Target On The X Axis
        float xDifference = this.transform.position.x - followTarget.transform.position.x;
        if (xDifference < 0)
        {
            xDifference *= -1;
        }

        if (xDifference > xRange)
        {
            newPosition.x = followTarget.transform.position.x;
        }

        //Following the Target On The Y Axis
        newPosition.y = followTarget.transform.position.y;

        //Lerping The Camera To The Target
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeedY * Time.deltaTime);
    }


    /*
    ========================================================================================================================================================================================================
    Manually Moving Camera
    ========================================================================================================================================================================================================
    */
    private void LerpCameraToPosition(Vector3 newPosition)
    {
        StartCoroutine(LerpPosition(this.transform.position, newPosition, 1));
    }

    IEnumerator LerpPosition(Vector3 currentPosition, Vector3 newPosition, float maxTime)
    {
        float t = 0;

        while (t < 1)
        {
            t += (Time.deltaTime / maxTime);

            this.transform.position = Vector3.Lerp(currentPosition, newPosition, t);

            if (t >= 1)
            {
                this.transform.position = newPosition;
            }

            yield return null;
        }
    }


    /*
    ========================================================================================================================================================================================================
    Controlling The Camera Target
    ========================================================================================================================================================================================================
    */
    private void SetNewFollowTarget(GameObject newTarget)
    {
        this.followTarget = newTarget;
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
