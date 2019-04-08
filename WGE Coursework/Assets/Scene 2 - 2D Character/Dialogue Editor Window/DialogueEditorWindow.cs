using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

public class DialogueEditorWindow : EditorWindow
{
    static DialogueEditorWindow theWindow;

    //Saving & Loading Panel
    SaveLoadPanel saveLoadPanel;
    string fileName;

    //Resizing window panels
    Rect windowResizer;
    private bool isResizing;
    private float panelRatio = 0.3f;
    private GUIStyle resizerStyle;

    //Dialogue System Panel
    NodeBasedPanel nodeBasedPanel;
    Rect dialoguePanel;

    [MenuItem("CustomWindows/Dialogue Editor")]
    static void Init()
    {
        theWindow = (DialogueEditorWindow)EditorWindow.GetWindow(typeof(DialogueEditorWindow));
        theWindow.Show();
    }

    private void OnEnable()
    {
        //Setting up Window Information
        saveLoadPanel = new SaveLoadPanel();
        nodeBasedPanel = new NodeBasedPanel();

        saveLoadPanel.PassRequiredInformation(nodeBasedPanel);
        nodeBasedPanel.PassRequiredInformation(saveLoadPanel);

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

        if (GUI.changed)
        {
            Repaint();
        }
    }


    /*
    ====================================================================================================
    Handling the saving & loading panel
    ====================================================================================================
    */
    private void DrawSaveLoadPanel()
    {
        //Setting Panel Size
        Rect panelSize = new Rect(0, 0, position.width * panelRatio, position.height);
        saveLoadPanel.DrawSaveLoadPanel(panelSize);
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
        inputArea.width = 8;

        //Setting the cursor when hovered over area
        EditorGUIUtility.AddCursorRect(inputArea, MouseCursor.ResizeHorizontal);
    }


    /*
    ====================================================================================================
    Handling the dialogue system panel
    ====================================================================================================
    */
    public void DrawDialogueEditorPanel()
    {
        //Setting Panel Size
        Rect panelSize = new Rect(position.width * panelRatio, 0, position.width * (1 - panelRatio), position.height);

        nodeBasedPanel.DrawDialogueEditorPanel(panelSize);
        nodeBasedPanel.HandlePanelEvents(Event.current);
    }
}
