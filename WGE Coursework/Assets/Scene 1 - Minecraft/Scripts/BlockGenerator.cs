using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.OnPlaceBlock += PlaceBlock;

        PlayerController.OnDestroyBlock += DestroyBlock;
    }


    /*
    ==================================================
    Mesh Interaction
    ==================================================
    */
    private void PlaceBlock(Vector3 position)
    {
        Debug.Log("Placing Block At: " + position);
    }

    private void DestroyBlock()
    {
        Debug.Log("Destroying Block");
    }
}
