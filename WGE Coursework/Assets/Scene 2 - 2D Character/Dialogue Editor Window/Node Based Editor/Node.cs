using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Node
{
    //Node Information
    private NodeType nodeType;
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    //Dialogue System Information
    private string npcText = "";
    private int playerResponcesAmount = 1;
    private List<string> playerResponcesText = new List<string>();

    /*
    ====================================================================================================
    Constructor
    ====================================================================================================     
    */
    public Node(NodeType type, Vector2 position, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        nodeType = type;
        switch (nodeType)
        {
            case (NodeType.START):
                {
                    //Setting Up Node
                    rect = new Rect(position.x, position.y, 200, 50);
                    outPoints.Add(new ConnectionPoint(this, ConnectionPointType.OUT, OnClickOutPoint));

                    //Setting Node Styles
                    defaultNodeStyle = new GUIStyle();
                    defaultNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;
                    defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

                    selectedNodeStyle = new GUIStyle();
                    selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3 on.png") as Texture2D;
                    selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
                }
                break;

            case (NodeType.END):
                {
                    //Setting Up Node
                    rect = new Rect(position.x, position.y, 200, 50);
                    inPoint = new ConnectionPoint(this, ConnectionPointType.IN, OnClickInPoint);

                    //Setting Node Styles
                    defaultNodeStyle = new GUIStyle();
                    defaultNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node6.png") as Texture2D;
                    defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

                    selectedNodeStyle = new GUIStyle();
                    selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node6 on.png") as Texture2D;
                    selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
                }
                break;

            default:
                {
                    //Setting Up Node
                    rect = new Rect(position.x, position.y, 225, 125);
                    inPoint = new ConnectionPoint(this, ConnectionPointType.IN, OnClickInPoint);
                    outPoints.Add(new ConnectionPoint(this, ConnectionPointType.OUT, OnClickOutPoint));
                    playerResponcesText.Add("");

                    //Setting Node Styles
                    defaultNodeStyle = new GUIStyle();
                    defaultNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
                    defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

                    selectedNodeStyle = new GUIStyle();
                    selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
                    selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
                }
                break;
        }
        style = defaultNodeStyle;

        //Setting Node Actions
        OnRemoveNode = OnClickRemoveNode;
    }


    /*
    ====================================================================================================
    Node Inputs & Rendering
    ====================================================================================================     
    */
    public void HandleNode(Event e)
    {
        DrawNode();
        ProcessNodeEvents(e);
    }

    public void DrawNode()
    {
        //Drawing Connection Points
        if (inPoint != null)
        {
            Rect renderPosition = new Rect(rect.x - 2, rect.y + 20, 10, 20);

            inPoint.HandleConnectionPoint(renderPosition);
        }

        if (outPoints.Count > 0)
        {
            for (int i = 0; i < outPoints.Count; i++)
            {
                Rect renderPosition = new Rect(rect.x + rect.width - 8, rect.y + 60 + (40 * i), 10, 20);

                outPoints[i].HandleConnectionPoint(renderPosition);
            }
        }

        //Drawing Dialogue Options Box
        GUI.Box(rect, "", style);
        switch (nodeType)
        {
            case (NodeType.START):
                {
                    GUIStyle textStyle = new GUIStyle();
                    textStyle.fontStyle = FontStyle.Bold;
                    textStyle.alignment = TextAnchor.MiddleCenter;

                    Rect textRect = new Rect(rect.x + 5, rect.y + 5, rect.width - 10, rect.height - 10);

                    GUI.Label(textRect, " DIALOGUE START", textStyle);
                }
                break;

            case (NodeType.END):
                {
                    GUIStyle textStyle = new GUIStyle();
                    textStyle.fontStyle = FontStyle.Bold;
                    textStyle.alignment = TextAnchor.MiddleCenter;

                    Rect textRect = new Rect(rect.x + 5, rect.y + 5, rect.width - 10, rect.height - 10);

                    GUI.Label(textRect, "DIALOGUE END", textStyle);
                }
                break;

            default:
                {
                    //Handling NPC Dialogue Editing
                    Rect npcTitleRect = new Rect(rect.x + 10, rect.y + 10, rect.width - 20, 20);
                    GUI.Label(npcTitleRect, "NPC SAYS:", EditorStyles.boldLabel);

                    Rect npcTextInputRect = new Rect(rect.x + 10, rect.y + 25, rect.width - 20, 20);
                    npcText = GUI.TextField(npcTextInputRect, npcText);

                    Rect split = new Rect(rect.x + 10, rect.y + 52, rect.width - 20, 2);
                    Handles.DrawLine(new Vector3(split.x, split.y, 0), new Vector3(split.x + split.width, split.y, 0));

                    //Handling Player Dialogue Option Editing
                    Rect playerTitleRect = new Rect(rect.x + 10, rect.y + 60, rect.width - 20, 20);
                    GUI.Label(playerTitleRect, "PLAYER RESPONCES:", EditorStyles.boldLabel);
                    
                    Rect responceIncreaseRect = new Rect(rect.x + rect.width - 75, rect.y + 60, 30, 20);
                    if (GUI.Button(responceIncreaseRect, "+"))
                    {
                        playerResponcesAmount++;
                        if (playerResponcesAmount > 4)
                        {
                            playerResponcesAmount = 4;
                        }
                        else
                        {
                            playerResponcesText.Add("");
                        }

                        rect.height = 90 + (40 * playerResponcesAmount);
                    }
                    Rect optionDecreaseRect = new Rect(rect.x + rect.width - 40, rect.y + 60, 30, 20);
                    if (GUI.Button(optionDecreaseRect, "-"))
                    {
                        playerResponcesAmount--;
                        if (playerResponcesAmount < 1)
                        {
                            playerResponcesAmount = 1;
                        }
                        else
                        {
                            playerResponcesText.RemoveAt(playerResponcesAmount);
                        }

                        rect.height = 90 + (40 * playerResponcesAmount);
                    }

                    for (int i = 0; i < playerResponcesAmount; i++)
                    {
                        Rect playerOptionTitleRect = new Rect(rect.x + 10, rect.y + 75 + (40 * i), rect.width - 20, 20);
                        GUI.Label(playerOptionTitleRect, "Player Responce " + (i + 1) + ":");

                        Rect playerOptionInputRect = new Rect(rect.x + 10, rect.y + 90 + (40 * i), rect.width - 20, 20);
                        npcText = GUI.TextField(playerOptionInputRect, playerResponcesText[i]);
                    }
                }
                break;
        }
    }

    public bool ProcessNodeEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition) && nodeType == NodeType.DIALOGUE)
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                {
                    isDragged = false;
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }
    
    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }


    /*
    ====================================================================================================
    Node Actions
    ====================================================================================================     
    */
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}


public enum NodeType
{
    START,
    DIALOGUE,
    END
}