using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Transform cam;
    Vector2 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthesBack;

    [Range(0.01f, 0.6f)]

    public float parallaxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int BackCount = transform.childCount;
        mat = new Material[BackCount];
        backSpeed = new float[BackCount];
        backgrounds = new GameObject[BackCount];

        for(int i = 0; i < BackCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        backSpeedCalculate(BackCount);
    }

    void backSpeedCalculate(int Backcount)
    {
        for(int i = 0;i < backSpeed.Length;i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthesBack)
            {
                farthesBack = backgrounds[i].transform.position.z - cam.position.z;
            }               
        }  
        
        for(int i = 0; i < Backcount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthesBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for(int i = 0;i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
