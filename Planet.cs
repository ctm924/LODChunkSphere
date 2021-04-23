using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Planet : MonoBehaviour
{

    [Range(1, 255)]
    public int resolution = 3;
    public int ChunkRes = 3;
    public bool toggle;
    public GameObject meshObj;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        Initialize();
    }

    void Initialize()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.name = "Planet";

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.up};

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<TerrainFace>();
                meshObj.GetComponent<TerrainFace>().localUp = directions[i];
                meshObj.GetComponent<TerrainFace>().ConstructMesh();
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution + 1, directions[i]);
        }
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}