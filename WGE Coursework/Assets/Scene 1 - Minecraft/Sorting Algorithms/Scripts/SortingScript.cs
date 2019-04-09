using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingScript : MonoBehaviour
{
    public int[] collection1;
    public int[] collection2;

    /*
    ====================================================================================================
    UI Button Methods
    ====================================================================================================
    */
    public void InsertionSortCollection1()
    {
        int[] sortedCollection = collection1;
        sortedCollection = InsertionSort(sortedCollection);

        string s = "";
        for (int i = 0; i < sortedCollection.Length; i++)
        {
            s += sortedCollection[i].ToString() + "\n";
        }
        Debug.Log(s);
    }

    public void InsertionSortCollection2()
    {
        int[] sortedCollection = collection2;
        sortedCollection = InsertionSort(sortedCollection);

        string s = "";
        for (int i = 0; i < sortedCollection.Length; i++)
        {
            s += sortedCollection[i].ToString() + "\n";
        }
        Debug.Log(s);

    }

    public void BubbleSortCollection1()
    {
        int[] sortedCollection = collection1;
        sortedCollection = BubbleSort(sortedCollection);

        string s = "";
        for (int i = 0; i < sortedCollection.Length; i++)
        {
            s += sortedCollection[i].ToString() + "\n";
        }
        Debug.Log(s);
    }

    public void BubbleSortCollection2()
    {
        int[] sortedCollection = collection2;
        sortedCollection = BubbleSort(sortedCollection);

        string s = "";
        for (int i = 0; i < sortedCollection.Length; i++)
        {
            s += sortedCollection[i].ToString() + "\n";
        }
        Debug.Log(s);

    }


    /*
    ====================================================================================================
    Simple Sorting Algorithms
    ====================================================================================================
    */
    private int[] InsertionSort(int[] collectionToBeSorted)
    {
        int[] sorted = collectionToBeSorted;

        int i, j;
        for(j = 1; j < sorted.Length; j++)
        {
            i = j;
            while (i  > 0 && sorted[i-1] > sorted[i])
            {
                int swap = sorted[i];
                sorted[i] = sorted[i - 1];
                sorted[i - 1] = swap;
                i--;
            }
        }

        return sorted;
    }

    private int[] BubbleSort(int[] collectionToBeSorted)
    {
        int[] sorted = collectionToBeSorted;

        for (int i = 0; i < sorted.Length; i++)
        {
            bool hasSwapped = false;
            for (int j = 0; j < sorted.Length - 1; j++)
            {
                if (sorted[j] > sorted[j + 1])
                {
                    int swap = sorted[j];
                    sorted[j] = sorted[j + 1];
                    sorted[j + 1] = swap;

                    hasSwapped = true;
                }

                if (!hasSwapped)
                {
                    break;
                }
            }
        }

        return sorted;
    }
}
