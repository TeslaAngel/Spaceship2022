using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public float Power;
    public float AngularOffset;
    public float InitialMass;
    public float Fuel;
    public float Cargo;
    [Space]
    public float Bullets;
    public float GunPower;
    public GameObject Gun;
    public Transform AimAssistant;
    [Space]
    public GameObject Thurst;

    public Rigidbody body;

    private GameManager gameManager;

    void CalculateMass()
    {
        body.mass = InitialMass + Cargo + Fuel + Bullets;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if(Cargo>0) //delete a cargo
                Cargo--;
            CalculateMass();
            Destroy(collision.gameObject);
            gameManager.RefreshBulletList();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        CalculateMass();
        Gun.SetActive(false);

        gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && Fuel > 0)
        {
            body.AddRelativeForce(0, 0, Power*Time.deltaTime);
            Thurst.SetActive(true);

            Fuel -= 0.05f * Time.deltaTime;
        }
        else
        {
            Thurst.SetActive(false);
        }
        /*
        if (Input.GetKey(KeyCode.S))
        {
            body.AddRelativeForce(0, 0, -Power * Time.deltaTime);
        }
        */
        
        if (Input.GetKey(KeyCode.A))
        {
            body.AddRelativeTorque(0,0, Power * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.AddRelativeTorque(0,0, -Power * Time.deltaTime);
        }
        

        Ray Mray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Mray, out hit))
        {
            Vector3 LookTarget = new Vector3(0, hit.point.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookTarget - transform.position), AngularOffset * Time.deltaTime);

            if (Input.GetMouseButton(0) && Bullets > 0)
            {
                Gun.SetActive(true);
                //Gun.transform.rotation = Quaternion.LookRotation(LookTarget - transform.position);
                Vector3 Aim = new Vector3(0, AimAssistant.position.y, AimAssistant.position.z);
                Vector3 From = new Vector3(0, transform.position.y, transform.position.z);
                Gun.transform.rotation = Quaternion.LookRotation(Aim - transform.position);
                Bullets -= 0.1f*Time.deltaTime;
                body.AddRelativeForce(0, 0, -GunPower * Time.deltaTime);
            }
            else
            {
                Gun.SetActive(false);
            }
        }

        CalculateMass();
    }
}
