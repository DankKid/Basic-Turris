using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Animator mapAnim;
    [SerializeField] Animator settingAnim;

    public void PlayGame()
    {
        //Open map menu
        mapAnim.SetTrigger("Open");
    }

    public void Back()
    {
        mapAnim.SetTrigger("Close");
    }

    public void Settings()
    {
        settingAnim.SetTrigger("Open");
    }

    public void SettingBack()
    {
        settingAnim.SetTrigger("Close");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }        


}
