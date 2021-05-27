using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    GameObject chunkObject;
    Mesh mesh;

    int vertexIndex = 0; //Gibt den aktuellen Vektorplatzt an (im Bezug auf den Chunk context)

    

    List<int> Triangles = new List<int>();
    List<Vector3> Vertices = new List<Vector3>();


    bool[,,] voxelMap = new bool[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
    
    public Chunk(World _world)
    {
        World world = _world;

        CreateChunkAndItsMesh(world);
        PopulateVoxelMap();

        GenerateNewChunk(world.transform.position);
        CreateMesh();
        
    }

    void PopulateVoxelMap()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    voxelMap[x, y, z] = true;
                }
            }
        }
    }

    bool CheckVoxel (Vector3 pos) //Gibt true wenn ein block solid ist!
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth - 1)//Überprüft ob es sich um einen Block außerhalb der Begrenzung handelt(nicht existent:0)
            //Folglich muss dieser nicht solid sein da er nicht existiert
        {
            return false;
        }

        return voxelMap[x,y,z];
    }

    void CreateChunkAndItsMesh(World world)
    {
        chunkObject = new GameObject();
        chunkObject.transform.SetParent(world.transform);
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
    }

    void GenerateNewVoxel(Vector3 pos)
    {
        
        
        for (int p = 0; p < 6; p++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p])) //Es wird überprüft ob nachbar Block solid ist und wenn nein wird die entsprechende Seite generiert!
            {
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][0]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][1]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][2]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][3]]);
                Triangles.AddRange(new int[6] { vertexIndex, vertexIndex + 1, vertexIndex + 2, vertexIndex + 2, vertexIndex + 1, vertexIndex + 3 });
                vertexIndex += 4;

            }
        }

       
    }

    void GenerateNewChunk(Vector3 worldPos)
    {
        for (int y=0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    if (voxelMap[x,y,z]) 
                        GenerateNewVoxel(worldPos + new Vector3(x, y, z));
                }
            }
        } 
    }

    void CreateMesh()
    {
        mesh = new Mesh();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}
