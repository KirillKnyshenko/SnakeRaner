using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text crystals, kills;
    public static UIManager instance;
    [SerializeField] GameObject deathUI, winUI;
    [SerializeField] GameObject tapToPlay;

    #region Mono
    private void Start()
    {
        instance = this;
        InitTapToPlay();
        UpdateText();
    }

    public void InitTapToPlay()
    {
        if (tapToPlay != null)
        {
            if (GameManager.instance.gameStage == GameStage.StartWait)
            {
                tapToPlay.SetActive(true);
                GameManager.TapToPlayUI += () => { Tweaks.AnimationPlayType(tapToPlay, PlayType.Rewind); }; //Анимации к эвенту тапа
            }
            else
            {
                tapToPlay.SetActive(false);
            }
        }
    }
    
    private void Update()
    {
        EditorControls();
    }

    public void UpdateText()
    {
        crystals.text = GameManager.instance.data.saves.GetPref(Prefs.Crystals).ToString();
        kills.text = GameManager.currentLevel.stickmen.ToString();
    }
    #endregion

    #region Buttons
    public void EditorControls()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Loose();
        }
#endif
    }

    public void NextLevel()
    {
        GameManager.NextLevel();
    }

    public void Restart()
    {
        GameManager.Restart();
    }

    #endregion

    #region Evens_Win_Loose
    public void Win()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManager.OnLevelEnd();
            winUI.SetActive(true);
        }
    }

    public void Loose()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManager.OnLevelEnd(false);
            deathUI.SetActive(true);
        }
    }

    #endregion
}
