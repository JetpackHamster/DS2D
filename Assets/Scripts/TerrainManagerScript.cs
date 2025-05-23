using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    public GameObject terrainPiece;
    public float deltaX;
    public static float terrainLength = 80F; // TODO: consider changing to 100
    public static float terrainVertexDensity = 1F;
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
    public float[] loadedLimits; // stores a float for x value of furthest left loaded terrain, and furthest right respectively
    public bool isNewTerrain;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //generatePiece();
        cam = GameObject.Find("Main Camera");
        tradeStructureTimer = 0F;
        loadedLimits = new float[2];
        loadedLimits[0] = -10F * terrainLength;
        loadedLimits[1] = 10F * terrainLength;
        /*if (isNewTerrain) {
            
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.transform.position.x > gameObject.transform.position.x - 150){ // move forward if camera close
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + terrainLength - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            //terrainSpiciness += 0.1F;
            generatePiece(transform.position.x);

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
                            Instantiate(structures[0], new Vector3((transform.position.x + (i - 3) / terrainVertexDensity) + 5, transform.position.y + prevH/10 + 1F, 0), new Quaternion(0F, 0F, 0F, 1F), newPiece.transform);
                            i = HVertices.Length - 1; // exit loop
                        }
                    }
                    prevH = HVertices[i].y;
                }
            }

            if (structures.Length > 1) {
                // spawn other structures // TODO: fix; y was at 12 ish when should be at -16 ish with - 50 offset in below big code line
                while (Random.Range(0F, 10F) > 7F) {
                    float structX = Random.Range(-0.5F * terrainLength, 0.5F * terrainLength);
                    Instantiate(structures[Random.Range(1,structures.Length)], new Vector3(transform.position.x + structX, transform.position.y - 50 + HVertices[Mathf.RoundToInt(structX / terrainVertexDensity)].y/*<- TODO: terrain y at closest*/, 0), /*constrained rotation->*/new Quaternion(0F, 0F, Random.Range(-0.3F, 0.3F), 1F));
                }
            }
        }
    }

    // generate a new terrain piece
    void generatePiece(float posX) {

        // instantiate new object
        newPiece = Instantiate(terrainPiece, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 80, gameObject.transform.position.z), gameObject.transform.rotation, GameObject.Find("TerrainPieces").transform);
        
        
        // generate list of heights
        for(int i = 0; i < terrainLength * terrainVertexDensity; i++) {
            // calculate perlin noise input value
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
        // combine vertex arrays
        allVertices = HVertices;
        UnityEditor.ArrayUtility.AddRange(ref allVertices, bVertices);
        
        Mesh tMesh = new Mesh();
        tMesh.SetVertices(allVertices);
        
        // assign triangles array
        //for each height if has next
        for (int i = 0; i < terrainLength * terrainVertexDensity - 1; i++){
            // make this (surface) tri
            //add this height, next height, this base
            //UnityEditor.ArrayUtility.Add(ref triangles, );
            UnityEditor.ArrayUtility.Add(ref triangles, i);
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + (int)(terrainLength * terrainVertexDensity)));
            
            // make sub (base) tri
            //add next height, next base, this base
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 1 + (int)(terrainLength * terrainVertexDensity)));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + (int)(terrainLength * terrainVertexDensity)));
            
        }
        

        tMesh.SetTriangles(triangles, 0, true, 0); 
        tMesh.RecalculateBounds();
        newPiece.GetComponent<MeshFilter>().sharedMesh = tMesh; // assign new mesh as the mesh of the new piece

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
