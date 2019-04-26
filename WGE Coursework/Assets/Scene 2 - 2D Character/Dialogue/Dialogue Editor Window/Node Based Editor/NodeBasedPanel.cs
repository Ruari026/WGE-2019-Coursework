using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBasedPanel
{
    SaveLoadPanel theSaveLoadPanel;
    Rect panelDimentions;
    Vector2 offset = Vector2.zero;

    //Node Information
    private Node startNode;
    private Node endNode;
    public List<Node> fileNodes;
    public List<Connection> fileConnections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;


    public NodeBasedPanel()
    {
        CreateNewDialogue();
    }

    /*
    ====================================================================================================
    Setting Up Panel Information
    ====================================================================================================
    */
    public void PassRequiredInformation(SaveLoadPanel theSLP)
    {
        theSaveLoadPanel = theSLP;
    }

    public void CreateNewDialogue()
    {
        fileNodes = new List<Node>();
        fileConnections = new List<Connection>();

        startNode = new Node(NodeType.START, new Vector2(50, 50), OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
        fileNodes.Add(startNode);

        endNode = new Node(NodeType.END, new Vector2(50, 250), OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
        fileNodes.Add(endNode);

        offset = Vector2.zero;
    }


    /*
    ====================================================================================================
    Rendering Panel Elements
    ====================================================================================================
    */
    public void DrawDialogueEditorPanel(Rect panelSize)
    {
        panelDimentions = panelSize;

        //Drawing Panel Contents
        GUILayout.BeginArea(panelSize);

        //Drawing The Grid Background
        DrawGrid(panelSize, 20, 0.2f, new Color(0.41f, 0.41f, 0.41f, 1));
        DrawGrid(panelSize, 100, 0.4f, new Color(0.31f, 0.31f, 0.31f, 1));

        //Drawing Panel Elements
        HandleNodes();
        HandleConnections();
        DrawConnectionLine(Event.current);

        GUILayout.Label("Dialogue System Panel", EditorStyles.centeredGreyMiniLabel);

        GUILayout.EndArea();
    }

    private void DrawGrid(Rect panelSize, float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(panelSize.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(panelSize.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i <= widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, panelSize.height + gridSpacing, 0f) + newOffset);
        }

        for (int j = 0; j <= heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(panelSize.width + gridSpacing, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void HandleNodes()
    {
        if (fileNodes != null)
        {
            for (int i = 0; i < fileNodes.Count; i++)
            {
                fileNodes[i].HandleNode(Event.current);
            }
        }
    }

    private void HandleConnections()
    {
        if (fileConnections != null)
        {
            for (int i = 0; i < fileConnections.Count; i++)
            {
                fileConnections[i].Draw();
            }
        }
    }


    /*
    ====================================================================================================
    Handling Panel Inputs
    ====================================================================================================
    */
    public void HandlePanelEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }
                else if (e.button == 1)
                {
                    GenericMenu genericMenu = new GenericMenu();
                    genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(e.mousePosition));
                    genericMenu.ShowAsContext();
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 2)
                {
                    offset += e.delta;

                    if (fileNodes != null)
                    {
                        for (int i = 0; i < fileNodes.Count; i++)
                        {
                            fileNodes[i].Drag(e.delta);
                        }
                    }

                    GUI.changed = true;
                }
                break;
        }
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (fileNodes == null)
        {
            fileNodes = new List<Node>();
        }

        Vector2 nodePosition = mousePosition;
        nodePosition.x -= panelDimentions.x;
        fileNodes.Add(new Node(NodeType.DIALOGUE, nodePosition, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));

        GUI.changed = true;
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }


    /*
    ====================================================================================================
    Passable Panel Element Actions
    ====================================================================================================
    */
    public void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    public void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
        else
        {
            foreach (Connection c in fileConnections)
            {
                if (c.outPoint == selectedOutPoint && c.inPoint != null)
                {
                    fileConnections.Remove(c);
                }
            }
        }
    }

    public void OnClickRemoveNode(Node node)
    {
        if (fileConnections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < fileConnections.Count; i++)
            {
                if (fileConnections[i].inPoint == node.inPoint || node.outPoints.Contains(fileConnections[i].outPoint))
                {
                    connectionsToRemove.Add(fileConnections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                fileConnections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        fileNodes.Remove(node);
    }

    public void OnClickRemoveConnection(Connection connection)
    {
        fileConnections.Remove(connection);
    }

    public void CreateConnection()
    {
        if (fileConnections == null)
        {
            fileConnections = new List<Connection>();
        }

        fileConnections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }


    /*
    ====================================================================================================
    Sharing Panel Information
    ====================================================================================================
    */
    public List<Node> GetPanelNodes()
    {
        return this.fileNodes;
    }

    public List<Connection> GetPanelConnections()
    {
        return this.fileConnections;
    }
}
