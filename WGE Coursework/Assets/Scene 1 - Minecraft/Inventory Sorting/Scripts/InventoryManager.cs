using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Parent object inventory item
    public Transform parentPanel;
    // Item info to build inventory items
    public List<Sprite> itemSprites;
    public List<string> itemNames;
    public List<int> itemAmounts;
    // Starting template item
    public GameObject startItem;

    List<InventoryItemScript> inventoryList;

    // Use this for initialization
    void Start()
    {
        inventoryList = new List<InventoryItemScript>();
        for (int i = 0; i < itemNames.Count; i++)
        {
            GameObject inventoryItem = (GameObject)Instantiate(startItem);
            inventoryItem.transform.SetParent(parentPanel);
            inventoryItem.SetActive(true);

            InventoryItemScript iis = inventoryItem.GetComponent<InventoryItemScript>();
            iis.itemSprite.sprite = itemSprites[i];
            iis.itemNameText.text = itemNames[i];
            iis.itemName = itemNames[i];
            iis.itemAmountText.text = itemAmounts[i].ToString();
            iis.itemAmount = itemAmounts[i];

            inventoryList.Add(iis);
        }

        DisplayListInOrder();
    }

    void DisplayListInOrder()
    {
        float yOffset = 55f;

        Vector3 startPos = startItem.transform.position;

        foreach (InventoryItemScript iis in inventoryList)
        {
            iis.transform.position = startPos;
            startPos.y -= yOffset;
        }
    }


    /*
    ====================================================================================================
    Sorting The Inventory
    ====================================================================================================
    */
    //Selection Sort
    public void SelectionSortInventory()
    {
        for (int i = 0; i < inventoryList.Count - 1; i++)
        {
            int minIndex = i;

            for (int j = i; j < inventoryList.Count; j++)
            {
                if (inventoryList[j].itemAmount < inventoryList[minIndex].itemAmount)
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                InventoryItemScript iis = inventoryList[i];
                inventoryList[i] = inventoryList[minIndex];
                inventoryList[minIndex] = iis;
            }
        }

        DisplayListInOrder();
    }

    //Quick Sort
    public void StartQuickSort()
    {
        inventoryList = QuickSort(inventoryList);
        DisplayListInOrder();
    }
    List<InventoryItemScript> QuickSort(List<InventoryItemScript> listIn)
    {
        if (listIn.Count <= 1)
        {
            return listIn;
        }
        else
        {
            int pivotIndex = 0;

            List<InventoryItemScript> leftList = new List<InventoryItemScript>();
            List<InventoryItemScript> rightList = new List<InventoryItemScript>();

            for (int i = 1; i < listIn.Count; i++)
            {
                if (listIn[i].itemAmount > listIn[pivotIndex].itemAmount)
                {
                    leftList.Add(listIn[i]);
                }
                else
                {
                    rightList.Add(listIn[i]);
                }
            }

            leftList = QuickSort(leftList);
            rightList = QuickSort(rightList);

            leftList.Add(listIn[pivotIndex]);
            leftList.AddRange(rightList);

            return leftList;
        }
    }

    //Merge Sort
    public void StartMergeSort()
    {
        inventoryList = MergeSort(inventoryList);
        DisplayListInOrder();
    }
    List<InventoryItemScript> MergeSort(List<InventoryItemScript> listIn)
    {
        if (listIn.Count <= 1)
        {
            //Since there is only one element in the list it is already sorted
            return listIn;
        }
        else
        {
            //Finding the mid point in the array and splitting it into two halves
            int midPoint = Mathf.FloorToInt(listIn.Count / 2);

            List<InventoryItemScript> leftList = new List<InventoryItemScript>();
            for (int i = 0; i < midPoint; i++)
            {
                leftList.Add(listIn[i]);
            }

            List<InventoryItemScript> rightList = new List<InventoryItemScript>();
            for (int i = midPoint; i < listIn.Count; i++)
            {
                rightList.Add(listIn[i]);
            }


            //Merge sorting the two halves
            leftList = MergeSort(leftList);
            rightList = MergeSort(rightList);


            //Merging the two sorted halves
            List<InventoryItemScript> sortedList = new List<InventoryItemScript>();



            return sortedList;
        }
    }
}
