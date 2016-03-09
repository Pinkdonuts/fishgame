using UnityEngine;
using System.Collections;

public class PlayBandTest : MonoBehaviour
{
	public GameObject sound_water;
	public GameObject sound_get;
	public bool water;
	public GameObject s_w;

	public int[] fish_num;//每種魚抓到的數量

	private string m_ConnectMsg;
	private string m_DataMsg;

	private int dir = 1; 
	private float humidity_num = 0f;
	private float Durability = 1; //耐力值
	private float dryability = 1; //放乾值
	
	public static bool up;//漁網上下(true上false下)
	public static bool net_can;//可以撈魚嗎

	public bool Bucket_s;

	public GameObject cursor_obj ;//鼠標位置

	public GameObject stop; //暫停UI
	private bool stopp = false;
	private float timeff;

	void Start()
	{
		PlayBand.OnConnectResultEvent += ConnectResult2;
		PlayBand.Device1.OnIncomingDataEvent += ReceiveData2;
		PlayBand.StartMouseListener();
		PlayBand.Connect();
		PlayBand.Device1.OnButtonClickedEvent += ButtonClicked;
		ResetMsg();

		fish_num[0] = PlayerPrefs.GetInt("fish1");
		fish_num[1] = PlayerPrefs.GetInt("fish2");
		fish_num[2] = PlayerPrefs.GetInt("fish3");
		fish_num[3] = PlayerPrefs.GetInt("fish4");
		fish_num[4] = PlayerPrefs.GetInt("fish5");
		fish_num[5] = PlayerPrefs.GetInt("fish6");
		fish_num[6] = PlayerPrefs.GetInt("fish7");

		dryability = PlayerPrefs.GetInt("lev")/2f;
		Durability = PlayerPrefs.GetInt("end")/2f;
		humidity_num = -Durability;
		stopp = false;
		up = false;
		net_can = true;
		Bucket_s = false;
		StartCoroutine (hu());
	}
	void OnDestroy()
	{
		PlayBand.OnConnectResultEvent -= ConnectResult2;
		PlayBand.Device1.OnIncomingDataEvent -= ReceiveData2;
		PlayBand.Device1.OnButtonClickedEvent -= ButtonClicked;
	}
	public void ButtonClicked ()
	{
		if(stop.activeSelf==false){
			stopp=true;
			stop.SetActive(true);
			timeff=0.1f;
		}
	}
	void OnGUI()
	{
		//GUI.Label(new Rect(400, 100, 200, 50), m_ConnectMsg);

		//GUI.Label(new Rect(230, 400, 200, 100), m_DataMsg);
	}

	public void ConnectResult2(PlayBandConnectData result)
	{
		if (result.success)
		{
			m_ConnectMsg = "Bluetooth connected to "+result.address;
		}
	}

	public void ReceiveData2(PlayBandData data)
	{
		//float newzz = data.EulerAngles.z / 90 * 30;
		m_DataMsg = string.Format("Acceleration: X={0},Y={1},Z={2}  EulerAngle: X={3},Y={4},Z={5} Rotation:X={6},Y={7},Z{8}",
		                          data.Acceleration.x, data.Acceleration.y, data.Acceleration.z, 
		                          data.EulerAngles.x, data.EulerAngles.y, data.EulerAngles.z,
		                          data.Rotation.eulerAngles.x,data.Rotation.eulerAngles.y,data.Rotation.eulerAngles.z);
		//控制網子上下和旋轉角度
		/*if(!game_god.end&&!game_god.stopp)
		{
			transform.position = new Vector3 (transform.position.x,
		                                  	transform.position.y,
		                                  	Mathf.Clamp (transform.position.z + (data.Acceleration.y * 2), -0.01f, 2));

			transform.eulerAngles = new Vector3 (0, Mathf.Clamp (newzz, 0, 31), 0);
		}*/

		if(game_god.end)
		{
			if (/*Mathf.Abs(data.Acceleration.y)<0.1 && */data.Acceleration.z > 0.4) {
				PlayBand.VibrateOnce (50, 500);
				PlayerPrefs.SetInt("fish1",fish_num[0]);
				PlayerPrefs.SetInt("fish2",fish_num[1]);
				PlayerPrefs.SetInt("fish3",fish_num[2]);
				PlayerPrefs.SetInt("fish4",fish_num[3]);
				PlayerPrefs.SetInt("fish5",fish_num[4]);
				PlayerPrefs.SetInt("fish6",fish_num[5]);
				PlayerPrefs.SetInt("fish7",fish_num[6]);
				//PlayerPrefs.SetInt("tatol",fish_num[0]+fish_num[1]+fish_num[2]+fish_num[3]+fish_num[4]+fish_num[5]+fish_num[6]);


				Application.LoadLevel("menu");
				PlayBand.Device1.OnIncomingDataEvent -= ReceiveData2;
				PlayBand.OnConnectResultEvent -= ConnectResult2;
				}	
		}
		if(game_god.stopp)
		{
			if (/*Mathf.Abs(data.Acceleration.y)<0.1 && */data.Acceleration.z > 0.5) 
			{
				PlayBand.VibrateOnce (50, 500);
				game_god.stopp=false;
			}
		}
	}

