using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    ChangeColor changeColor;

    void Start()
    {
        changeColor = gameObject.GetComponent<ChangeColor>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Head" || other.transform.tag == "BodyPart")
        {
            other.gameObject.GetComponent<ChangeColor>().currentColor = changeColor.currentColor.ToString();
        }
    }
}
