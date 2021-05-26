using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{
    public static readonly int ChunkWidth = 5;
    public static readonly int ChunkHeigth = 5;

    public static readonly Vector3[] VerticesArr = new Vector3[8]
    {
       new Vector3(0,0,0), //0
       new Vector3(1,0,0), //1
       new Vector3(1,1,0), //2
       new Vector3(0,1,0), //3
       new Vector3(0,0,1), //4
       new Vector3(0,0,0), //5
       new Vector3(1,1,1), //6
       new Vector3(0,1,1), //7
    };

    public static readonly int[][] TriangelsArr = new int[6][]
    {
        new int [6] {0, 3, 1, 1, 3, 2}, //back
        new int [6] {5, 6, 4, 4, 6, 7}, //front
        new int [6] {3, 7, 2, 2, 7, 6}, //top
        new int [6] {1, 5, 0, 0, 5, 4}, //bottom
        new int [6] {4, 7, 0, 0, 7, 3}, //left
        new int [6] {1, 2, 5, 5, 2, 6} //right  
    };
}
