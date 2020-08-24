using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;

    public int textureWidth = 1024;
    public int textureHeight = 1024;

    public float noise01Scale = 2f;
    public float noise01Amp = 2f;

    public float noise02Scale = 4f;
    public float noise02Amp = 4f;

    public float noise03Scale = 6f;
    public float noise03Amp = 6f;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    // Start is called before the first frame update
    void Start () {
        mesh = new Mesh ();
        GetComponent<MeshFilter> ().mesh = mesh;
        CreateShape ();
        UpdateMesh ();
    }
    void CreateShape () {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float y = Mathf.PerlinNoise (x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3 (x, y, z);
                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                else if (y < minTerrainHeight)
                    minTerrainHeight = y;
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + xSize + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + xSize + 1;
                triangles[5 + tris] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float height = Mathf.InverseLerp (minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate (height);
                i++;
            }
        }
    }

    void UpdateMesh () {
        mesh.Clear ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals ();
    }

    // private void OnDrawGizmos () {
    //     if (vertices == null) return;
    //     for (int i = 0; i < vertices.Length; i++) {
    //         Gizmos.DrawSphere (vertices[i], .1f);
    //     }
    // }
}