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

}
