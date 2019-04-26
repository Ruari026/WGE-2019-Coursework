using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcDialogueHandler : MonoBehaviour
{
    //Player Character
    public GameObject playerCharacter;
    public GameObject dialogueNotificationIcon;

    //Dialogue Information
    public string dialogueFileName;
    private List<ResponseStruct> dialogueResponses;

    [Header("Dialogue System UI Elements")]
    public RectTransform dialogueUI;
    public Text[] npcResponseTexts;
    public GameObject[] playerResponseButtons;
    public Text[] playerResponsesTexts;

    private string startPoint = "";
    private bool runningSystem = false;

    public delegate void EventDialogueLoad();
    public static event EventDialogueLoad OnEventDialogueStart;
    public static event EventDialogueLoad OnEventDialogueEnd;

    // Use this for initialization
    void Start()
    {
        string filePath = dialogueFileName + ".xml";
        dialogueResponses = DialogueFileLoader.LoadDialogueFile(filePath);
        startPoint = dialogueResponses[0].responceID;
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(this.transform.position, playerCharacter.transform.position);
        if (playerDistance < 3 && !runningSystem)
        {
            dialogueNotificationIcon.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogueSystem();
            }
        }
        else
        {
            dialogueNotificationIcon.SetActive(false);
        }
    }


    /*
    ====================================================================================================
    Running the Dialogue System
    ====================================================================================================
    */
    public void StartDialogueSystem()
    {
        OnEventDialogueStart();

        //Removing Control From The Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement2D>()._mState = MovementState.DISABLED;

        //Starting The Dialogue UI
        runningSystem = true;
        dialogueUI.gameObject.SetActive(true);
        GenerateDialogueUI(startPoint);
    }

    public void GenerateDialogueUI(string npcID)
    {
        if (npcID == "NPCEnd" || npcID == "NPC" || npcID == "")
        {
            EndDialogueSystem();
        }
        else
        {
            for (int i = 0; i < dialogueResponses.Count; i++)
            {
                ResponseStruct r = dialogueResponses[i];
                if (r.responceID == npcID)
                {
                    //Setting Dialogue UI Background Size
                    Vector2 newSize = new Vector2(dialogueUI.rect.width, dialogueUI.rect.height);
                    newSize.y = (160 + (r.responseConnections.Count * 80));
                    dialogueUI.sizeDelta = newSize;

                    //Getting the npc text
                    foreach (Text t in npcResponseTexts)
                    {
                        t.text = "NPC: " + r.responseContent;
                    }

                    //Getting the possible npc responces
                    foreach (GameObject g in playerResponseButtons)
                    {
                        g.SetActive(false);
                    }
                    for (int j = 0; j < r.responseConnections.Count; j++)
                    {
                        playerResponseButtons[j].SetActive(true);

                        foreach (ResponseStruct s in dialogueResponses)
                        {
                            if (s.responceID == r.responseConnections[j])
                            {
                                //Setting Response Button Text
                                playerResponsesTexts[j].text = (j + 1) + ": " + s.responseContent;

                                //Setting Button Click Events
                                playerResponseButtons[j].GetComponent<Button>().onClick.RemoveAllListeners();
                                if (s.responseConnections.Count > 0)
                                {
                                    playerResponseButtons[j].GetComponent<Button>().onClick.AddListener(delegate () { GenerateDialogueUI(s.responseConnections[0]); });
                                }
                                else
                                {
                                    playerResponseButtons[j].GetComponent<Button>().onClick.AddListener(delegate () { GenerateDialogueUI("End"); });
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void EndDialogueSystem()
    {
        OnEventDialogueEnd();

        //Ending The Dialogue UI 
        runningSystem = false;
        dialogueUI.gameObject.SetActive(false);

        //Restoring Control From The Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement2D>()._mState = MovementState.ON_GROUND;
    }
}
