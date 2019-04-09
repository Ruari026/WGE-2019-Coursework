using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassInts
{
    public int x;
}

public struct StructInts
{
    public int x;
}


public class StructTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            RunTest();
        }
    }

    private void RunTest()
    {
        ClassInts[] cInts = new ClassInts[1000000];
        StructInts[] sInts = new StructInts[1000000];

        for (int i = 0; i < 1000000; ++i)
        {
            cInts[i] = new ClassInts();
        }

        //for loop 1…
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        for (int i = 0; i < 1000000; i++)
        {
            RandomiseClass(cInts[i]);
        }
        stopWatch.Stop();
        System.TimeSpan test1Time = stopWatch.Elapsed;

        stopWatch.Reset();
        stopWatch.Start();
        //for loop 2…
        for (int i = 0; i < 1000000; i++)
        {
            RandomiseStruct(ref sInts[i]);
        }
        stopWatch.Stop();
        System.TimeSpan test2Time = stopWatch.Elapsed;

        System.TimeSpan difference = test1Time - test2Time;
        Debug.Log("Test Times\nTest 1: " + test1Time + "\nTest 2: " + test2Time + "\nDifference: " + difference);
    }

    void RandomiseStruct(ref StructInts s)
    {
        s.x = Random.Range(0, 100);
    }

    void RandomiseClass(ClassInts c)
    {
        c.x = Random.Range(0, 100);
    }
}
