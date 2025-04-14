using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if need new terrain piece
            instantiate new object
            
            HVertices = new float[100];
            generate list of heights
            for(int i = 0; i < 100; i++) {
                HVertices[i] = Mathf.PerlinNoise1D(i/10);
            }
            make heights into vertices (in array in object)
            make base vertex below each height vertex (in array in object)
            
            Mesh.SetVertices( array of Vector3s );
            
            assign triangles array (in array in object)
            for each height if has next {
                // make this tri
                add this height, next height, this base
                
                // make sub tri
                add next height, next base, this base
            }
            SetTriangles(int[] triangles, 0, true, 0); 



        */
    }

    public void Cleanup(float beginX, bool isLeft) {
        
    }
}
