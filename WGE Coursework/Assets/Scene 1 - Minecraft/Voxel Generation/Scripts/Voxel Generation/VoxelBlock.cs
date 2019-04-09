using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelBlock : MonoBehaviour
{
    public delegate void EventBlockPickup(int blockType);
    public static event EventBlockPickup OnEventBlockPickup;

    private bool triggered = false;

    public float floatForce;

    public int blockType;

    private Rigidbody theRB;


    /*
    ======================================================================================================================================================
    initializing block
    ======================================================================================================================================================
    */
    public void CreateBlock(int newBlockType)
    {
        theRB = GetComponent<Rigidbody>();
        SetBlockType(newBlockType);

        string s = "Grass";
        switch (newBlockType)
        {
            case (1):
                {
                    s = "Grass";
                }
                break;

            case (2):
                {
                    s = "Dirt";
                }
                break;

            case (3):
                {
                    s = "Sand";
                }
                break;

            case (4):
                {
                    s = "Stone";
                }
                break;
        }

        VoxelGenerator vg = this.GetComponent<VoxelGenerator>();
        if (vg != null)
        {
            this.GetComponent<VoxelGenerator>().Initialise();
            this.GetComponent<VoxelGenerator>().CreateVoxel(0, 0, 0, s);
            this.GetComponent<VoxelGenerator>().UpdateMesh();
        }
    }

    private void SetBlockType(int newBlockType)
    {
        this.blockType = newBlockType;
    }


    // Update is called once per frame
    void Update ()
    {
        CheckForPlayerPickup();

        //Destroying the dropped block if it has fallen out of bounds
        if (this.transform.position.y <= -25)
        {
            Destroy(this.gameObject);
        }
	}


    /*
    ======================================================================================================================================================
    Checking If The Block Is Close Enough To The Player To Be Added To The Player's Inventory
    ======================================================================================================================================================
    */
    private void CheckForPlayerPickup()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(this.transform.position, 2);
        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            if (nearbyColliders[i].tag == "Player")
            {
                float distance = Vector3.Distance(nearbyColliders[i].transform.position, this.transform.position);
                if (distance < 1)
                {
                    //Add the block to the player's inventory
                    OnEventBlockPickup(blockType);
                    Destroy(this.gameObject);
                }
                else
                {
                    //Block Floats To The Player
                    Vector3 floatDirection = (nearbyColliders[i].transform.position - this.transform.position).normalized;
                    theRB.AddForce(floatDirection * floatForce);
                }
            }
        }
    }
}
