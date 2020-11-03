using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovementScript : MonoBehaviour
{
    public float force = 10f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        int randNumber = Random.Range(1, 4);
        Vector2 temp = transform.position;
        if (randNumber == 1) rb.velocity = new Vector2(temp.x, temp.y + force);
        if (randNumber == 2) rb.velocity = new Vector2(temp.x, temp.y - force);
        if (randNumber == 3) rb.velocity = new Vector2(temp.x + force, temp.y);
        if (randNumber == 4) rb.velocity = new Vector2(temp.x - force, temp.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
