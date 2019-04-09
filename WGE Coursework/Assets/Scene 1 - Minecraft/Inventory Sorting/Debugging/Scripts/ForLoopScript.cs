using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoopScript : MonoBehaviour {

    // start for loop 
    public void ExecuteLoop()
    {
        int x = 0;
	
	    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
	    stopwatch.Start();

        for (int i = 0; i < 500; i++)
        {
	        x += i;
	        //Debug.Log("Value of x = " + x);
        }

	    stopwatch.Stop();
	    Debug.Log("Time taken: " + (stopwatch.Elapsed));
    }
}