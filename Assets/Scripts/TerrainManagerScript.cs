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
    public GameObject[] spawnedObjs;
    private float tradeStructureTimer;
    public float tradeStructureSpawnRate;
    public GameObject[] structures;
    public GameObject newPiece;
    public GameObject newScrap;
    public float terrainSpiciness;
    public float terrainOffset; // start value in editor should be ~200? // randomize on newGame
    
    
    // Start is called before the first frame update
    void Start()
    {
        //generatePiece();
        cam = GameObject.Find("Main Camera");
        tradeStructureTimer = 0F;
        //terrainOffset = 
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.transform.position.x > gameObject.transform.position.x - 150){ // move if need
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + terrainLength - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            //terrainSpiciness += 0.1F;
            generatePiece();

            // spawn scrap
            while (Random.Range(0F, 10F) > 7F) {
                newScrap = Instantiate(spawnedObjs[Random.Range(0,spawnedObjs.Length)], new Vector3(transform.position.x + Random.Range(-0.5F * terrainLength, 0.5F * terrainLength), transform.position.y + 10, 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
                newScrap.GetComponent<ScrapScript>().value *= Random.Range(0.6F,1.4F);
            }

            
            if (tradeStructureTimer < tradeStructureSpawnRate) {
                tradeStructureTimer += terrainLength;
                
            // if far enough from prev. structure
            } else {
                
                float prevH = 0F;
                int count = 0;
                for(int i = 0; i < HVertices.Length; i++) {
                    if (HVertices[i].y == prevH) { // if same, add to count, if count high, isLevel true
                        count++;
                        if(count > 3 * terrainVertexDensity) {
                            // reset timer and make structure
                            tradeStructureTimer = 0F;
                            //Debug.Log("structure make!");
                            Instantiate(structures[0], new Vector3((transform.position.x + (i - 3) / terrainVertexDensity) + 5, transform.position.y + prevH/10 + 1.4F, 0), new Quaternion(0F, 0F, 0F, 1F), newPiece.transform);
                            i = HVertices.Length - 1; // exit loop
                        }
                    }
                    prevH = HVertices[i].y;
                }
            }

            if (structures.Length > 1) {
                // spawn other structures
                while (Random.Range(0F, 10F) > 7F) {
                    Instantiate(structures[Random.Range(1,structures.Length)], new Vector3(transform.position.x + Random.Range(-0.5F * terrainLength, 0.5F * terrainLength), transform.position.y + 10/*<- TODO: terrain y at closest*/, 0), /*constrained rotation->*/new Quaternion(0F, 0F, Random.Range(-0.3F, 0.3F), 1F));
                }
            }
        }
        /*if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving) {
            deltaX += gameObject.transform.position.x - prevX;
            prevX = gameObject.transform.position.x;
        }   
        if (gameObject.transform.parent.GetComponent<TerrainCircleSpawnerScript>().moving && deltaX > (terrainLength * 0.93F)) {
            deltaX = 0;
            //generatePiece();

        }*/
        
    }

    //public void Cleanup(float beginX, bool isLeft) {
        
    //}
    void generatePiece() {

        // instantiate new object
        newPiece = Instantiate(terrainPiece, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 80, gameObject.transform.position.z), gameObject.transform.rotation, GameObject.Find("TerrainPieces").transform);
        
        
        // generate list of heights
        for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
            float perlinput = (gameObject.transform.position.x + (i / (terrainVertexDensity))) / (terrainLength * 0.3F) + terrainOffset;
            //Debug.Log("perlinput " + perlinput);
            HVertices[i] = new Vector3(((i / terrainVertexDensity)), (Mathf.PerlinNoise1D(perlinput) * terrainSpiciness + 80), 0); // make heights into vertices
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
