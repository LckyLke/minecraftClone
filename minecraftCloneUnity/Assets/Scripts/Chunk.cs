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
    List<Vector2> uvs = new List<Vector2>();
    World world;

    byte[,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
    
    public Chunk(World _world)
    {
        world = _world;

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
                    voxelMap[x, y, z] = 2;
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
                                                                                                                                     //Folglich ist dieser nicht solid da er nicht existiert
        {
            return false;
        }

        return world.blockTypes[voxelMap[x,y,z]].isSolid;
    }

    void CreateChunkAndItsMesh(World world)
    {
        chunkObject = new GameObject();
        chunkObject.transform.SetParent(world.transform);
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer.material = world.material;
    }

    void GenerateNewVoxel(Vector3 pos)
    {
        
        
        for (int p = 0; p < 6; p++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p])) //Es wird überprüft ob nachbar Block solid ist und wenn nein wird die entsprechende Seite generiert!
            {
                byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][0]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][1]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][2]]);
                Vertices.Add(pos + VoxelData.VerticesArr[VoxelData.TriangelsArr[p][3]]);
                AddTexture(world.blockTypes[blockID].GetTextureId(p));
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
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    void AddTexture (int textureID)
    {
        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;
        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}
