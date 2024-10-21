using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImagePlayer : Players
{
    [HideInInspector]
    public int waitTime = 10;

    private float startTime;
    public Image image;
    public PlayerChangeCtrl playerChangeCtrl;
    public override void LoadPlay(string url)
    {
        StartCoroutine(LoadImage(url));
    }

    // 协程加载图片
    private IEnumerator LoadImage(string filePath)
    {
        // 暂时隐藏Image组件，防止出现“白一下”的现象
        image.enabled = false;
        Debug.Log(filePath);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(filePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading image: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                image.sprite = sprite;
                //image.SetNativeSize(); // 设置Image的大小以适应精灵
            }
        }
        // 加载完成后显示Image组件
        image.enabled = true;
        startTime = Time.time;
    }


    private void Update()
    {
       if(image.enabled && Time.time > startTime + waitTime && playerChangeCtrl.IsLoop)
        {
            playerChangeCtrl.NextLoadSource();
        } 
    }
    public override void Head()
    {
        startTime = Time.time;
    }
  
    }

