using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenshot : MonoBehaviour
{

    [SerializeField] private string screenshotName;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ScreenCapture.CaptureScreenshot("Map1");
            Debug.Log("1");
            StartCoroutine(CoroutineScreenshot());
        }
    }

    private IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("2");
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Map Screenshots/" + screenshotName + ".png" , byteArray);

    }



}
