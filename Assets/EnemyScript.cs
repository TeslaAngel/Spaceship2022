using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float Health;
    [Space]
    public float AimmingOffset;
    public float ShootingInterval;
    private float ShootingTimer;
    public GameObject Bullet;
    public Transform GunPoint;
    [Space]
    public float ThurstForce;
    public GameObject Thurst;

    private GameObject player;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GlobalData.RemainedEnemies++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 0)
        {
            //transform.LookAt(player.transform);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), AimmingOffset * Time.deltaTime);
            //myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            GetComponent<Rigidbody>().AddRelativeForce(0, 0, ThurstForce*Time.deltaTime);

            if(Vector3.Distance(transform.position, player.transform.position) <= 20f && ShootingTimer<=0)
            {
                GameObject Projectile = Instantiate(Bullet, GunPoint.position, GunPoint.rotation);
                Projectile.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 3000f);
                gameManager.RefreshBulletList();
                ShootingTimer = ShootingInterval;
            }

            if (ShootingTimer > 0)
            {
                ShootingTimer -= Time.deltaTime;
            }

            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            Thurst.SetActive(true);
        }
        else
        {
            GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezeRotation;
            Thurst.SetActive(false);
            GlobalData.RemainedEnemies--;
            Destroy(GetComponent<EnemyScript>());
        }
    }
}
