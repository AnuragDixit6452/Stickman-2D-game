using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LimbSelect2
{
    right = 1,
    left = 0
}

public class StickBasic2 : MonoBehaviour
{

    PlayerControls controls;

    public Muscle2[] muscles;
    [Space]

    public float angle = 0f;
    public float moveSpeed = 300f;
    public float jumpForce = 100f;

    public bool playerCanMove = true;
    public bool straight = true;

    int walkDirection;
    float CurrentCycleTime = 0;
    float time_passed = 0;
    int state = 0;
    public float timeCycle = 0.25f;

    float straightTimeout;
    public float straightWaitTime = 2f;

    public GameObject weaponLeftObj;
    public GameObject weaponRightObj;

    public int torso = 1; //index of torso in muscles array

    private Muscle2[] upper_leg = new Muscle2[2];
    private Muscle2[] lower_leg = new Muscle2[2];
    bool walkLeftPointerDown = false;
    bool walkRightPointerDown = false;

    // Boolean to check if the player is in air or not
    private bool inAir = true;

    // Jump Value
    private int jumpVal;

    // No. of Jumps
    public int noOfJumps = 1;
    bool punchRightBool;

    public GameObject healthSquareObj;
    public Collider2D leftSword, rightSword;
    public GameObject syntheticHandObj;

    public GameObject leftArmObj;

    HealthBar healthBar;

    Vector2 move;

