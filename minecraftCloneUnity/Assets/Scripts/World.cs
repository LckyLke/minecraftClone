using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Chunk newChunk = new Chunk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
