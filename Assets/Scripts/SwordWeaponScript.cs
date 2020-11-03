using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeaponScript : MonoBehaviour
{
    public GameObject healthSquareObj;
    HealthBar healthBar;
    void Start()
    {
        healthBar = healthSquareObj.GetComponent<HealthBar>();
    }

    void Update()
    {

    }


}
