using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFileLoader : MonoBehaviour
{
    /*
    ====================================================================================================
    Loading The Dialogue File
    ====================================================================================================
    */
    public static List<ResponseStruct> LoadDialogueFile(string filePath)
    {
         List<ResponseStruct> dialogueResponses = new List<ResponseStruct>();
         ResponseStruct rs = new ResponseStruct();

        //Checking If File Exists
        if (System.IO.File.Exists(filePath))
        {
            XmlReader xmlReader = XmlReader.Create(filePath);
            dialogueResponses = new List<ResponseStruct>();

            while (xmlReader.Read())
            {
                //Creating New Dialogue Response
                if (xmlReader.Name == "Response" && xmlReader.NodeType == XmlNodeType.Element)
                {
                    dialogueResponses.Add(new ResponseStruct());

                    rs = dialogueResponses[dialogueResponses.Count - 1];
                    rs.responseConnections = new List<string>();
                }

                //Setting Response Details
                if (xmlReader.Name == "ID")
                {
                    rs.responceID = xmlReader.ReadElementString();
                }
                else if (xmlReader.Name == "Content")
                {
                    rs.responseContent = xmlReader.ReadElementString();
                }
                else if (xmlReader.Name == "Connection")
                {
                    rs.responseConnections.Add(xmlReader.ReadElementString());
                }
            }
        }
        else
        {
            Debug.Log("No File Exists At Supplied Location");
        }

        return dialogueResponses;
    }
}
