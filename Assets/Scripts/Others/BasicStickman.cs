using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right = 0,
    left = 1
}

public class BasicStickman : MonoBehaviour {

	private Dictionary<Rigidbody2D, _Muscle> musclesByRg = new Dictionary<Rigidbody2D, _Muscle>();

	bool buttonLeft = false, buttonRight = false;

	public float jumpForce;
	public _Muscle[] muscles;

	[Space]
	public Rigidbody2D head;
	public Rigidbody2D _legbr;
	public Rigidbody2D _legbl;
	public Rigidbody2D _legur;
	public Rigidbody2D _legul;
	public Rigidbody2D _hip;
	[Space]

	public float moveSpeed;
	public float maxMoveSpeed = 14;

	_RaycastDoll dollRaycast;

	private _Muscle[] upper_leg = new _Muscle[2];
	private _Muscle[] lower_leg = new _Muscle[2];
	private _Muscle hip;

	void Start(){
		dollRaycast = new _RaycastDoll(transform);

		foreach(_Muscle muscle in muscles){
			musclesByRg.Add(muscle.bone, muscle);
		}

		lower_leg[(int)Direction.right] = musclesByRg[_legbr];
		lower_leg[(int)Direction.left] = musclesByRg[_legbl];
		upper_leg[(int)Direction.right] = musclesByRg[_legur];
		upper_leg[(int)Direction.left] = musclesByRg[_legul];
		hip = musclesByRg[_hip];
	}

	bool walk = false;

	void Update(){
		if(Input.GetAxisRaw("Horizontal") > 0 || buttonRight){
			walk = true;
			walk_direction = 1;
		}
		else if(Input.GetAxisRaw("Horizontal") < 0 || buttonLeft){
			walk = true;
			walk_direction = -1;
		}
		else{
			walk = false;
			walk_direction = 0;
		}
		/*else if(walk) {
			walk = false;
			foreach(_Muscle muscle in muscles)
				muscle.bone.velocity *= 0.1f;
		}*/
	}

	void FixedUpdate(){
		jump();
		if(walk){
			Walk();
		}

		foreach(_Muscle muscle in muscles){
			muscle.ActivateMuscle();
		}
	}

	//////////////////////////////////////////////

	public void toggleButtonRight(){
		buttonRight = !buttonRight;
	}

	public void toggleButtonLeft(){
		buttonLeft = !buttonLeft;
	}

