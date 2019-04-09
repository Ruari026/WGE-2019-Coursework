using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    public Sprite slotDefaultSprite;
    public Sprite slotSelectedSprite;

    public Image[] inventorySlots;
    public int[] inventoryAmounts;
    public Text[] amountTexts;

    private int currentSelectedSlot = 0;

	// Use this for initialization
	void Start ()
    {
        UpdateSelectedSlot();
	}

    // When game object is enabled
    void OnEnable()
    {
        PlayerScript.OnEventGetBlock += GetSelectedSlot;
        VoxelBlock.OnEventBlockPickup += AddToInventory;
    }
    // When game object is disabled
    void OnDisable()
    {
        PlayerScript.OnEventGetBlock -= GetSelectedSlot;
        VoxelBlock.OnEventBlockPickup -= AddToInventory;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!PlayerScript.paused)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    currentSelectedSlot++;

                    if (currentSelectedSlot >= inventorySlots.Length)
                    {
                        currentSelectedSlot = 0;
                    }
                }
                else
                {
                    currentSelectedSlot--;

                    if (currentSelectedSlot < 0)
                    {
                        currentSelectedSlot = inventorySlots.Length - 1;
                    }
                }

                UpdateSelectedSlot();
            }
        }
	}

    private void UpdateSelectedSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].sprite = slotDefaultSprite;
        }
        inventorySlots[currentSelectedSlot].sprite = slotSelectedSprite;
    }


    /*
    ====================================================================================================
    Returning Information For The Player Controller
    ====================================================================================================
    */
    private bool GetSelectedSlot(out int blockType)
    {
        blockType = currentSelectedSlot;
        if (inventoryAmounts[currentSelectedSlot] != 0)
        {
            //Setting New Block Amount
            inventoryAmounts[currentSelectedSlot]--;

            //Setting UI Display
            amountTexts[(currentSelectedSlot * 2)].text = inventoryAmounts[currentSelectedSlot].ToString();
            amountTexts[(currentSelectedSlot * 2) + 1].text = inventoryAmounts[currentSelectedSlot].ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddToInventory(int blockType)
    {
        int i = blockType - 1;

        //Setting New Block Amount
        inventoryAmounts[i]++;

        //Setting UI Display
        amountTexts[(i * 2)].text = inventoryAmounts[i].ToString();
        amountTexts[(i * 2) + 1].text = inventoryAmounts[i].ToString();
    }
}
