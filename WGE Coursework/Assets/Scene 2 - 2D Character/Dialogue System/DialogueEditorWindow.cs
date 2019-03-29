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
    private float panelRatio = 0.5f;
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
        //Drawing the panel split line
        Rect resizer = new Rect(0, (position.height * panelRatio) - 5f, position.width, 10f);
        GUILayout.BeginArea(new Rect(resizer.position + (Vector2.up * 5f), new Vector2(position.width, 2)), resizerStyle);
        GUILayout.EndArea();

        EditorGUIUtility.AddCursorRect(resizer, MouseCursor.ResizeVertical);

        //Drawing The Lower Panel
        
    }


    /*
    ====================================================================================================
    Handling the saving & loading panel
    ====================================================================================================
    */
    private void DrawSaveLoadPanel()
    {
        //Setting Panel Size
        saveLoadPanel = new Rect(0, 0, position.width, position.height * 0.5f * panelRatio);

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
        //Setting handle Size

        //Drawing handle Contents
    }

    private void ResizingHandleInputs()
    {

    }


    /*
    ====================================================================================================
    Handling the dialogue system panel
    ====================================================================================================
    */
    private void DrawDialoguePanel()
    {
        //Setting Panel Size
        dialoguePanel = new Rect(0, position.height * 0.5f, position.width, position.height * 0.5f * (1 - panelRatio));
        
        //Drawing Panel Contents
        GUILayout.BeginArea(dialoguePanel);

        GUILayout.Label("Dialogue System Panel");

        GUILayout.EndArea();
    }


}
