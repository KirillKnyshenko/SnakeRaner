using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatedObject : MonoBehaviour
{
    public Player player;
    
    void Start()
    {
        transform.parent = player.transform;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 5 * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5 * Time.deltaTime);
    }
}
