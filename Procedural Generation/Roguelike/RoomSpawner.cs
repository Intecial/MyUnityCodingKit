using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Entrances{
    Left,
    Right,
    Top,
    Bottom,
    Center
}
public class RoomSpawner : MonoBehaviour
{
    [SerializeField]
    private Entrances openingDirection;

    [SerializeField]
    private RoomTemplate rooms;

    [SerializeField]
    public bool spawned = false;

    
    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        if(Entrances.Center == openingDirection){
            spawned = true;
        } else {
            Invoke("Spawn", 1f);
        }
    }

    // Update is called once per frame
    void Spawn()
    {   
        if(spawned == false){
            switch (openingDirection){
                case Entrances.Top:
                    // Spawn Bottom Door
                    Instantiate(rooms.bottomRooms[Random.Range(0, rooms.bottomRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case Entrances.Right:
                    // Spawn Left Door
                    Instantiate(rooms.leftRooms[Random.Range(0, rooms.leftRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case Entrances.Left:
                    // Spawn Right Door
                    Instantiate(rooms.rightRooms[Random.Range(0, rooms.rightRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case Entrances.Bottom:
                    // Spawn Top Door
                    Instantiate(rooms.topRooms[Random.Range(0, rooms.topRooms.Length)], transform.position, Quaternion.identity);
                    break;    
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned == true){
            Destroy(this.gameObject);
        }
    }
}
