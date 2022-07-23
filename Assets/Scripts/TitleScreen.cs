using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Animator mapAnim;
    [SerializeField] Animator settingAnim;
    [SerializeField] Animator gunAnim;
    [SerializeField] Animator statAnim;

    [SerializeField] GameObject selected;
    [SerializeField] TMP_Text statText;

    [SerializeField] List<GameObject> lockedMaps = new List<GameObject>();
    [SerializeField] List<GameObject> lockedGuns = new List<GameObject>();

    bool gunOpened = false, mapOpened = false, settingsOpened = false;

    #region general
    public void PlayGame()
    {
        
        if (!mapOpened)
        {
            mapAnim.SetTrigger("OpenMap");
            mapOpened = true;
        }
        
    }

    public void Back()
    {
        if (mapOpened)
        {
            mapAnim.SetTrigger("CloseMap");
            mapOpened = false;
        }
        
    }

    public void GunSelection()
    {
        if (!gunOpened)
        {
            gunAnim.SetTrigger("OpenGun");
            gunOpened = true;
        }
            
    }

    public void GunBack()
    {
        if (gunOpened)
        {
            gunAnim.SetTrigger("CloseGun");
            gunOpened = false;
        }
            
    }


    public void Settings()
    {
        if (!settingsOpened)
        {
            settingAnim.SetTrigger("Open");
            settingsOpened = true;
        }
        
    }

    public void SettingBack()
    {
        if (settingsOpened)
        {
            settingAnim.SetTrigger("Close");
            settingsOpened = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    
    public void Stats()
    {
        statAnim.SetTrigger("OpenStats");
    }
    public void CloseStats()
    {
        statAnim.SetTrigger("CloseStats");
    }
    
    
    #endregion

    #region maps
    public void Map1()
    {
        SceneManager.LoadScene(1);
    }
    public void Map2()
    {
        SceneManager.LoadScene(2);
    }
    public void Map3()
    {
        SceneManager.LoadScene(3);
    }
    public void Map4()
    {
        SceneManager.LoadScene(4);
    }
    public void Map5()
    {
        SceneManager.LoadScene(5);
    }
    public void Map6()
    {
        SceneManager.LoadScene(6);
    }
    #endregion






    private void Start()
    {
        if(PlayerPrefs.GetString("selectedGun") == "")
        {
            PlayerPrefs.SetString("selectedGun", "pistol");


            PlayerPrefs.SetInt("bulletsShot", 0);
            PlayerPrefs.SetInt("enemiesKilled", 0);
            PlayerPrefs.SetInt("wavesCompleted", 0);
            PlayerPrefs.SetInt("pointsGained", 0);
            PlayerPrefs.SetInt("pointsSpent", 0);
            PlayerPrefs.SetInt("playerLevel", 0);
        }




        switch (PlayerPrefs.GetString("selectedGun"))
        {
            case "pistol":
                selected.transform.localPosition = new Vector3(-350, 85, 0);
                break;
            case "rifle":
                selected.transform.localPosition = new Vector3(0, 85, 0);
                break;
            case "shotgun":
                selected.transform.localPosition = new Vector3(350, 85, 0);
                break;
        }

        checkUnlock();

        statText.text = "Bullets Shot: " + PlayerPrefs.GetInt("bulletsShot") + "\nEnemies Killed: " + PlayerPrefs.GetInt("enemiesKilled") + "\nWaves Completed: " + PlayerPrefs.GetInt("wavesCompleted") + "\nPoints Gained: " + PlayerPrefs.GetInt("pointsGained") + "\nPoints Spent: " + PlayerPrefs.GetInt("pointsSpent");



    }



    #region gun

    public void SelectedPistol()
    {
        selected.transform.localPosition = new Vector3(-350, 85, 0);
        
        PlayerPrefs.SetString("selectedGun", "pistol");
    }
    public void SelectedRifle()
    {
        selected.transform.localPosition = new Vector3(0, 85, 0);
        PlayerPrefs.SetString("selectedGun", "rifle");
    }

    public void SelectedShotgun()
    {
        selected.transform.localPosition = new Vector3(350, 85, 0);
        PlayerPrefs.SetString("selectedGun", "shotgun");
    }
    #endregion


    #region unlocks

    void checkUnlock()
    {
        switch (PlayerPrefs.GetInt("playerLevel"))
        {
            case 1:
                lockedMaps[0].SetActive(false);
                break;
            case 2:
                lockedMaps[0].SetActive(false);
                lockedMaps[1].SetActive(false);
                lockedGuns[0].SetActive(false);
                break;
            case 3:
                lockedMaps[0].SetActive(false);
                lockedMaps[1].SetActive(false);
                lockedMaps[2].SetActive(false);
                break;
            case 4:
                lockedMaps[0].SetActive(false);
                lockedMaps[1].SetActive(false);
                lockedMaps[2].SetActive(false);
                lockedMaps[3].SetActive(false);
                lockedGuns[0].SetActive(false);
                lockedGuns[1].SetActive(false);
                break;
            case 5:
                lockedMaps[0].SetActive(false);
                lockedMaps[1].SetActive(false);
                lockedMaps[2].SetActive(false);
                lockedMaps[3].SetActive(false);
                lockedMaps[4].SetActive(false);
                break;
        }
    }




    #endregion

}
