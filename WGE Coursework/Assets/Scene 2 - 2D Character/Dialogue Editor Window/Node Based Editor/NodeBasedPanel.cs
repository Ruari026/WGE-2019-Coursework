using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBasedPanel
{
    SaveLoadPanel theSaveLoadPanel;

    Vector2 offset = Vector2.zero;

    /*
    ====================================================================================================
    Setting Up Panel Information
    ====================================================================================================
    */
    public void PassRequiredInformation(SaveLoadPanel theSLP)
    {
        theSaveLoadPanel = theSLP;
    }


    /*
    ====================================================================================================
    Rendering Panel Elements
    ====================================================================================================
    */
    public void DrawDialogueEditorPanel(Rect panelSize)
    {
        //Drawing Panel Contents
        GUILayout.BeginArea(panelSize);

        //Drawing The Grid Background
        DrawGrid(panelSize, 20, 0.2f, Color.gray);
        DrawGrid(panelSize, 100, 0.4f, Color.gray);

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

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, panelSize.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(panelSize.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {

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
            case EventType.MouseDrag:
                if (e.button == 2)
                {
                    offset += e.delta;

                    GUI.changed = true;
                }
                break;
        }
    }
}
