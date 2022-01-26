using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SpaceShipController Player;
    public CameraScript mainCam;
    public GameObject StartPlane;
    [Space]
    public GameObject GameScene;
    public GameObject PreparationScene;
    [Space]
    public Slider CargoSlider;
    public Slider FuelSlider;
    public Slider BulletSlider;
    public Text TotalCargoShower;
    [Space]
    public GameObject[] Route1;
    public GameObject[] Route2;
    public GameObject[] Route3;
    public Transform[] EnemySpawns;
    public GameObject Enemy;
    [Space]
    public string CurrentScene;
    public string NextScene;
    [Space]
    public Text FinishText;
    public Text FuelShower;
    public Text BulletShower;
    public Text CargoShower;
    public Image EndingImage;
    [Space]
    public float EndGameTime;
    private float EndGameTimer;


    private GameObject[] Bullets;

    public void ActivateRoute1()
    {
        for(int i = 0; i < Route1.Length; i++)
        {
            Route1[i].SetActive(true);
        }
    }

    public void ActivateRoute2()
    {
        for (int i = 0; i < Route2.Length; i++)
        {
            Route2[i].SetActive(true);
        }
    }

    public void ActivateRoute3()
    {
        for (int i = 0; i < Route3.Length; i++)
        {
            Route3[i].SetActive(true);
        }
    }

    public void DisableRoute1()
    {
        for (int i = 0; i < Route1.Length; i++)
        {
            Route1[i].SetActive(false);
        }
    }

    public void DisableRoute2()
    {
        for (int i = 0; i < Route2.Length; i++)
        {
            Route2[i].SetActive(false);
        }
    }

    public void DisableRoute3()
    {
        for (int i = 0; i < Route3.Length; i++)
        {
            Route3[i].SetActive(false);
        }
    }

    public void EndGame()
    {
        FinishText.gameObject.SetActive(true);
        EndGameTimer = EndGameTime;
        EndingImage.color = new Color(EndingImage.color.r, EndingImage.color.b, EndingImage.color.g, 0);

        GlobalData.CargoCarried += Player.Cargo;
        GlobalData.RouteNumber++;
    }

    public void StartGame()
    {
        GameScene.SetActive(true);
        if (GlobalData.RouteNumber == 1)
        {
            DisableRoute2();
            DisableRoute3();
        }
        else if (GlobalData.RouteNumber == 2)
        {
            DisableRoute1();
            DisableRoute3();
        }
        else if (GlobalData.RouteNumber == 3)
        {
            DisableRoute2();
            DisableRoute1();
        }

        PreparationScene.SetActive(false);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceShipController>();
        mainCam.Target = Player.transform;

        Player.Cargo = CargoSlider.value;
        Player.Bullets = BulletSlider.value;
        Player.Fuel = FuelSlider.value;

        //generate enemies
        if (GlobalData.RemainedEnemies > 0)
        {
            for (int i = 0; i < EnemySpawns.Length && i < GlobalData.RemainedEnemies; i++)
            {
                Instantiate(Enemy, EnemySpawns[i].position, EnemySpawns[i].rotation);
            }
        }
    }

    public void RefreshBulletList()
    {
        Bullets = GameObject.FindGameObjectsWithTag("Bullet");
    }

    private void Awake()
    {
        GlobalData.gameManager = GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FinishText.gameObject.SetActive(false);
        EndGameTimer = 999;

        //Player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceShipController>();
        mainCam = Camera.main.GetComponent<CameraScript>();
        mainCam.Target = StartPlane.transform;

        TotalCargoShower.text = "Cargo Carried: " + GlobalData.CargoCarried + " t";

        RefreshBulletList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(CurrentScene);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (EndGameTimer != 999)
        {
            if (EndGameTimer > 0)
            {
                EndGameTimer -= Time.deltaTime;
                EndingImage.color = new Color(EndingImage.color.r, EndingImage.color.b, EndingImage.color.g, (EndGameTime-EndGameTimer)/EndGameTime);
            }
            else
            {
                if (GlobalData.RouteNumber > 3 && GlobalData.CargoCarried >= 10)//Victorious
                {
                    SceneManager.LoadScene("Victorious");
                }
                else if (GlobalData.RouteNumber > 3 && GlobalData.CargoCarried <= 10)//Fail
                {
                    SceneManager.LoadScene("Failed");
                }
                else
                SceneManager.LoadScene(NextScene);
            }
        }

        

        if (PreparationScene.activeInHierarchy)
        {
            CargoShower.text = "Cargo Mass " + ((int)(CargoSlider.value * 10)) / 10.0f + " t";
            FuelShower.text = "Fuel " + ((int)(FuelSlider.value * 10)) / 10.0f + " t";
            BulletShower.text = "Laser Battery " + ((int)(BulletSlider.value * 10)) / 10.0f + " t";
        }
        else
        {
            CargoShower.text = "Cargo Mass " + ((int)(Player.Cargo * 10)) / 10.0f + " t";
            FuelShower.text = "Fuel " + ((int)(Player.Fuel * 10)) / 10.0f + " t";
            BulletShower.text = "Battery " + ((int)(Player.Bullets * 10)) / 10.0f + " t";

            for(int i = 0; i<Bullets.Length; i++)
            {
                if(Bullets[i])
                Bullets[i].transform.position = new Vector3(0, Bullets[i].transform.position.y, Bullets[i].transform.position.z);
            }
        }
    }
}



class GlobalData
{
    public static float CargoCarried = 0;
    public static float RemainedEnemies = 0;
    public static int RouteNumber = 1;

    public static GameManager gameManager;
}