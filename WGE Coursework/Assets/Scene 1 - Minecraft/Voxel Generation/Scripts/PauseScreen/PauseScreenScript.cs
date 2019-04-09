using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreenScript : MonoBehaviour
{
    public InputField fileName;
    public VoxelChunk theChunk;


    /*
    ======================================================================================================================================================
    Loading & Saving Voxel Chunk From File
    ======================================================================================================================================================
    */
    public void LoadChunkFromFile()
    {
        theChunk.LoadFile(fileName.text);
    }

    public void SaveChunkToFile()
    {
        theChunk.SaveFile(fileName.text);
    }


    /*
    ======================================================================================================================================================
    Inventory Sorting
    ======================================================================================================================================================
    */
    public void SortInventoryByName()
    {

    }

    public void SortInventoryByNumberHeld()
    {

    }

    public void SearchInventory(string name)
    {

    }
}
