using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    public GameObject terrainPiece;
    public float deltaX;
    public static float terrainLength = 20F;
    public static float terrainVertexDensity = 2F;
    private float prevX;
    public Vector3[] HVertices = new Vector3[(int)(terrainLength * terrainVertexDensity)];
    public Vector3[] bVertices = new Vector3[(int)(terrainLength * terrainVertexDensity)];
    public Vector3[] allVertices;
    public int[] triangles = new int[(int)(terrainLength * terrainVertexDensity * 6)];
    private Mesh tMesh;
    // Start is called before the first frame update
    void Start()
    {
        tMesh = new Mesh();
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

            // instantiate new object
            Instantiate(terrainPiece, gameObject.transform.position, gameObject.transform.rotation, GameObject.Find("TerrainPieces").transform);
            
            
            // generate list of heights
            for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
                HVertices[i] = new Vector3(((i / terrainLength) / terrainVertexDensity), Mathf.PerlinNoise1D(i/10), 0); // make heights into vertices
            }
            
            // make base vertex below each height vertex
            for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
                bVertices[i] = new Vector3(HVertices[i].x, -10, 0); // make base vertices
            }
            allVertices = HVertices;
            UnityEditor.ArrayUtility.AddRange(ref allVertices, bVertices);
            tMesh.SetVertices(allVertices);
            
            // assign triangles array
            //for each height if has next
            for (int i = 0; i < terrainLength * terrainVertexDensity; i++){
                // make this tri
                //add this height, next height, this base
                //UnityEditor.ArrayUtility.Add(ref triangles, );
                triangles[i] = i;
                triangles[i + 1] = (i + 1);
                triangles[i + 2] = (i + (int)(terrainLength * terrainVertexDensity));
                
                // make sub tri
                //add next height, next base, this base
                triangles[i + 3] = (i + 1);
                triangles[i + 4] = (i + 1 + (int)(terrainLength * terrainVertexDensity));
                triangles[i + 5] = (i + (int)(terrainLength * terrainVertexDensity));
                
            }
            tMesh.SetTriangles(triangles, 0, true, 0); 
            gameObject.GetComponent<MeshFilter>().mesh = tMesh;
            //gameObject.GetComponent<MeshRenderer>().mesh = tMesh;
        }
        
    }

    //public void Cleanup(float beginX, bool isLeft) {
        
    //}
}
