using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private Vector3 mouseWorldPosition;

    [SerializeField]
    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
     }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        Physics2D.Raycast(transform.position, mouseWorldPosition);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, mouseWorldPosition);

    }

    public void fire() {
        Vector2 directionToFire = mouseWorldPosition - transform.position;
        var distance = directionToFire.magnitude;
        var direction = directionToFire / distance;
        bullet.GetComponent<BulletScript>().setDirections(direction);
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
