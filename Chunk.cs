using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    Mesh chunk;
    public int ChunkRes = 1;
    public int parentResolution = 1;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    public Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    public Vector3 parentVertex;
    public float rando;


    public void Start()
    {
        if (gameObject.transform.IsChildOf(gameObject.transform) == true)
        {
            Base script = gameObject.GetComponentInParent<Base>();
            if (script != null)
            {
                ChunkRes = gameObject.transform.parent.GetComponent<Base>().ChunkRes;
                parentResolution = gameObject.transform.parent.GetComponent<Base>().resolution;
            }

            TerrainFace tf = gameObject.GetComponentInParent<TerrainFace>();
            if (tf != null)
            {
                localUp = gameObject.transform.parent.GetComponent<TerrainFace>().localUp;
                ChunkRes = gameObject.transform.parent.GetComponent<TerrainFace>().ChunkRes;
                parentResolution = gameObject.transform.parent.GetComponent<TerrainFace>().resolution;
                // parentResolution = tf.resolution ;
            }
        }

        CreateMesh();
    }
    public void OnValidate()
    {
        if (gameObject.transform.IsChildOf(gameObject.transform) == true)
        {
            Base script = gameObject.GetComponentInParent<Base>();
            if (script != null)
            {
                ChunkRes = gameObject.transform.parent.GetComponent<Base>().ChunkRes;
                parentResolution = gameObject.transform.parent.GetComponent<Base>().resolution;
            }

            TerrainFace tf = gameObject.GetComponentInParent<TerrainFace>();
            if (tf != null)
            {
                parentVertex = gameObject.transform.parent.GetComponent<TerrainFace>().childVertex;
                localUp = gameObject.transform.parent.GetComponent<TerrainFace>().localUp;
                ChunkRes = gameObject.transform.parent.GetComponent<TerrainFace>().ChunkRes;
                parentResolution = gameObject.transform.parent.GetComponent<TerrainFace>().resolution;
                // parentResolution = tf.resolution ;
            }
        }

        CreateMesh();
    }

    public void CreateMesh()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        chunk = new Mesh();
        GetComponent<MeshFilter>().mesh = chunk;
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        vertices = new Vector3[(ChunkRes + 1) * (ChunkRes + 1)];
        triangles = new int[ChunkRes * ChunkRes * 6];
        uvs = new Vector2[vertices.Length];
       // localUp = parentVertex;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
        if (parentVertex != null)
        {
            for (int i = 0, x = 0; x <= ChunkRes; x++)
            {
                for (int y = 0; y <= ChunkRes; y++)
                {
                    if (parentVertex != Vector3.zero)
                    {
                        //float z = Mathf.PerlinNoise(x * .3f, y * .3f) * 2f;
                        rando = 1.75f;
                        Vector2 percent = new Vector2(x, y) / (ChunkRes - 1);
                        Vector3 center = GameObject.Find("Planet").transform.position;
                        Vector3 pointOnCube = (localUp - localUp + parentVertex + ((((percent.x) * 2 * axisA + (percent.y) * 2 * axisB) / ChunkRes) / parentResolution) * rando);
                        vertices[i] = pointOnCube.normalized;
                        // vertices[i] =(((localUp + new Vector3(x, 0, y) / ChunkRes )/ parentResolution )   * rando     );
                        uvs[i] = new Vector2(x, y);
                        i++;
                    }
                }
            }
        }

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < ChunkRes; x++)
        {
            for (int y = 0; y < ChunkRes; y++)
            {
                //        triangles[tris + 0] = vert;
                //        triangles[tris + 1] = vert + 1;
                //        triangles[tris + 2] = vert + ChunkRes + 1;
                //        triangles[tris + 3] = vert + 1;
                //        triangles[tris + 4] = vert + ChunkRes + 2;
                //        triangles[tris + 5] = vert + ChunkRes + 1;

                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + ChunkRes + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + ChunkRes + 1;
                triangles[tris + 5] = vert + ChunkRes + 2;


                vert++;
                tris += 6;
            }
            vert++;
        }
        chunk.Clear();
        chunk.vertices = vertices;
        chunk.triangles = triangles;
        chunk.uv = uvs;
        chunk.RecalculateNormals();
    }
}