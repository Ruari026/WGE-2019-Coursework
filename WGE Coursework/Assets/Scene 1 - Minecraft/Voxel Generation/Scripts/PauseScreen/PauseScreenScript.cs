using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreenScript : MonoBehaviour
{
    //Required Information For The Scene
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


    /*
    ======================================================================================================================================================
    Other
    ======================================================================================================================================================
    */
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scene 0");

    }

}
