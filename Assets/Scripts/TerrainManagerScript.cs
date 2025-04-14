using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    public GameObject terrainPiece;
    public float deltaX;
    public float terrainLength = 20F;
    public float terrainVertexDensity = 2F;
    private float prevX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving) {
            deltaX += gameObject.transform.position.x - prevX;
            prevX = gameObject.transform.position.x;
        }   
        if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving && deltaX > terrainLength) {
            deltaX = 0;

            //instantiate new object
            Instantiate(terrainPiece, gameObject.transform.position, gameObject.transform.rotation, GameObject.Find("TerrainPieces").transform);
            /*
            HVertices = new float[terrainLength];
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
                //add this height, next height, this base
                
                // make sub tri
                //add next height, next base, this base
            }
            SetTriangles(int[] triangles, 0, true, 0); 
            */

        }
        
    }

    public void Cleanup(float beginX, bool isLeft) {
        
    }
}
