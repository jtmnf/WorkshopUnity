using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGeneration : MonoBehaviour {

    public GameObject car;
    public GameObject barrier;
    public GameObject road;
    public GameObject building;

    List<GameObject> list_buildings;
    List<GameObject> list_barriers;
    List<GameObject> list_roads;

    // Static Values
    private static float BUILDING_LEFT = -7.37F;
    private static float BUILDING_RIGHT = 7.37F;
    private static float BUILDING_Z_START = -10F;
    private static float BUILDING_STEP = 10F;
    private float LAST_BUILDING_LEFT = 0F;
    private float LAST_BUILDING_RIGHT = 0F;

    private static float ROAD_STEP = 30F;
    private static float ROAD_X_START = 0F;
    private static float ROAD_Z_START = 0F;
    private float LAST_ROAD = 0F;

    private static float MAX_BARRIER_X = 1.63F;
    private static float MIN_BARRIER_X = -1.63F;

    // Use this for initialization
    void Start() {
        InitializateLists();
        GenerateObjectsInScene();

        // Road
        //Instantiate(road, new Vector3(0F, -0.3F, 0F), Quaternion.Euler(-90F, 0F, 0F));

        // Barrier
        //Instantiate(barrier, new Vector3(1F, -0.25F, 10F), Quaternion.identity);

        // Car
        //Instantiate(car, new Vector3(0F, 0F, 0F), Quaternion.identity);

        // Left Building
        //Instantiate(building, new Vector3(BUILDING_LEFT, -0.3F, 0F), Quaternion.Euler(-90F, 180F, 0F));
        
        // Right Building
        //Instantiate(building, new Vector3(BUILDING_RIGHT, -0.3F, 0F), Quaternion.Euler(-90F, 0, 0F));
    }

    // Update is called once per frame
    void Update() {
        CalculateObjectsDistance();
    }

    void CalculateObjectsDistance() {
        // Roads
        for(int i = 0; i < list_roads.Count; ++i) {
            if (list_roads[i].activeSelf) {
                float d = Vector3.Distance(list_roads[i].transform.position, car.transform.position);

                if(d > 25 && (car.transform.position.z > list_roads[i].transform.position.z)) {
                    list_roads[i].transform.position = new Vector3(list_roads[i].transform.position.x, -0.3F, ROAD_Z_START + (LAST_ROAD * ROAD_STEP));
                    LAST_ROAD += 1;

                    GenerateBarriers(list_roads[i].transform.position);
                }
            }
        }

        // Buildings
        for (int i = 0; i < list_buildings  .Count; ++i) {
            if (list_buildings[i].activeSelf) {
                float d = Vector3.Distance(list_buildings[i].transform.position, car.transform.position);

                if (d > 15 && (car.transform.position.z > list_buildings[i].transform.position.z)) {
                    if (list_buildings[i].transform.position.x == BUILDING_LEFT) {
                        list_buildings[i].transform.position = new Vector3(list_buildings[i].transform.position.x, 0, BUILDING_Z_START + (LAST_BUILDING_LEFT * BUILDING_STEP));
                        LAST_BUILDING_LEFT += 1;
                    }
                    else {
                        list_buildings[i].transform.position = new Vector3(list_buildings[i].transform.position.x, 0, BUILDING_Z_START + (LAST_BUILDING_RIGHT * BUILDING_STEP));
                        LAST_BUILDING_RIGHT += 1;
                    }
                }
            }
        }

        // Walls
        for (int i = 0; i < list_barriers.Count; ++i) {
            if (list_barriers[i].activeSelf) {
                float d = Vector3.Distance(list_barriers[i].transform.position, car.transform.position);

                if (d > 15 && (car.transform.position.z > list_barriers[i].transform.position.z)) {
                    DestroyObjectPool(list_barriers, list_barriers[i]);
                }
            }
        }
    }

    // Generate Barriers
    void GenerateBarriers(Vector3 roadPos) {
        GameObject wallToDeploy;
        for(int i = 0; i < 1; ++i) {
            wallToDeploy = GetObject(list_barriers);
            wallToDeploy.transform.position = new Vector3(Random.Range(roadPos.x + MIN_BARRIER_X, roadPos.x + MAX_BARRIER_X), -0.25F, Random.Range(roadPos.z, roadPos.z + ROAD_STEP));
            wallToDeploy.SetActive(true);
            wallToDeploy.name = "Barrier";
        }
    }

    // Init Lists
    void InitializateLists() {
        list_buildings = GenerateObjects(20, building);
        list_barriers = GenerateObjects(20, barrier);
        list_roads = GenerateObjects(20, road);
    }

    // Generate Objects for the first time
    void GenerateObjectsInScene() {
        // Buildings
        GameObject leftBuilding;
        GameObject rightBuilding;
        for(int i = 0; i < 10; ++i) {
            leftBuilding = GetObject(list_buildings);
            leftBuilding.transform.position = new Vector3(BUILDING_LEFT, -0.3F, BUILDING_Z_START + (i * BUILDING_STEP));
            leftBuilding.transform.rotation = Quaternion.Euler(-90F, 180F, 0F);
            leftBuilding.SetActive(true);

            rightBuilding = GetObject(list_buildings);
            rightBuilding.transform.position = new Vector3(BUILDING_RIGHT, -0.3F, BUILDING_Z_START + (i * BUILDING_STEP));
            rightBuilding.transform.rotation = Quaternion.Euler(-90F, 0F, 0F);
            rightBuilding.SetActive(true);
        }

        // Roads
        GameObject mainRoad;
        for (int i = 0; i < 5; ++i) {
            mainRoad = GetObject(list_roads);
            mainRoad.transform.position = new Vector3(ROAD_X_START, -0.3F, ROAD_Z_START + (i * ROAD_STEP));
            mainRoad.transform.rotation = Quaternion.Euler(-90F, 0F, 0F);
            mainRoad.SetActive(true);
        }

        LAST_BUILDING_LEFT = 10;
        LAST_BUILDING_RIGHT = 10;
        LAST_ROAD = 5;
    }

    // Objects manipulation
    public List<GameObject> GenerateObjects(int size, GameObject prefab) {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 30; i++) {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            list.Add(obj);
        }
        return list;
    }

    public GameObject GetObject(List<GameObject> list) {
        for (int i = 0; i < list.Count; i++) {
            if (!list[i].activeSelf) {
                GameObject obj = list[i];
                return obj;
            }
        }
        return null;
    }

    public void DestroyObjectPool(List<GameObject> list, GameObject obj) {
        obj.SetActive(false);
    }

    public GameObject GetCar() {
        return car;
    }
}
