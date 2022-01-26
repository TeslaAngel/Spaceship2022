using UnityEngine;
using System.Collections;

public class XLine : MonoBehaviour {

    private GameManager gameManager;

	public GameObject Line;
	public GameObject FXef;//激光击中物体的粒子效果
                           // Use this for initialization
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update () {
		RaycastHit hit;
		Vector3 Sc;// 变换大小
		Sc.x=0.5f;
		Sc.z=0.5f;
		//发射射线，通过获取射线碰撞后返回的距离来变换激光模型的y轴上的值
        if (Physics.Raycast(transform.position, this.transform.forward, out hit)){
			Debug.DrawLine(this.transform.position,hit.point);
			Sc.y=hit.distance;
			FXef.transform.position=hit.point;//让激光击中物体的粒子效果的空间位置与射线碰撞的点的空间位置保持一致；
			FXef.SetActive(true);

            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(85f * Time.deltaTime, transform.position, 5);
            }

            if (hit.collider.gameObject.GetComponent<EnemyScript>())
            {
                hit.collider.gameObject.GetComponent<EnemyScript>().Health -= Time.deltaTime;
                
            }
            if (hit.collider.gameObject.GetComponent<ShieldCoreScript>())
            {
                hit.collider.gameObject.GetComponent<ShieldCoreScript>().Health -= Time.deltaTime;
                hit.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(85f * Time.deltaTime, transform.position, 5);
            }
            if (hit.collider.gameObject.tag == "Bullet")
            {
                Destroy(hit.collider.gameObject);
                gameManager.RefreshBulletList();
            }

        }
		//当激光没有碰撞到物体时，让射线的长度保持为50m，并设置击中效果为不显示
		else{
			Sc.y=50;
		    FXef.SetActive(false);
		}
			
		Line.transform.localScale=Sc;
            
	}
}
