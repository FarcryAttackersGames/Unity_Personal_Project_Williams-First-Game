using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject Pausemenu;
    public PlayerController playerData;

    public Image Healthbar;
    public TextMeshProUGUI Clip;
    public TextMeshProUGUI Ammo;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.fillAmount = Mathf.Clamp((float)playerData.Health / (float)playerData.maxHealth, 0, 1);

        if (playerData.weaponID < 0)
        {
            Clip.gameObject.SetActive(false);
            Ammo.gameObject.SetActive(false);

        }
        else
        {
            Clip.gameObject.SetActive(true);
            Ammo.gameObject.SetActive(true);

            Clip.text = "Current Mag" + playerData.currentAmmo + "/" + playerData.clipsize;
            Ammo.text = "Ammo" + playerData.currentAmmo;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausemenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;

            isPaused = true;
        }
        else
            Resume();
    }
    public void Resume()
    {
        Pausemenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        isPaused = false;
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}