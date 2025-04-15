using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    public GameObject terrainPiece;
    public float deltaX;
    public static float terrainLength = 80F;
    public static float terrainVertexDensity = 1F;
    private float prevX;
    private Vector3[] HVertices = new Vector3[(int)(terrainLength * terrainVertexDensity)];
    private Vector3[] bVertices = new Vector3[(int)(terrainLength * terrainVertexDensity)];
    private Vector3[] allVertices;
    private int[] triangles = new int[0];// = new int[(int)(terrainLength * terrainVertexDensity * 6)];
    private GameObject cam;
    
    // Start is called before the first frame update
    void Start()
    {
        //generatePiece();
        cam = GameObject.Find("Main Camera");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.transform.position.x > gameObject.transform.position.x - 150){ // move if need
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + terrainLength - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            generatePiece();
        }
        if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving) {
            deltaX += gameObject.transform.position.x - prevX;
            prevX = gameObject.transform.position.x;
        }   
        if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving && deltaX > (terrainLength * 0.93F)) {
            deltaX = 0;
            //generatePiece();

        }
        
    }

    //public void Cleanup(float beginX, bool isLeft) {
        
    //}
    void generatePiece() {

        // instantiate new object
        GameObject newPiece = Instantiate(terrainPiece, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 80, gameObject.transform.position.z), gameObject.transform.rotation, GameObject.Find("TerrainPieces").transform);
        
        
        // generate list of heights
        for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
            float perlinput = (gameObject.transform.position.x + (i / (terrainVertexDensity))) / (terrainLength * 0.3F);
            Debug.Log("perlinput " + perlinput);
            HVertices[i] = new Vector3(((i / terrainVertexDensity)), (Mathf.PerlinNoise1D(perlinput) * 10 + 80), 0); // make heights into vertices
        }
        /*for (int i = 0; i < 10; i++) {
            Debug.Log("HVertices[" + i + "]: " + HVertices[i]);
        }*/
        // make base vertex below each height vertex
        for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
            bVertices[i] = new Vector3(HVertices[i].x, -10, 0); // make base vertices
        }
        allVertices = HVertices;
        UnityEditor.ArrayUtility.AddRange(ref allVertices, bVertices);
        
        Mesh tMesh = new Mesh();
        tMesh.SetVertices(allVertices);
        
        // assign triangles array
        //for each height if has next
        for (int i = 0; i < terrainLength * terrainVertexDensity - 1; i++){
            // make this tri
            //add this height, next height, this base
            //UnityEditor.ArrayUtility.Add(ref triangles, );
            UnityEditor.ArrayUtility.Add(ref triangles, i);
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + (int)(terrainLength * terrainVertexDensity)));
            
            // make sub tri
            //add next height, next base, this base
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1 + (int)(terrainLength * terrainVertexDensity)));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + (int)(terrainLength * terrainVertexDensity)));
            
        }
        

        tMesh.SetTriangles(triangles, 0, true, 0); 
        tMesh.RecalculateBounds();
        newPiece.GetComponent<MeshFilter>().sharedMesh = tMesh;

        Vector2[] allVertices2D = new Vector2[allVertices.Length];
        for (int i = 0; i < allVertices.Length; i++){
            allVertices2D[i] = new Vector2(allVertices[i].x, allVertices[i].y);
        }
        newPiece.GetComponent<PolygonCollider2D>().points = allVertices2D;

        //Vector2[] pathVertices2D = new Vector2[allVertices.Length];
        //for (int i = 0; i < allVertices.Length; i++){
        //    allVertices2D[i] = new Vector2(allVertices[i].x, allVertices[i].y);
        //}
        newPiece.GetComponent<PolygonCollider2D>().SetPath(0, allVertices2D);
    }
}
