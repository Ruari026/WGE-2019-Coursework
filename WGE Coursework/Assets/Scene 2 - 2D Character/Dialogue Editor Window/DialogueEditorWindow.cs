using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

public class DialogueEditorWindow : EditorWindow
{
    static DialogueEditorWindow theWindow;
    
    //Saving & Loading Panel
    Rect saveLoadPanel;
    string fileName;

    //Resizing window panels
    Rect windowResizer;
    private bool isResizing;
    private float panelRatio = 0.3f;
    private GUIStyle resizerStyle;

    //Dialogue System Panel
    Rect dialoguePanel;

    [MenuItem("CustomWindows/DialogueEditor")]
    static void Init()
    {
        theWindow = (DialogueEditorWindow)EditorWindow.GetWindow(typeof(DialogueEditorWindow));
        theWindow.Show();
    }

    private void OnEnable()
    {
        resizerStyle = new GUIStyle();
        resizerStyle.normal.background = (Texture2D)EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png");
    }

    private void OnGUI()
    {
        //Handling Saving & Loading Panel
        DrawSaveLoadPanel();

        //Handling Resizer Panel
        DrawResizingHandle();
        HandleResizingInputs();

        //Drawing The Lower Panel
        DrawDialogueEditorPanel();
    }


    /*
    ====================================================================================================
    Handling the saving & loading panel
    ====================================================================================================
    */
    private void DrawSaveLoadPanel()
    {
        //Setting Panel Size
        saveLoadPanel = new Rect(0, 0, position.width * panelRatio, position.height);

        //Drawing Panel Contents
        GUILayout.BeginArea(saveLoadPanel);

        GUILayout.Label("Saving & Loading File", EditorStyles.boldLabel);
        if (GUILayout.Button("Load Dialogue File"))
        {
            string fileName = EditorUtility.OpenFilePanel("Open Dialogue File (.xml)", "", "xml");
        }

        if (GUILayout.Button("Save Dialogue File"))
        {
            string fileName = EditorUtility.SaveFilePanel("Open Dialogue File (.xml)", "", "", "xml");
        }

        GUILayout.EndArea();
    }


    /*
    ====================================================================================================
    Handling resizing handle
    ====================================================================================================
    /*/
    private void DrawResizingHandle()
    {
        //Setting the panel split line size
        windowResizer = new Rect((position.width * panelRatio), 0, 3, position.height);

        //Drawing the panel split line
        GUILayout.BeginArea(windowResizer, resizerStyle);
        GUILayout.EndArea();
    }

    private void HandleResizingInputs()
    {
        //Setting input area size
        Rect inputArea = windowResizer;
        inputArea.x = inputArea.x - 3;
        inputArea.width = 9;

        //Setting the cursor when hovered over area
        EditorGUIUtility.AddCursorRect(inputArea, MouseCursor.ResizeHorizontal);
    }


    /*
    ====================================================================================================
    Handling the dialogue system panel
    ====================================================================================================
    */
    private void DrawDialogueEditorPanel()
    {
        //Setting Panel Size
        dialoguePanel = new Rect(position.width * panelRatio, 0, position.width * (1 - panelRatio), position.height);

        //Drawing Panel Contents
        GUILayout.BeginArea(dialoguePanel);

        //Drawing The Grid Background
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        GUILayout.Label("Dialogue System Panel", EditorStyles.boldLabel);

        GUILayout.EndArea();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        Vector3 drag = Vector3.zero;
        Vector3 offset = drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}
