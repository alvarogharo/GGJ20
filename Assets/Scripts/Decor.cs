using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Decor : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 position = this.transform.position;
        position[2] = position[1];
        this.transform.position = position;
    }
}
