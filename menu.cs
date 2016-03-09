using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {
	public GameObject sound_button;
	public static bool sound;
	public GameObject cam;
	public int d;//揮動方向
	public float t;//畫面移動時間

	public int total;//抓到的魚總數
	public GameObject total_text;//抓到的魚總數的text

	public GameObject[] fish_atlas;//魚的種類物件
	public int[] fish_num;//每種魚抓到的數量
	public GameObject[] fish_text;//每種魚抓到的數量的text

	private string m_ConnectMsg;
	private string m_DataMsg;

	private Animator play_anim;
	private Animator exit_anim;
	private int Choose;

	public static int buttonn = 0;

	public Sprite[] spr_level;
	private SpriteRenderer levell;
	private SpriteRenderer endurancee; 
	private int netlevel; //網子等級

	void Start()
	{
		buttonn=0;
		Screen.orientation = ScreenOrientation.Portrait;//UpsideDown;
		PlayBand.OnConnectResultEvent += ConnectResult;
		PlayBand.Device1.OnIncomingDataEvent += ReceiveData;
		PlayBand.Device1.On4WayTriggerEventV+=fway;
		PlayBand.OnButtonClickedEvent += ButtonClicked;

		PlayBand.StartMouseListener();
		PlayBand.Connect();
		ResetMsg();

		//total = PlayerPrefs.GetInt ("total",0);
		fish_num[0] = PlayerPrefs.GetInt("fish1",0);
		fish_num[1] = PlayerPrefs.GetInt("fish2",0);
		fish_num[2] = PlayerPrefs.GetInt("fish3",0);
		fish_num[3] = PlayerPrefs.GetInt("fish4",0);
		fish_num[4] = PlayerPrefs.GetInt("fish5",0);
		fish_num[5] = PlayerPrefs.GetInt("fish6",0);
		fish_num[6] = PlayerPrefs.GetInt("fish7",0);
		total = fish_num [0] + fish_num [1] + fish_num [2] + fish_num [3] + fish_num [4] + fish_num [5] + fish_num [6];
		total_text.SetActive (false);
		for (int i =0; i<7; i++) {
			fish_atlas[i].SetActive (false);
			fish_text[i].SetActive(false); 
		}

		play_anim = GameObject.Find ("play_button_1").GetComponent<Animator> ();
		exit_anim = GameObject.Find ("exit_button_1").GetComponent<Animator> ();
		levell = GameObject.Find ("level_1").GetComponent<SpriteRenderer>();
		endurancee = GameObject.Find ("endurance_1").GetComponent<SpriteRenderer>();

		levell.sprite = spr_level[PlayerPrefs.GetInt("lev",1)-1];
		endurancee.sprite = spr_level[PlayerPrefs.GetInt("end",1)+9];
		Choose = 1;
		d = 0;
		t = 0.0f;
		play_anim.SetInteger("chos",1);
		play_anim.SetTrigger("chang");
	}

	void Update(){if(Input.GetKeyDown("s")){Application.LoadLevel("game");}
		//原點往右
		if (d == 1) {
			t+=Time.deltaTime;
		cam.transform.position = new Vector3 (Mathf.Lerp (0.0f, 4.8f, t), 0.0f, -4.1f);
		}
		//右邊往左
		if (d == -1) {
			t+=Time.deltaTime;
			cam.transform.position = new Vector3 (Mathf.Lerp (4.8f, 0.0f, t), 0.0f, -4.1f);
		}
		//原點往左
		if (d == 2) {
			t+=Time.deltaTime;
			cam.transform.position = new Vector3 (Mathf.Lerp (0.0f, -4.8f, t), 0.0f, -4.1f);
		}
		//左邊往右
		if (d == -2) {
			t+=Time.deltaTime;
			cam.transform.position = new Vector3 (Mathf.Lerp (-4.8f, 0.0f, t), 0.0f, -4.1f);
		}
		if (cam.transform.position.x == 0.0f || cam.transform.position.x == 4.8f || cam.transform.position.x == -4.8f) {
			d=0;
			t=0.0f;
			if(cam.transform.position.x == -4.8f){
				total_text.SetActive (true);
				total_text.gameObject.GetComponent<GUIText> ().text = total.ToString();
				for (int i =0; i<7; i++) {
					if(fish_num[i]>0){
					fish_atlas[i].SetActive (true);
					fish_text[i].SetActive(true);
				    fish_text[i].gameObject.GetComponent<GUIText> ().text = fish_num[i].ToString();
					}
				}
			}
			else{
				total_text.SetActive (false);
				for (int i =0; i<7; i++) {
					fish_atlas[i].SetActive (false);
					fish_text[i].SetActive(false); 
				}
			}
		}
		else{
			total_text.SetActive (false);
			for (int i =0; i<7; i++) {
				fish_atlas[i].SetActive (false);
				fish_text[i].SetActive(false); 
			}
		}
		/*if (cam.transform.position.x== 0.0f) {
			d=1;
			t=0.0f;
				}
		if (cam.transform.position.x == 4.8f) {
			d=0;
			t=0.0f;
				}*/

		if(buttonn==1)
		{
			PlayBand.VibrateOnce (50, 500);
			Application.LoadLevel("game");
			//PlayBand.StopMouseListener();
			PlayBand.Device1.On4WayTriggerEventV-=fway;
			PlayBand.Device1.OnIncomingDataEvent -= ReceiveData;
			PlayBand.OnConnectResultEvent -= ConnectResult;
		}
		if(buttonn==2)
		{
			PlayBand.VibrateOnce (50, 500);
			Application.Quit();
		}
	}

	void Play_Click(){
		PlayBand.VibrateOnce (50, 500);
		Application.LoadLevel("game");
		//PlayBand.StopMouseListener();
		PlayBand.Device1.On4WayTriggerEventV-=fway;
		PlayBand.Device1.OnIncomingDataEvent -= ReceiveData;
		PlayBand.OnConnectResultEvent -= ConnectResult;
	}
	
	void Exit_Click(){
		print ("ok");
		PlayBand.VibrateOnce (50, 500);
		Application.Quit();
		//Application.LoadLevel("game");
	}

	void Right_Click(){
		print ("right");
		//if (!sound) {
		//sound=true;
		Instantiate (sound_button);
		PlayBand.VibrateOnce (50, 500);
		//}
		if(cam.transform.position.x==0.0f){
			d=1;
			//cam.transform.position= new Vector3(4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, 4.8f, 1.0f), 0.0f, 2.82f);
		}
		else if(cam.transform.position.x==-4.8f){
			d=-2;
			//cam.transform.position= new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(-4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
		}
	}

	void Left_Click(){
		print ("left");
		//if (!sound) {
		//sound=true;
		Instantiate (sound_button);
		PlayBand.VibrateOnce (50, 500);
		//}
		if(cam.transform.position.x==0.0f){
			d=2;
			//cam.transform.position = new Vector3(-4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, -4.8f, 1.0f), 0.0f, 2.82f);
		}
		else if(cam.transform.position.x==4.8f){
			d=-1;
			//cam.transform.position = new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
		}
	}

	public void ButtonClicked (PlayBandID data)
	{

	}
	void OnGUI()
	{
		//GUI.Label(new Rect(50, 150, 200, 50), m_ConnectMsg);
		
		//GUI.Label(new Rect(50, 250, 200, 100), m_DataMsg);
	}
	public void ConnectResult(PlayBandConnectData result)
	{
		if (result.success)
		{
			m_ConnectMsg = "Bluetooth connected to "+result.address;
		}
	}

	public void fway(PlayBandDirection direction,PlayBandData data){
		if (direction == PlayBandDirection.Left) {
			//if (!sound) {
				//sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			//}
			if(cam.transform.position.x==0.0f){
				d=2;
				//cam.transform.position = new Vector3(-4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, -4.8f, 1.0f), 0.0f, 2.82f);
			}
			else if(cam.transform.position.x==4.8f){
				d=-1;
				//cam.transform.position = new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
			}
		}
		if (direction == PlayBandDirection.Right) {
			//if (!sound) {
				//sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			//}
			if(cam.transform.position.x==0.0f){
				d=1;
				//cam.transform.position= new Vector3(4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, 4.8f, 1.0f), 0.0f, 2.82f);
			}
			else if(cam.transform.position.x==-4.8f){
				d=-2;
				//cam.transform.position= new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(-4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
			}
	    }
		if (cam.transform.position.x == 0.0f &&direction == PlayBandDirection.Up) {
			//if (!sound) {
				//sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			//}
			Choose = 1;
			play_anim.SetInteger ("chos", 1);
			exit_anim.SetInteger ("chos", 0);
			play_anim.SetTrigger ("chang");
		}
		if (cam.transform.position.x == 0.0f &&direction == PlayBandDirection.Down) {
			//if (!sound) {
				//sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			//}
			Choose = 2;
			play_anim.SetInteger ("chos", 0);
			exit_anim.SetInteger ("chos", 2);
			exit_anim.SetTrigger ("chang");
		}

	}



	public void ReceiveData(PlayBandData data)
	{
		m_DataMsg = string.Format("Acceleration: X={0},Y={1},Z={2}  EulerAngle: X={3},Y={4},Z={5} Rotation:X={6},Y={7},Z{8}",
		                          data.Acceleration.x, data.Acceleration.y, data.Acceleration.z, 
		                          data.EulerAngles.x, data.EulerAngles.y, data.EulerAngles.z,
		                          data.Rotation.eulerAngles.x,data.Rotation.eulerAngles.y,data.Rotation.eulerAngles.z
		                          );


		//往左滑選擇play
		/*if (data.Acceleration.x < -0.4) {
			if (!sound) {
				sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			}
			if(camera.transform.position.x==0.0f){
				camera.transform.position = new Vector3(-4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, -4.8f, 1.0f), 0.0f, 2.82f);
			}
			else if(camera.transform.position.x==4.8f){
				camera.transform.position = new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
			}
		}
		//往右滑選擇play
		else if (data.Acceleration.x > 0.4) {
			if (!sound) {
				sound=true;
				Instantiate (sound_button);
				PlayBand.VibrateOnce (50, 500);
			}
			if(camera.transform.position.x==0.0f){
				camera.transform.position= new Vector3(4.8f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(0.0f, 4.8f, 1.0f), 0.0f, 2.82f);
			}
			else if(camera.transform.position.x==-4.8f){
				camera.transform.position= new Vector3(0.0f,0.0f,-4.1f);//new Vector3(Mathf.Lerp(-4.8f, 0.0f, 1.0f), 0.0f, 2.82f);
			}
		}
		//往上滑選擇play
	    else if (data.Acceleration.y < -0.4) {
						if (!sound) {
				sound=true;
								Instantiate (sound_button);
								PlayBand.VibrateOnce (50, 500);
						}
						Choose = 1;
						play_anim.SetInteger ("chos", 1);
						exit_anim.SetInteger ("chos", 0);
						play_anim.SetTrigger ("chang");
				}
		//往下滑選擇exit
		else if (data.Acceleration.y > 0.4) {
						if (!sound) {
				sound=true;
								Instantiate (sound_button);
								PlayBand.VibrateOnce (50, 500);
						}
						Choose = 2;
						play_anim.SetInteger ("chos", 0);
						exit_anim.SetInteger ("chos", 2);
						exit_anim.SetTrigger ("chang");
				} else {
			sound=false;
				}*/
		if (cam.transform.position.x == 0.0f && Mathf.Abs(data.Acceleration.y)<0.1 && data.Acceleration.z > 0.3) {
			if(Choose==1){
				PlayBand.VibrateOnce (50, 500);
				//PlayBand.StopMouseListener();
				PlayBand.Device1.On4WayTriggerEventV-=fway;
				PlayBand.Device1.OnIncomingDataEvent -= ReceiveData;
				PlayBand.OnConnectResultEvent -= ConnectResult;
				Application.LoadLevel("game");
			}
			if(Choose==2){
				PlayBand.VibrateOnce (50, 500);
				Application.Quit();
			}	
		}

	}	
	private void ResetMsg()
	{
		m_ConnectMsg = "Bluetooth not connected...";
		m_DataMsg = "";
	}
}
