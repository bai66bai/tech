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

    // Э�̼���ͼƬ
    private IEnumerator LoadImage(string filePath)
    {
        // ��ʱ����Image�������ֹ���֡���һ�¡�������
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
                //image.SetNativeSize(); // ����Image�Ĵ�С����Ӧ����
            }
        }
        // ������ɺ���ʾImage���
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

