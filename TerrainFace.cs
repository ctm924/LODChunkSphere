using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TerrainFace : MonoBehaviour
{

    Mesh mesh;
    public int resolution = 2;
    public int ChunkRes = 2;
    public Vector3 localUp;
    public Vector3 childVertex;
    Vector3 axisA;
    Vector3 axisB;
    Vector2[] uvs;
    GameObject[] Tiles;
    Transform transform;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }
    public void Start()
    {
        //if (transform.childCount != 0) { Destroy(); } //else if (transform.childCount == 0) { ConstructMesh(); }
        Destroy();
        //ConstructMesh();
    }
    public void Destroy()
    {
        GameObject planet = GameObject.Find("Planet");
        Planet pscript = planet.GetComponent<Planet>();
        transform = pscript.meshObj.transform;
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
            if (transform.childCount == resolution - 1 * resolution - 1 ) { break; }
        }
        //ConstructMesh();

    }
    public void OnValidate()
    {


        if (gameObject.transform.IsChildOf(gameObject.transform) == true)
        {
            Planet host = gameObject.GetComponentInParent<Planet>();
            resolution = host.resolution;
            ChunkRes = host.ChunkRes;
            if (host != null)
            {

                //transform = host.transform;
                ConstructMesh();
            }
        }
    }

    public void ConstructMesh()
    {
        //mesh = new Mesh();
        if (mesh == null) { mesh = new Mesh(); }
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        uvs = new Vector2[vertices.Length];

    //    Planet host = GameObject.Find("Planet").GetComponent<Planet>();
    //    resolution = host.resolution;
    //    ChunkRes = host.ChunkRes;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;
                uvs[i] = new Vector2((float)x / resolution, (float)y / resolution);


                GameObject planet = GameObject.Find("Planet");
                Planet pscript = planet.GetComponent<Planet>();
                transform = pscript.meshObj.transform;
                Tiles = new GameObject[vertices.Length];
                if (x != resolution && y != resolution)
                {
                    if (vertices[i] != Vector3.zero)
                    {
                        childVertex = vertices[i];
                        GameObject Tile = new GameObject("Tile");
                    //    Tile.transform.position = vertices[i];
                    //    Tile.transform.localPosition = vertices[i];
                        Tile.transform.parent = transform;

                        Tile.AddComponent<Chunk>();
                        Tile.GetComponent<Chunk>().localUp = localUp;
                        Tile.GetComponent<Chunk>().parentVertex = vertices[i];
                        Tile.GetComponent<Chunk>().ChunkRes = ChunkRes;
                        Tile.GetComponent<Chunk>().parentResolution = resolution - 1;
                        Tile.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
                        Tiles[i] = Tile;
                    //    Tiles[i].transform.position = vertices[i];
                    //    Tiles[i].transform.localPosition = vertices[i];
                    }

                    
                    //Debug.Log(gameObject.transform);
                    //Tile.transform.SetParent(gameObject.transform);
                    //GameObject.Find("Tile").GetComponent<Chunk>().ChunkRes = ChunkRes;
                }

                //Tiles[i].transform.parent = transform;


                if (x != resolution - 1 && y != resolution - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution + 1;
                        triangles[triIndex + 2] = i + resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution + 1;
                        triIndex += 6;
                    }

                
            }
        }
        mesh.Clear();  
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
    }
}