	public void jump(){
		if(Input.GetKey(KeyCode.Space)){
			_Muscle.noForce = true;
			walk = false;
			//head.velocity = new Vector2(head.velocity.x, 0);
			head.AddForce(new Vector2(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
		}
	}

	/////////////////////////////////////////////

	int walk_direction;
	float CurrentCycleTime = 0;
	float time_passed = 0;
	int state = 0;

	public float timeCycle = 0.25f;

	public void Walk(){
		_Muscle.noForce = false;
		//RaycastHit2D hit = dollRaycast.RayCastClosest(transform.position, Vector2.down);

		//Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		//walk_direction = dir.x < 0 ? -1 : 1;

		WalkCycle(walk_direction);

	/*	Vector2 walkVector = (walk_direction * hit.normal) * movement_speed;
		walkVector = new Vector2(walkVector.y, -walkVector.x + .1f * movement_speed);

		hip.OverloadMuscleRot(6); */
		/*float xVel = _hip.velocity.x;
			
			if (xVel < maxMoveSpeed && walk_direction > 0)
				_hip.AddForce(moveSpeed * Time.deltaTime * Vector2.right * walk_direction);
			else if (xVel > -maxMoveSpeed && walk_direction < 0)
				_hip.AddForce(moveSpeed * Time.deltaTime * Vector2.right * walk_direction);

			if (walk_direction == 0) {
				
			}
			
			
			//If player is turning around, help turn faster
			if (xVel > 0.2f && walk_direction < 0)
				_hip.AddForce(moveSpeed * 3.2f * Time.deltaTime * -Vector2.right);
			if (xVel < 0.2f && walk_direction > 0) {
				_hip.AddForce(moveSpeed * 3.2f * Time.deltaTime * Vector2.right);
			} */

		head.AddForce(new Vector2(walk_direction * moveSpeed, 0));	
	}

	public void WalkCycle(int dir){
		int f1 = (int)Direction.right;
		int s1 = (int)Direction.left;
		if(state > 1){
			f1 = (int)Direction.left;
			s1 = (int)Direction.right;
		}
		switch(state){
			case 2:
			case 0:
				CurrentCycleTime = timeCycle;
				//upper_leg[f1].SetMuscleRot(90 * dir);
				upper_leg[f1].SetMuscleRot(35 * dir);
				upper_leg[s1].SetMuscleRot(-35 * dir);
				//upper_leg[s1].SetMuscleRot(0);
				//foot
				lower_leg[f1].SetMuscleRot(35 * dir);
				//lower_leg[f1].SetMuscleRot(0);
				lower_leg[s1].SetMuscleRot(-35 * dir);
				//lower_leg[s1].SetMuscleRot(-25 * dir);
				break;
			case 3:
			case 1:
				CurrentCycleTime = timeCycle * 0.33f;
				/*upper_leg[f1].SetMuscleRot(0);
				upper_leg[s1].SetMuscleRot(45 * dir);
				//foot
				upper_leg[f1].SetMuscleRot(0);
				upper_leg[s1].SetMuscleRot(-25 * dir);*/
				upper_leg[f1].SetMuscleRot(-35 * dir);
				upper_leg[s1].SetMuscleRot(35 * dir);
				//foot
				lower_leg[f1].SetMuscleRot(-35 * dir);
				lower_leg[s1].SetMuscleRot(35 * dir);
				break;
		}

		time_passed += Time.deltaTime;
		if(time_passed > CurrentCycleTime){
			time_passed = 0;
			state++;
			if(state > 3)
				state = 0;
		}
	}
}

[System.Serializable]
public class _Muscle{
	public Rigidbody2D bone;
	public float restRotation;
	public float force;
	public static bool noForce;

	private float addRotation;
	private float currentforce;

	public void ActivateMuscle(){
		if(noForce) return;
		RotateSmooth(restRotation + addRotation, currentforce);
		addRotation = 0;
		currentforce = force;
	}

	public void SetMuscleRot(float rot){
		addRotation = rot;
	}

	public void OverloadMuscleRot(float addForcePercentage){
		currentforce = force * addForcePercentage;
	}

	public void AddMovement(float movement){
		//bone.MovePosition(bone.position + movement * Time.deltaTime);
		bone.AddForce(new Vector2(movement * Time.deltaTime, 0));
	}

	private void RotateSmooth(float rotation, float force){
		float angle = Mathf.DeltaAngle(bone.rotation, rotation);
		float ratio = angle / 180;
		ratio *= ratio;

		bone.MoveRotation(Mathf.LerpAngle(bone.rotation, rotation, force * ratio * Time.fixedDeltaTime));
		bone.AddTorque(angle * force * (1 - ratio) * .1f);
	}
}

public class _RaycastDoll{
	private List<Collider2D> transform_colliders;

	public _RaycastDoll(Transform parent){
		transform_colliders = new List<Collider2D>();
		foreach(Collider2D collider in parent.GetComponentsInChildren<Collider2D>()){
			transform_colliders.Add(collider);
		}
	}
	public RaycastHit2D RayCastClosest(Vector2 origin, Vector2 dir, float distance = 5f){
		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance);
		float closest_distance = float.MaxValue;
		RaycastHit2D closest_hit = new RaycastHit2D();
		foreach(RaycastHit2D hit in hits){
			if(!IsChild(hit.collider)){
				if(hit.distance < closest_distance){
					closest_distance = hit.distance;
					closest_hit = hit;
				}
			}
		}
		return closest_hit;
	}

	public bool IsChild(Collider2D col){
		foreach(Collider2D collider in transform_colliders){
			if(collider == col)
				return true;
		}
		return false;
	}
}