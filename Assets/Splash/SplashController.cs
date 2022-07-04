using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    public VideoPlayer vid;
    //public Animator anim;
    public GameObject splash;
    float timer;
    public RawImage image;
    bool fading;
    void Awake()
    {
        
        timer = 10;
        vid.Play();
        fading = false;
    }

    private void Update()
    {
        Debug.Log(timer);
        timer -= Time.deltaTime;

        if(timer < 9)
        {
            
        }
        
        

        if(timer < 1 && !fading)
        {
            fading = true;
            image.CrossFadeAlpha(0f, 0.5f, true);
        }
        if(timer < 0)
        {
            splash.SetActive(false);
        }

    }


}
