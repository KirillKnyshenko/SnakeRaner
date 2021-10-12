using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пример
/// </summary>
public class Player : MonoBehaviour
{
    public float speedLR, speedForward, speedParts;
    public List<Transform> parts;
    public bool isFever;
    [SerializeField] GameObject prefabPlayerPart;
    GameDataObject gdo;
    ChangeColor changeColor;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gdo = GameDataObject.GetData();
        parts.Add(transform);
        changeColor = gameObject.GetComponent<ChangeColor>();
        isFever = false;
    }

    void Update()
    {
        rb.isKinematic = !(GameManager.instance.gameStage == GameStage.Game);
        if (GameManager.instance.gameStage == GameStage.Game)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                transform.Translate(Vector3.right * Input.GetAxis("Mouse X") * speedLR * Time.deltaTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.5f, 2.5f), transform.position.y, transform.position.z);
            }
            HeadMove(speedForward);
        }

        PartsMove(speedParts);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Untagged" && other.transform.tag != "Head" && other.transform.tag != "BodyPart")
        {
            switch (other.transform.tag)
            {
                case "Crystal":
                    gdo.main.saves.AddToPref(Prefs.Crystals, 1);
                    var crystal = other.transform.gameObject;
                    crystal.transform.GetChild(0).gameObject.SetActive(true);
                    Destroy(other, 1);
                    GetComponent<CrystalEater>().Eat();
                    break;
                case "Stickman":
                    ChangeColor ccStickmen = other.transform.gameObject.GetComponent<ChangeColor>();
                    if (changeColor.currentColor == ccStickmen.currentColor || isFever)
                    {
                        GameManager.currentLevel.stickmen++;
                        var part = Instantiate(prefabPlayerPart, transform.position - new Vector3(0, 0, 2), transform.rotation);
                        part.GetComponent<ChangeColor>().currentColor = changeColor.currentColor;
                        parts.Add(part.transform);
                        Destroy(other, 1);
                    }
                    else
                    {
                        UIManager.instance.Loose();
                    }
                    break;
            }
            other.transform.gameObject.AddComponent<EatedObject>().player = this;
            // Обновляется счётчики
            UIManager.instance.UpdateText();
            //other.transform.tag = "Untagged";
        }
    }

    public void HeadMove(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void PartsMove(float speed)
    {
        for (int i = 1; i < parts.Count; i++)
        {
            parts[i].position = Vector3.Lerp(parts[i].position, parts[i - 1].position + Vector3.back, speed * Time.deltaTime);
            parts[i].LookAt(parts[i - 1]);
        }
    }
}
