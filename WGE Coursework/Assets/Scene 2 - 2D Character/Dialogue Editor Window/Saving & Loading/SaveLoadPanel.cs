using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveLoadPanel
{
    //File Handling
    string fileName;

    //Passing Information
    NodeBasedPanel theNodeBasedPanel;

    /*
    ====================================================================================================
    Setting Up Panel Information
    ====================================================================================================
    */
    public void PassRequiredInformation(NodeBasedPanel theNBP)
    {
        theNodeBasedPanel = theNBP;
    }


    /*
    ====================================================================================================
    Rendering Panel Elements
    ====================================================================================================
    */
    public void DrawSaveLoadPanel(Rect windowPanel)
    {
        //Indenting Panel
        Rect saveLoadPanel = new Rect(windowPanel.x + 5, windowPanel.y, windowPanel.width - 10, windowPanel.height - 10);

        //Drawing Panel Contents
        GUILayout.BeginArea(saveLoadPanel);

        GUILayout.Label("Saving & Loading File", EditorStyles.centeredGreyMiniLabel);

        if (GUILayout.Button("New Dialogue File"))
        {

        }

        if (GUILayout.Button("Open Dialogue File"))
        {
            fileName = EditorUtility.OpenFilePanel("Open Dialogue File (.xml)", "", "xml");
        }

        if (GUILayout.Button("Save Dialogue File"))
        {
            fileName = EditorUtility.SaveFilePanel("Open Dialogue File (.xml)", "", "", "xml");
        }

        GUILayout.EndArea();
    }
}
