using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

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
            theNodeBasedPanel.CreateNewDialogue();
        }

        if (GUILayout.Button("Open Dialogue File"))
        {
            fileName = EditorUtility.OpenFilePanel("Open Dialogue File (.xml)", "", "xml");
            LoadDialogueFile();

        }

        if (GUILayout.Button("Save Dialogue File"))
        {
            fileName = EditorUtility.SaveFilePanel("Open Dialogue File (.xml)", "", "", "xml");
            SaveDialogueFile();
        }

        GUILayout.EndArea();
    }


    /*
    ====================================================================================================
    Loading Dialogue File
    ====================================================================================================
    */
    private void LoadDialogueFile()
    {
        //Creating List of Nodes
        List<Node> newNodes = new List<Node>();
        List<Connection> newConnections = new List<Connection>();

        //Getting Stored Responses
        List<ResponseStruct> responses = DialogueFileLoader.LoadDialogueFile(fileName);


        //Setting Nodes
        Node startNode = new Node(NodeType.START, new Vector2(50, 50), theNodeBasedPanel.OnClickInPoint, theNodeBasedPanel.OnClickOutPoint, theNodeBasedPanel.OnClickRemoveNode);
        newNodes.Add(startNode);

        Node endNode = new Node(NodeType.END, new Vector2(50, 250), theNodeBasedPanel.OnClickInPoint, theNodeBasedPanel.OnClickOutPoint, theNodeBasedPanel.OnClickRemoveNode);
        newNodes.Add(endNode);
        
        foreach (ResponseStruct r in responses)
        {
            if (r.responceID.Substring(0,3) == "NPC")
            {
                Debug.Log("Adding New Node");
                
                //Adding New Node
                Node newNode = new Node(NodeType.DIALOGUE, Vector2.zero, theNodeBasedPanel.OnClickInPoint, theNodeBasedPanel.OnClickOutPoint, theNodeBasedPanel.OnClickRemoveNode);
                
                //Setting Node Content
                newNode.npcResponse = r;

                foreach (string playerResponceID in r.responseConnections)
                {
                    foreach (ResponseStruct s in responses)
                    {
                        if (s.responceID == playerResponceID)
                        {
                            newNode.playerResponses.Add(s);
                        }
                    }
                }

                newNodes.Add(newNode);
            }
        }

        theNodeBasedPanel.fileNodes = newNodes;
        theNodeBasedPanel.fileConnections = newConnections;
    }


    /*
    ====================================================================================================
    Saving Dialogue File
    ====================================================================================================
    */
    private void SaveDialogueFile()
    {
        XmlWriterSettings writerSettings = new XmlWriterSettings();
        writerSettings.Indent = true;

        //Creating a write instance
        XmlWriter xmlWriter = XmlWriter.Create(fileName, writerSettings);
        xmlWriter.WriteStartDocument();

        //Creating Root Element
        xmlWriter.WriteStartElement("Dialogue");

        //Storing Each Node's Information
        List<Node> theNodes = theNodeBasedPanel.GetPanelNodes();
        List<Connection> theConnections = theNodeBasedPanel.GetPanelConnections();

        //Getting Starting Node
        Node startingNode = null;
        foreach (Node n in theNodes)
        {
            if (n.nodeType == NodeType.START)
            {
                foreach (Connection c in theConnections)
                {
                    if (c.outPoint == n.outPoints[0])
                    {
                        startingNode = c.inPoint.node;
                    }
                }
            }
        }

        //Writing The Starting Node Information
        if (startingNode != null)
        {
            XMLWriteNodeResponses(startingNode, xmlWriter);
        }
        else
        {
            Debug.Log("Error: Dialogue Editor Has No Start Point");
        }

        //Ending the document
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Close();
    }

    private void XMLWriteNodeResponses(Node theNode, XmlWriter theWriter)
    {
        //Writing NPC Responce Information
        theWriter.WriteStartElement("Response");

        theWriter.WriteStartElement("ID");
        theWriter.WriteString("NPC" + theNode.npcResponse.responceID);
        theWriter.WriteEndElement();

        theWriter.WriteStartElement("Content");
        theWriter.WriteString(theNode.npcResponse.responseContent);
        theWriter.WriteEndElement();

        theWriter.WriteStartElement("Connections");
        for (int i = 0; i < theNode.playerResponses.Count; i++)
        {
            theWriter.WriteStartElement("Connection");
            theWriter.WriteString("Player" + (theNode.playerResponses[i].responceID));
            theWriter.WriteEndElement();
        }
        theWriter.WriteEndElement();
        theWriter.WriteEndElement();

        //Writing Each Player Response Information
        for (int i = 0; i < theNode.playerResponses.Count; i++)
        {
            theWriter.WriteStartElement("Response");

            theWriter.WriteStartElement("ID");
            theWriter.WriteString("Player" + theNode.playerResponses[i].responceID);
            theWriter.WriteEndElement();

            theWriter.WriteStartElement("Content");
            theWriter.WriteString(theNode.playerResponses[i].responseContent);
            theWriter.WriteEndElement();

            theWriter.WriteStartElement("Connections");
            theWriter.WriteStartElement("Connection");
            foreach (Connection c in theNodeBasedPanel.GetPanelConnections())
            {
                if (c.outPoint == theNode.outPoints[i])
                {
                    theWriter.WriteString("NPC" + c.inPoint.node.npcResponse.responceID);
                }
            }
            theWriter.WriteEndElement();
            theWriter.WriteEndElement();

            theWriter.WriteEndElement();
        }

        for (int i = 0; i < theNode.outPoints.Count; i++)
        {
            foreach (Connection c in theNodeBasedPanel.GetPanelConnections())
            {
                if (c.outPoint == theNode.outPoints[i])
                {
                    XMLWriteNodeResponses(c.inPoint.node, theWriter);
                }
            }
        }
    }
}
