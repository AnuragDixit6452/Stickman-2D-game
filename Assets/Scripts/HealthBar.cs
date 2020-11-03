using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float currentHealth;
    public bool reduceHealthBool;
    public bool noMoreHealthReductionBool = false;
    void Start()
    {
        currentHealth = 100f;
        reduceHealthBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reduceHealthBool && !noMoreHealthReductionBool)
        {
            float temp = transform.localScale.x;
            transform.localScale = new Vector2((temp * (temp * currentHealth) / temp) / 100, 0.8f);
            reduceHealthBool = false;
        }
    }

    public void reduceCurrentHealth(float h)
    {
        if (!noMoreHealthReductionBool)
            currentHealth -= h;
    }
}
