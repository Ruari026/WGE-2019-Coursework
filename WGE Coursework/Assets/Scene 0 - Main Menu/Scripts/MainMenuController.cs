using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

	}


    /*
    ====================================================================================================
    Scene Button Methods
    ====================================================================================================
    */
    public void MoveToScene1()
    {
        SceneManager.LoadScene("Scene 1");
    }

    public void MoveToScene2()
    {
        SceneManager.LoadScene("Scene 2");
    }
}
