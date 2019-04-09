using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class VoxelGenerator : MonoBehaviour
{
    //Voxel Generation
    private Mesh mesh;
    private MeshCollider meshCollider;
    private List<Vector3> vertexList;
    private List<int> triIndexList;
    private List<Vector2> UVList;
    private int numQuads = 0;

    //Texture Dictionary
    public List<string> texNames;
    public List<Vector2> texCoords;
    public float texSize;
    private Dictionary<string, Vector2> texNameCoordDictionary;
    
    public void Initialise()
    {
        CreateTextureNameCoordDictionary();

#if UNITY_EDITOR
        DestroyImmediate(GetComponent<MeshFilter>().sharedMesh, true);
        GetComponent<MeshFilter>().sharedMesh = new Mesh();
        mesh = GetComponent<MeshFilter>().sharedMesh;
#else
        mesh = GetComponent<MeshFilter>().mesh;
#endif
        numQuads = 0;
        meshCollider = GetComponent<MeshCollider>();

        vertexList = new List<Vector3>();
        triIndexList = new List<int>();
        UVList = new List<Vector2>();
    }

    // Clear previous data structures used to create the mesh
    public void ClearPreviousData()
    {
        vertexList.Clear();
        triIndexList.Clear();
        UVList.Clear();
        numQuads = 0;
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        // Convert index list to array and store in mesh
        mesh.vertices = vertexList.ToArray();

        // Convert index list to array and store in mesh
        mesh.triangles = triIndexList.ToArray();

        // Convert UV list to array and store in mesh
        mesh.uv = UVList.ToArray();
        mesh.RecalculateNormals();

        // Create a collision mesh
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;

        ClearPreviousData();
    }


    /*
    ==================================================
    Block Creation Methods
    ==================================================
    */
    public void CreateVoxel(int x, int y, int z, Vector2 uvCoords)
    {
        CreatePositiveXFace(x, y, z, uvCoords);
        CreateNegativeXFace(x, y, z, uvCoords);

        CreatePositiveYFace(x, y, z, uvCoords);
        CreateNegativeYFace(x, y, z, uvCoords);

        CreatePositiveZFace(x, y, z, uvCoords);
        CreateNegativeZFace(x, y, z, uvCoords);
    }
    public void CreateVoxel(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        CreatePositiveXFace(x, y, z, uvCoords);
        CreateNegativeXFace(x, y, z, uvCoords);

        CreatePositiveYFace(x, y, z, uvCoords);
        CreateNegativeYFace(x, y, z, uvCoords);

        CreatePositiveZFace(x, y, z, uvCoords);
        CreateNegativeZFace(x, y, z, uvCoords);
    }


    /*
    ==================================================
    Creating Block Faces
    ==================================================
    */
    //Drawing Block X Faces
    public void CreatePositiveXFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y, z + 1));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreatePositiveXFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y, z + 1));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }

    public void CreateNegativeXFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x, y, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z));
        vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreateNegativeXFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x, y, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z));
        vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }

    //Drawing Block Y Faces
    public void CreateNegativeYFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x + 1, y, z + 1));
        vertexList.Add(new Vector3(x, y, z + 1));
        vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreateNegativeYFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x + 1, y, z + 1));
        vertexList.Add(new Vector3(x, y, z + 1));
        vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }

    public void CreatePositiveYFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x, y + 1, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreatePositiveYFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x, y + 1, z));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }

    //Drawing Block Z Faces
    public void CreateNegativeZFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x, y, z));

        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreateNegativeZFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y + 1, z));
        vertexList.Add(new Vector3(x + 1, y, z));
        vertexList.Add(new Vector3(x, y, z));

        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }

    public void CreatePositiveZFace(int x, int y, int z, Vector2 uvCoords)
    {
        vertexList.Add(new Vector3(x + 1, y, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y, z + 1));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }
    public void CreatePositiveZFace(int x, int y, int z, string texture)
    {
        Vector2 uvCoords = texNameCoordDictionary[texture];

        vertexList.Add(new Vector3(x + 1, y, z + 1));
        vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y + 1, z + 1));
        vertexList.Add(new Vector3(x, y, z + 1));
        AddTriangleIndices();
        AddUVCoords(uvCoords);
    }


    /*
    ==================================================
    Setting Block Edges
    ==================================================
    */
    void AddTriangleIndices()
    {
        triIndexList.Add(numQuads * 4);
        triIndexList.Add((numQuads * 4) + 1);
        triIndexList.Add((numQuads * 4) + 3);
        triIndexList.Add((numQuads * 4) + 1);
        triIndexList.Add((numQuads * 4) + 2);
        triIndexList.Add((numQuads * 4) + 3);
        numQuads++;
    }


    /*
    ==================================================
    Setting Block Texture
    ==================================================
    */
    void AddUVCoords(Vector2 uvCoords)
    {
        UVList.Add(new Vector2(uvCoords.x, uvCoords.y + 0.5f));
        UVList.Add(new Vector2(uvCoords.x + 0.5f, uvCoords.y + 0.5f));
        UVList.Add(new Vector2(uvCoords.x + 0.5f, uvCoords.y));
        UVList.Add(new Vector2(uvCoords.x, uvCoords.y));
    }

    void CreateTextureNameCoordDictionary()
    {
        // Create a dictionary instance before using
        texNameCoordDictionary = new Dictionary<string, Vector2>();

        // Check the number of names and coordinates match
        if (texNames.Count == texCoords.Count)
        {
            // Iterate through both lists
            for (int i = 0; i < texNames.Count; i++)
            {
                texNameCoordDictionary.Add(texNames[i], texCoords[i]);
            }
        }
        else
        {
            // List counts are not matching
            Debug.Log("texNames and texCoords count mismatch");
        }
    }
}
