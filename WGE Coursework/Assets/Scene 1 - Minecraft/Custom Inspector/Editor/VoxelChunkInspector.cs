using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelChunk))]
public class VoxelChunkInspector : Editor
{
    string fileName = "VoxelChunk";

    public override void OnInspectorGUI()
    {
        VoxelChunk myTarget = (VoxelChunk)target; //casting target to be voxel chunk

        fileName = EditorGUILayout.TextField(fileName);
        if (GUILayout.Button("Load from XML"))
        {
            //Load functionality
            myTarget.LoadFile(fileName);
        }

        if (GUILayout.Button("Save to XML"))
        {
            //Save functionality
            myTarget.SaveFile(fileName);
        }

        if (GUILayout.Button("Clear Terrain"))
        {
            //Clear functionality
            myTarget.ClearFile();
        }
    }
}
