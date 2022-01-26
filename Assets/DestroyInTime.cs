using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float LifeTime;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        Timer = LifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(gameObject);
            GlobalData.gameManager.RefreshBulletList();
        }
    }
}
