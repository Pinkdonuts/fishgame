using UnityEngine;
using System.Collections;

public class game_god : MonoBehaviour {
	public static int fish_num; //場上的魚數量
	public static int get_num; //抓到的魚數量
	public static float humidity; //濕度
	public GameObject[] FishPerfab;
	public Sprite[] moisture;
	private SpriteRenderer bar_spr;
	public int moisture_num;
	public static bool end = false;
	public GameObject black; //分數UI
	public GameObject score_text;//分數text
	public GameObject score;
	private Quaternion rot;
	public GameObject stop; //暫停UI
	public static bool stopp = false;
	private float timeff;
	
	// Use this for initialization
	void Awake () {
		stopp=false;
		fish_num = 0;
		rot.eulerAngles = new Vector3(0,0,90);
		end = false;
		get_num = 0;
		humidity = 0;
		score.SetActive (true);
		//Screen.orientation = ScreenOrientation.LandscapeLeft;
		bar_spr = GameObject.Find ("Bar").GetComponent<SpriteRenderer> ();
		//PlayBand.Device1.OnButtonClickedEvent -= ButtonClicked;
		//PlayBand.Device1.OnButtonClickedEvent += ButtonClicked;

	}
	
	// Update is called once per frame
	void Update () {
		if (!end && fish_num < 5) 
		{
			GameObject fish = Instantiate( FishPerfab[Random.Range(0,7)],
			                              new Vector3(0,-10,0.5f),rot) as GameObject;
			fish_num+=1;
			score.guiText.text=get_num.ToString();
		}
		hu_bar ();
		if(end)
		{
			score.SetActive(false);
			black.SetActive(true);
			score_text.guiText.text=get_num.ToString();
		}
	/*	if(game_god.stopp==false && stop.activeSelf==true && timeff>1)
		{
			stop.SetActive(false);
			stopp=false;
		}
		if(timeff>0){timeff+=Time.deltaTime;}*/
	}
/*	public void ButtonClicked ()
	{
		if(stop.activeSelf==false){
			stopp=true;
			stop.SetActive(true);
			timeff=0.1f;
		}
	}*/
	void hu_bar () {
		humidity = Mathf.Clamp (humidity, 0, 100);
		moisture_num = (int)(humidity / 100 * 16);
		bar_spr.sprite = moisture [moisture_num];
		if(humidity==100){end=true;}
	}
}
