using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField]
    private int numOfEnemies;

    [SerializeField]
    private List<GameObject> SpawnerList;

    [SerializeField]
    private List<GameObject> Enemies;

    private int SpawnedObjects;

     private void OnValidate() {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        if (this.SpawnerList.Count != allChildren.Length - 1){
            foreach(Transform child in allChildren){
                if(child != this.transform)
                this.SpawnerList.Add(child.gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int totalSpace = 0;
        SpawnLimit[] allChildren = GetComponentsInChildren<SpawnLimit>();
        foreach(SpawnLimit i in allChildren){
            totalSpace += i.getSpawnLimit();
        }
        if(numOfEnemies > totalSpace){
            numOfEnemies = totalSpace;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnedObjects < 1){
            //SpawnedObjects are destroyed
            spawn();
        }
    }

    private void spawn(){
        for(int i = 0; i < numOfEnemies; i++){
            GameObject enemy = Enemies[Random.Range(0, Enemies.Count)];
            GameObject spawner = SpawnerList[Random.Range(0, SpawnerList.Count)];
            Instantiate(enemy, spawner.transform);
            SpawnedObjects++;
        }
    }

    public void removeMe(GameObject obj){
        SpawnedObjects--;
        Destroy(obj);
    }
}
