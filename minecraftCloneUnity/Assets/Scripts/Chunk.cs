using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    GameObject chunkObject;
    Mesh mesh;
    
    public Chunk(World _world)
    {
        World world = _world;
        chunkObject = new GameObject();
        chunkObject.transform.SetParent(world.transform);
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshFilter = chunkObject.AddComponent<MeshFilter>();

        GenerateChunk();
        meshFilter.mesh = mesh;
    }

    void GenerateChunk()
    {
        mesh = new Mesh();
        mesh.vertices = VoxelData.VerticesArr;

        mesh.triangles = VoxelData.TriangelsArr[0];
        mesh.RecalculateNormals();
    }
}
