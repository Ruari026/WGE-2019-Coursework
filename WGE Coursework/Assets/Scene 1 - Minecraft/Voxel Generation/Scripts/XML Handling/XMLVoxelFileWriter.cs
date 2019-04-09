using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLVoxelFileWriter
{
    /*
    ====================================================================================================
    Checking if an xml file exists and the supplied location
    ====================================================================================================
    */
    public static bool CheckIfFileExists(string fileName)
    {
        if (System.IO.File.Exists(fileName + ".xml"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    ====================================================================================================
    Write a voxel chunk to XML file
    ====================================================================================================
    */
    public static void SaveChunkToXMLFile(int[,,] voxelArray, string fileName)
    {
        XmlWriterSettings writerSettings = new XmlWriterSettings();
        writerSettings.Indent = true;
        //Creating a write instance
        XmlWriter xmlWriter = XmlWriter.Create(fileName + ".xml", writerSettings);
        //Writing the beginning of the document
        xmlWriter.WriteStartDocument();

        //Creating VoxelChunk Root Element
        xmlWriter.WriteStartElement("VoxelChunk");


        //Saving Block Positions And Blocktype
        for (int x = 0; x < voxelArray.GetLength(0); x++)
        {
            for (int y = 0; y < voxelArray.GetLength(1); y++)
            {
                for (int z = 0; z < voxelArray.GetLength(1); z++)
                {
                    if (voxelArray[x, y, z] != 0)
                    {
                        // Create a single voxel element
                        xmlWriter.WriteStartElement("Voxel");

                        //Creating attributes to store the indices
                        xmlWriter.WriteAttributeString("x", x.ToString());
                        xmlWriter.WriteAttributeString("y", y.ToString());
                        xmlWriter.WriteAttributeString("z", z.ToString());

                        //Storing the voxel type
                        xmlWriter.WriteString(voxelArray[x, y, z].ToString());

                        //Ending the voxel element
                        xmlWriter.WriteEndElement();
                    }
                }
            }
        }

        //Ending the VoxelChunk root element
        xmlWriter.WriteEndElement();
        //Writing the end of the document
        xmlWriter.WriteEndDocument();
        //Closing the document to save
        xmlWriter.Close();
    }


    /*
    ====================================================================================================
    Read a voxel chunk from XML file
    ====================================================================================================
    */
    public static int[,,] LoadChunkFromXMLFile(int size, string filename)
    {
        int[,,] voxelArray = new int[size, size, size];

        //Creating an xml reader with the file supplied
        XmlReader xmlReader = XmlReader.Create(filename + ".xml");

        //Iterate through and read every line in the xml file
        while (xmlReader.Read())
        {
            Debug.Log("Creating Voxel Blocks");
            //Building The Saved Voxel
            if (xmlReader.IsStartElement("Voxel"))
            {
                //Getting the voxel position
                int x = int.Parse(xmlReader["x"]);
                int y = int.Parse(xmlReader["y"]);
                int z = int.Parse(xmlReader["z"]);

                //Getting the voxel blocktype
                xmlReader.Read();
                int value = int.Parse(xmlReader.Value);

                //Setting the blocktype
                voxelArray[x, y, z] = value;
            }
        }

        return voxelArray;
    }
}
