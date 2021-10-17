using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : MonoBehaviour
{
    Player player;
    public float timeFever;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (timeFever > 0 && GameManager.instance.gameStage == GameStage.Game)
        {
            timeFever -= Time.deltaTime;
            player.enabled = false;
            player.isFever = true;
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(0, player.transform.position.y, player.transform.position.z), 10 * Time.deltaTime);
            player.HeadMove(player.speedForward * 3);
            player.PartsMove(player.speedParts * 3);
            CameraShake.Shake();
        }
        else
        {
            player.enabled = true;
            player.isFever = false;
            GameManager.instance.data.saves.SetPref(Prefs.Crystals, 0);
            UIManager.instance.UpdateText();
            this.enabled = false;
        }
    }
    
    public void Fevering()
    {
        timeFever = 5; 
    }
}
