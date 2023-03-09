using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPointer : MonoBehaviour
{

    private Transform playeTransform;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playeTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playeTransform.position) <= 1.5f)
        {
            Destroy(gameObject);
        }  
    }
}
