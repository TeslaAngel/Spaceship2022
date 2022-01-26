using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCoreScript : MonoBehaviour
{
    public float Health;
    public FixedJoint joint;
    public GameObject Shield;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(joint);
            //Shield.GetComponent<MeshRenderer>().material.SetColor("Color_83E49A57", new Color(44, 200, 36));
            Shield.GetComponent<MeshRenderer>().material.SetFloat("Vector1_FF5ECB89", 0.05f);
            Destroy(Shield.GetComponent<SphereCollider>());

            Destroy(GetComponent<ShieldCoreScript>());
        }
    }
}
