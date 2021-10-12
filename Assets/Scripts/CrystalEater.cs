using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalEater : MonoBehaviour
{
    [SerializeField] [ReadOnly] float cooldown;
    [SerializeField] float maxCooldown;
    [SerializeField] int count;
    Fever fever;
    void Start()
    {
        cooldown = 0;
        fever = GetComponent<Fever>();
    }

    void Update()
    {
        cooldown += Time.deltaTime;
        if (maxCooldown <= cooldown)
        {
            count = 0;
        }
    }

    public void Eat()
    {
        cooldown = 0;
        count++;
        if (count >= 3)
        {
            fever.enabled = true;
            fever.Fevering();
        }
    }
}