	private void ResetMsg()
	{
		m_ConnectMsg = "Bluetooth not connected...";
		m_DataMsg = "";
	}
	IEnumerator hu ()
	{
		yield return new WaitForSeconds (Durability);
		if(!game_god.stopp)game_god.humidity += humidity_num;
		if(humidity_num==0){if(transform.position.z>1){humidity_num=5;}else{humidity_num=-dryability;}}
		StartCoroutine (hu());
	}
	void Update () 
	{
		if(Input.GetKeyDown("a")){
			if(stop.activeSelf==false){
				stopp=true;
				stop.SetActive(true);
				timeff=0.1f;
			}}
		if(Input.GetKeyDown("b")){stopp=false;}
		if(Input.GetKeyDown("s")){Application.LoadLevel("menu");}
		if(timeff>0){timeff+=Time.deltaTime;}
		if(stopp==false && stop.activeSelf==true && timeff>1)
		{
			stop.SetActive(false);
			stopp=false;
		}
		if(!game_god.end && !game_god.stopp)
		{
			if(transform.position.z==-0.01f){
				net_can=false;
			}
			if(transform.position.z==2){
				net_can=true;
			}
			if(up==true&&transform.position.z!=-0.01f){
				transform.Translate(new Vector3(0,0,1)* -10f * Time.deltaTime);
			}
			else if(up==false&&transform.position.z!=2){
				transform.Translate(new Vector3(0,0,1)* 10f * Time.deltaTime);
			}
			//transform.Translate(new Vector2(dir*2,0)* 1f * Time.deltaTime);
			transform.position=new Vector3(cursor_obj.transform.position.x,cursor_obj.transform.position.y,transform.position.z);

			if (transform.position.x >= 3.2f) {dir=-1;}
			if (transform.position.x <= -1.5f) {dir=1;}
			transform.eulerAngles = new Vector3 (0, Mathf.Clamp (transform.eulerAngles.y, 0, 31), 0);
		}
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -3.4f, 5),
		                                 Mathf.Clamp (transform.position.y, -3.7f, 2.4f),
		                                  Mathf.Clamp (transform.position.z, -0.01f, 2));

		if(transform.position.z>1){
			if(!water){
				s_w=Instantiate(sound_water)as GameObject;
				water=true;
				humidity_num=0;
			}
			renderer.sortingLayerID = 1; }
		if(transform.position.z<1){
			if(water){
				humidity_num=0;
				water=false;
				Destroy(s_w);
			}
			renderer.sortingLayerID = 2;}

		if (transform.childCount>0) 
		{
			for(int q=0; q<transform.childCount; q++)
			{
				if(transform.GetChild(q).renderer.sortingLayerID!=renderer.sortingLayerID)
				{
					for(int i=0; i<transform.childCount; i++)
					{
						transform.GetChild(i).renderer.sortingLayerID=renderer.sortingLayerID;
					}
				}
			}
			if(Bucket_s/*transform.position.z<0 && transform.rotation.y>0.23*/)
			{
				bool hitbucket = Physics2D.OverlapCircle(new Vector2(transform.position.x-0.83f,transform.position.y+0.86f),
				                                              1.24f,
			                                           		1 << LayerMask.NameToLayer("Ignore Raycast"));
				if (hitbucket)
				{
					Instantiate(sound_get);
					PlayBand.VibrateOnce(50, 500);
					game_god.get_num+=transform.childCount;
					for(int q=0; q<transform.childCount; q++)
					{
						for(int i=0;i<7;i++){
						if(transform.GetChild(q).tag==i.ToString()){
							fish_num[i]+=1;
						}
						}
						Destroy(transform.GetChild(q).gameObject);
					}
				}
				Bucket_s=false;
			}
		}
	}	

	void Net_Click(){
		if (transform.position.z > 1) {//往上
			up=true;
			//transform.Translate(new Vector3 (0,0,-2.01f));
		}
		else if(transform.position.z < 1){//往下
			up=false;
			//transform.Translate(new Vector3 (0,0,+2.01f));
		}
	}

	void Bucket_Click(){
		Bucket_s = true;
	}

}
