using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour {

	public GameObject wall;
	public GameObject building;
	public GameObject road;

	public Text value;
	public Text text;
	public GameObject image;
	public Text finalScore;

	public float X_LEFT_SIDE = 13.92F;
	public float X_RIGHT_SIDE = -1.4F;
	public int Z_BUILDING_START = 20;
	public int BUILDING_STEP = 10;
	public float X_ROAD = 6.26F;
	public float Z_START_ROAD = 6.54F;
	public int ROAD_STEP = 30;

	private int LAST_Z_BUILDING_RIGHT = 0;
	private int LAST_Z_BUILDING_LEFT = 0;
	private int LAST_Z_ROAD = 0;

	private float MAX_LEFT_CAR_X = 8.33F;
	private float MAX_RIGHT_CAR_X = 4.13F;

	List<GameObject>list_wall;
	List<GameObject>list_building;
	List<GameObject>list_roads;

	private float acceleration = 1.0F;
	private float curSpeed = 0F;

	// Use this for initialization
	void Start () {
		wall.SetActive (false);
		building.SetActive (false);
		road.SetActive (false);	
		image.SetActive (false);

		list_building = PoolSystem (20, building);
		list_wall = PoolSystem (10, wall);
		list_roads = PoolSystem (5, road);

		GameObject left;
		GameObject right;
		GameObject street;
		for (int i = 0; i < 10; ++i) {
			left = GetObject (list_building);
			left.transform.position = new Vector3 (X_LEFT_SIDE, 0, Z_BUILDING_START - (i * BUILDING_STEP));
			left.SetActive (true);

			right = GetObject (list_building);
			right.transform.position = new Vector3 (X_RIGHT_SIDE, 0, Z_BUILDING_START - (i * BUILDING_STEP));
			right.transform.rotation = Quaternion.Euler (-90, 0, 180);
			right.SetActive (true);
		}

		for (int i = 0; i < 3; ++i) {
			street = GetObject (list_roads);
			street.transform.position = new Vector3 (X_ROAD, 0, Z_START_ROAD - (i * ROAD_STEP));
			street.transform.rotation = Quaternion.Euler (-90, 0, 180);
			street.SetActive (true);
		}

		LAST_Z_BUILDING_LEFT = 10;
		LAST_Z_BUILDING_RIGHT = 10;
		LAST_Z_ROAD = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (acceleration > 0F) {
			transform.Translate (Vector3.forward * curSpeed * Time.deltaTime);
			curSpeed += acceleration * Time.deltaTime;

			value.text = Mathf.Floor(curSpeed * 3.6F).ToString ();

			getTouchs ();
			calculatePositionsOfObjects ();
		}
	}

	IEnumerator OnTriggerEnter(Collider col){
		image.SetActive (true);

		float maxSpeed = curSpeed*3.6F;
		curSpeed = 0F;
		acceleration = 0F;

		text.text = "";
		value.text = "";

		finalScore.text = "Final Score: " + Mathf.Floor(maxSpeed).ToString ();

		yield return new WaitForSeconds (3);
		SceneManager.LoadScene ("1");
	}

	private void getTouchs(){
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
			Vector2 fingerPos = Input.GetTouch (0).position;

			float widthAtHalf = Screen.width / 2;

			if ((widthAtHalf - fingerPos.x) > 0 && transform.position.x < MAX_LEFT_CAR_X) {
				transform.Translate (Vector3.left * 0.1F);
			}

			if ((widthAtHalf - fingerPos.x) < 0 && transform.position.x > MAX_RIGHT_CAR_X) {
				transform.Translate (Vector3.left * (-0.1F));
			}
		}
	}

	private void calculatePositionsOfObjects(){
		// Buildings
		for (int i = 0; i < list_building.Count; i++) {
			if (list_building [i].activeSelf) {

				float distance = Vector3.Distance (list_building [i].transform.position, transform.position);

				if (distance > 15 && (transform.position.z < list_building [i].transform.position.z)) {
					if (list_building [i].transform.position.x == X_LEFT_SIDE) {
						list_building [i].transform.position = new Vector3 (list_building [i].transform.position.x, 0, Z_BUILDING_START - (LAST_Z_BUILDING_LEFT * BUILDING_STEP));
						LAST_Z_BUILDING_LEFT += 1;
					} else {
						list_building [i].transform.position = new Vector3 (list_building [i].transform.position.x, 0, Z_BUILDING_START - (LAST_Z_BUILDING_RIGHT * BUILDING_STEP));
						LAST_Z_BUILDING_RIGHT += 1;
					}
					//DestroyObjectPool (list_building, list_building [i]);
				}
			}
		}

		// Roads
		for (int i = 0; i < list_roads.Count; i++) {
			if (list_roads [i].activeSelf) {

				float distance = Vector3.Distance (list_roads [i].transform.position, transform.position);

				if (distance > 25 && (transform.position.z < list_roads [i].transform.position.z)) {
					list_roads[i].transform.position = new Vector3 (list_roads[i].transform.position.x, 0, Z_START_ROAD - (LAST_Z_ROAD * ROAD_STEP));
					LAST_Z_ROAD += 1;

					generateNewObstacles (list_roads[i].transform.position);

					//DestroyObjectPool (list_roads, list_roads[i]);
				}
			}
		}

		// Walls
		for (int i = 0; i < list_wall.Count; i++) {
			if (list_wall [i].activeSelf) {

				float distance = Vector3.Distance (list_wall [i].transform.position, transform.position);

				if (distance > 15 && (transform.position.z < list_wall [i].transform.position.z)) {
					DestroyObjectPool (list_wall, list_wall[i]);
				}
			}
		}
	}

	private void generateNewObstacles(Vector3 lastRostPos){
		GameObject wallToDeploy;
		for (int i = 0; i < 2; ++i) {
			wallToDeploy = GetObject (list_wall);
			wallToDeploy.transform.position = new Vector3(Random.Range(lastRostPos.x - 2F, lastRostPos.x + 2F), 0.045F, Random.Range(lastRostPos.z, lastRostPos.z + ROAD_STEP));
			wallToDeploy.SetActive (true);
		}
	}

	public List<GameObject> PoolSystem(int size, GameObject prefab){
		List<GameObject> list = new List<GameObject>();
		for(int i = 0 ; i < 30; i++){
			GameObject obj = (GameObject)Instantiate(prefab);
			list.Add(obj);
		}
		return list;
	}

	public GameObject GetObject(List<GameObject> list){
		for (int i = 0; i < list.Count; i++) {
			if (!list[i].activeSelf) { 
				GameObject obj = list [i];
				return obj;
			}
		}
		return null;
	}

	public void DestroyObjectPool(List<GameObject> list, GameObject obj){
		obj.SetActive(false);
	}
}

