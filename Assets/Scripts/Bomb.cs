using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Head")
        {
            if (!other.transform.GetComponent<Player>().isFever)
            {
                UIManager.instance.Loose();
            }
            explosion.transform.parent = null;
            explosion.SetActive(true);           
            Destroy(gameObject);
            Destroy(explosion, 1);
        }
    }
}