    Vector2 aimVector;
    public float offsetForControllerAim;
    public bool gunOn = false;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Hit.started += context => punchButton();
        controls.Gameplay.Jump.started += context => jumpBtn();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Aim.performed += ctx => aimVector = ctx.ReadValue<Vector2>();
        controls.Gameplay.Shoot.started += context => turnShootBoolOnToShootProjectile();
    }

    void turnShootBoolOnToShootProjectile()
    {
        if (gunOn)
            syntheticHandObj.GetComponent<GunScriptController>().shootBool = true;
    }

    void Start()
    {
        punchRightBool = true;
        weaponLeftObj.SetActive(true);
        weaponRightObj.SetActive(false);
        resetJumpVal();
        upper_leg[(int)LimbSelect2.left] = muscles[2];
        upper_leg[(int)LimbSelect2.right] = muscles[3];

        lower_leg[(int)LimbSelect2.left] = muscles[4];
        lower_leg[(int)LimbSelect2.right] = muscles[5];
        healthBar = healthSquareObj.GetComponent<HealthBar>();

    }

    void Update()
    {
        float rotZ = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
        syntheticHandObj.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offsetForControllerAim);

        if (straightTimeout >= 0)
            straightTimeout -= Time.deltaTime;

        if (gunOn)
        {
            syntheticHandObj.SetActive(true);
            rightSword.gameObject.SetActive(false);
            leftArmObj.SetActive(false);
        }
        else
        {
            syntheticHandObj.SetActive(false);
            leftArmObj.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (straight)
        {
            foreach (Muscle2 muscle in muscles)
                muscle.ActivateMuscle();
        }
        else
        {
            leftSword.enabled = false;
            rightSword.enabled = false;
            Destroy(healthSquareObj);
            playerCanMove = false;
        }

        if (move.x > 0)
            walkDirection = 1;
        else if (move.x < 0)
            walkDirection = -1;
        else walkDirection = 0;

        // Walk - This is used twice beacuse this gives better performance
        if (walkDirection > 0 || walkRightPointerDown)
        {
            walkDirection = 1;
            punchRightBool = true;
            walkRight();
        }
        else if (walkDirection < 0 || walkLeftPointerDown)
        {
            walkDirection = -1;
            punchRightBool = false;
            walkLeft();
        }

        // Walk
        if (walkDirection > 0)
        {
            walkDirection = 1;
            punchRightBool = true;
            walkRight();
        }
        else if (walkDirection < 0)
        {
            walkDirection = -1;
            punchRightBool = false;
            walkLeft();
        }

        // JumpCycle
        if (inAir) jumpCycle(walkDirection);

        if (healthBar.currentHealth <= 70)
            //Destroy(this.gameObject);
            straight = false;
    }

    public void walkLeft()
    {
        if (!playerCanMove) return;
        if (!inAir) WalkCycle(walkDirection);
        if (straight)
        {
            muscles[torso].bone.velocity = new Vector2(0, muscles[torso].bone.velocity.y);
            muscles[torso].bone.AddForce(new Vector2(-moveSpeed, 0));
        }
        else
            muscles[torso].bone.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0));
        if (straightTimeout <= 0)
            straight = true;
    }

    public void walkRight()
    {
        if (!playerCanMove) return;
        if (!inAir) WalkCycle(walkDirection);
        if (straight)
        {
            muscles[torso].bone.velocity = new Vector2(0, muscles[torso].bone.velocity.y); ;
            muscles[torso].bone.AddForce(new Vector2(moveSpeed, 0));
        }
        else
            muscles[torso].bone.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0));
        if (straightTimeout <= 0)
            straight = true;
    }

    public void jump()
    {
        if (!playerCanMove) return;
        setInAirToTrue();
        jumpVal--;
        straightTimeout = straightWaitTime;
        muscles[torso].bone.velocity = new Vector2(muscles[torso].bone.velocity.x, 0);
        if (noOfJumps > 1 && jumpVal < noOfJumps) //Extra force for 2nd or consecutive jumps 
            muscles[torso].bone.AddForce(new Vector2(0, jumpForce + 7), ForceMode2D.Impulse);
        else
            muscles[torso].bone.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        straight = true;
    }

    public void WalkCycle(int dir)
    {
        int f1 = (int)Direction.right;
        int s1 = (int)Direction.left;
        if (state > 1)
        {
            f1 = (int)Direction.left;
            s1 = (int)Direction.right;
        }
        switch (state)
        {
            case 2:
            case 0:
                CurrentCycleTime = timeCycle;

                upper_leg[f1].SetMuscleRot(20 * dir);
                upper_leg[s1].SetMuscleRot(-20 * dir);

                lower_leg[f1].SetMuscleRot(20 * dir);
                lower_leg[s1].SetMuscleRot(-20 * dir);

                break;
            case 3:
            case 1:
                CurrentCycleTime = timeCycle * 0.33f;

                upper_leg[f1].SetMuscleRot(-20 * dir);
                upper_leg[s1].SetMuscleRot(20 * dir);

                lower_leg[f1].SetMuscleRot(-20 * dir);
                lower_leg[s1].SetMuscleRot(20 * dir);

                break;
        }

        time_passed += Time.deltaTime;
        if (time_passed > CurrentCycleTime)
        {
            time_passed = 0;
            state++;
            if (state > 3)
                state = 0;
        }
    }

    public void resetJumpVal()
    {
        jumpVal = noOfJumps;
    }

    void jumpCycle(int dir)
    {
        int f1 = (int)Direction.right;
        int s1 = (int)Direction.left;
        if (dir < 0)
        {
            f1 = (int)Direction.left;
            s1 = (int)Direction.right;
        }
        if (muscles[torso].bone.velocity.y > 0)
        {
            upper_leg[f1].RotateSmooth(90 * dir, 15);
            upper_leg[s1].RotateSmooth(-60 * dir, 15);

            lower_leg[f1].RotateSmooth(55 * dir, 15);
            lower_leg[s1].RotateSmooth(-85 * dir, 15);
        }
        else
        {
            upper_leg[f1].RotateSmooth(0 * dir, 1);
            upper_leg[s1].RotateSmooth(-0 * dir, 1);

            lower_leg[f1].RotateSmooth(0 * dir, 1);
            lower_leg[s1].RotateSmooth(-0 * dir, 1);
        }
    }

    public void setInAirToTrue()
    {
        inAir = true;
    }

    public void setInAirToFalse()
    {
        inAir = false;
    }
    public void onWalkLeftPointerDown()
    {
        walkLeftPointerDown = true;
        punchRightBool = false;
    }
    public void onWalkLeftPointerUp()
    {
        walkLeftPointerDown = false;
    }
    public void onWalkRightPointerDown()
    {
        walkRightPointerDown = true;
        punchRightBool = true;
    }
    public void onWalkRightPointerUp()
    {
        walkRightPointerDown = false;
    }
    public void punchButton()
    {
        if (gunOn) return;
        if (punchRightBool)
            StartCoroutine(punchRight(0.1f));
        else
            StartCoroutine(punchLeft(0.1f));
    }
    public void jumpBtn()
    {
        if (jumpVal > 0) jump();
    }
    IEnumerator punchRight(float time)
    {
        weaponLeftObj.SetActive(true);
        weaponRightObj.SetActive(false);
        muscles[6].restRotation = -100;
        yield return new WaitForSeconds(time);
        muscles[6].restRotation = 90;
        yield return new WaitForSeconds(time);
        for (int i = 90; i >= -1; i--)
            muscles[6].restRotation = i;
    }
    IEnumerator punchLeft(float time)
    {
        weaponLeftObj.SetActive(false);
        weaponRightObj.SetActive(true);
        muscles[7].restRotation = 100;
        yield return new WaitForSeconds(time);
        muscles[7].restRotation = -90;
        yield return new WaitForSeconds(time);
        for (int i = -90; i <= -1; i++)
            muscles[7].restRotation = i;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public bool getPlayerCanMoveBool()
    {
        return playerCanMove;
    }

    public void setPlayerCanMoveBool(bool val)
    {
        playerCanMove = val;
    }

    public bool getGunFlippedBool()
    {
        return syntheticHandObj.GetComponent<GunScriptController>().flippedBool;
    }
}


[System.Serializable]
public class Muscle2
{
    public Rigidbody2D bone;
    public float restRotation;
    private float currentforce;

    public float force;

    private float addRotation = 0f;

    public void ActivateMuscle()
    {
        RotateSmooth(restRotation + addRotation, currentforce);
        addRotation = 0;
        currentforce = force;
    }

    public void SetMuscleRot(float rot)
    {
        addRotation = rot;
    }

    public void RotateSmooth(float rotation, float force)
    {
        if (restRotation == -1) return;
        float angle = Mathf.DeltaAngle(bone.rotation, rotation);
        float ratio = angle / 180;
        ratio *= ratio;

        bone.MoveRotation(Mathf.LerpAngle(bone.rotation, rotation, force * ratio * Time.fixedDeltaTime));
        bone.AddTorque(angle * force * (1 - ratio) * .1f);
    }
}