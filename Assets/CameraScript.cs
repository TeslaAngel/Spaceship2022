using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Target;
    public float LerpOffset;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + Offset, LerpOffset * Time.deltaTime);

    }
}
