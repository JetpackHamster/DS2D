using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowballScript : MonoBehaviour
{
    public GameObject self;
    private float scale;
    private float nubmer;

    // Start is called before the first frame update
    void Start()
    {
        scale = self.transform.localScale.x - Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        nubmer = (1F + 0.05F * Time.deltaTime);
        scale /= nubmer;
        GetComponent<Rigidbody2D>().mass = scale / 2;
        self.transform.localScale = new Vector3(scale, scale, scale);

        if (Input.GetKey(KeyCode.Minus) && Random.value > 0.99F || scale < 0.05F)
        {
            Destroy(gameObject);
        }
        if (Input.GetKey(KeyCode.Plus) && Random.value > 0.95F)
        {
            Instantiate(self, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        }
    }
}
