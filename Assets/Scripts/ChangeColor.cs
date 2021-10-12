using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    Renderer rn;
    public string currentColor = "Red";
    GameDataObject gdo;
    [SerializeField] bool isUpdate;

    void Start()
    {
        gdo = GameDataObject.GetData();
        rn = GetComponentInChildren<Renderer>();
        ChangingColor();
    }

    void Update()
    {
        if (isUpdate)
        {
            ChangingColor();
        }
    }
    public void ChangingColor()
    {
        rn.material.color = gdo.colors.Find(x => x.colorName == currentColor).color;
    }
}
