using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullCharacter : MonoBehaviour {

	public float force = 70f;
	public Rigidbody2D rg;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Mouse0)){
			Vector2 dir = rg.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
			dir = dir.normalized;

			rg.MovePosition(rg.position + -dir * force * Time.deltaTime);
			//rg.AddForce(-dir * force * Time.deltaTime * 1000);
		}
	}
}
