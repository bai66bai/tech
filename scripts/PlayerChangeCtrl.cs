using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerChangeCtrl : MonoBehaviour
{
    public string FolderName;

    public CtrlVideoPlayer ctrlVideoPlayer;

    public ImagePlayer imagePlayer;
    [SerializeField]
    private GameObject videoPanel;
    [SerializeField]
    private GameObject imagePanel;

    private Players Player;
    private List<string> videoPaths;
    private int currentVideoIndex = 0;
    // 支持的图片扩展名
    private string[] imageExtensions = { ".png", ".jpg", ".jpeg"};

    // 支持的视频扩展名
    private string[] videoExtensions = { ".mp4", ".mov", ".avi", ".mkv", ".m4v"};

    private bool isMute = true;
    private bool isLoop = true;

    public bool IsMute { 
        get => isMute; 
        set => isMute = value;
    }
    
    public bool IsLoop{
        get => isLoop;
        set => isLoop = value;
    }

    private string fileName = "Source.json";

    void Start()
    {
        StartCoroutine(ReadJSON());
    }

    private IEnumerator ReadJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // 使用UnityWebRequest读取JSON文件
        using UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
        // 发送请求并等待响应
        yield return www.SendWebRequest();

        // 检查是否有错误
        if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error reading JSON file: " + www.error);
        }
        else
        {
            // 解析JSON内容
            string json = www.downloadHandler.text;

            // 解析为SourceData对象
            SourceData sourceData = JsonUtility.FromJson<SourceData>(json);
            imagePlayer.waitTime = sourceData.imgTime;

            videoPaths = new List<string>();
            string streamingAssetsPath = Application.streamingAssetsPath; //获取 StreamingAssets 文件夹的路径
            string[] supportedExtensions = { ".mp4", ".mov", ".avi", ".mkv", ".m4v", ".png", ".jpg", ".jpeg" };
            var files = sourceData.sourceName.Select(fileName=>$"{streamingAssetsPath}/{FolderName}/{fileName}");

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                if (System.Array.Exists(supportedExtensions, ext => ext == extension))
                {
                    videoPaths.Add(file);
                }
            }
            if (videoPaths.Count > 0)
            {
                FileType(0);
            }
        }
    }

    //加载下一个素材
    public void NextLoadSource()
    {
        if (currentVideoIndex + 1 < videoPaths.Count)
        {
             FileType(currentVideoIndex + 1);
             currentVideoIndex++;
        }
        else
        {
            FileType(0);
            currentVideoIndex = 0;
        }
    }

    //结束循环和静音
    private void EndMuteAndLoop()
    {
        Player.Loop(false);
        Player.Mute(false);
        isMute = false;
        isLoop = false;
    }


    //切换下一个
    public void Next()
    {
        EndMuteAndLoop();
        NextLoadSource();   
    }


    //切换上一个
    public void Last()
    {
        if (currentVideoIndex - 1 >= 0)
        {
            FileType(currentVideoIndex - 1);
            currentVideoIndex--;          
        }
        else
        {
            FileType(videoPaths.Count - 1);
            currentVideoIndex = videoPaths.Count - 1;
        }
        EndMuteAndLoop();
    }

    //暂停
    public void Pause()
    {
        EndMuteAndLoop();
        Player.Pause();
    }

    //播放
    public void Play()
    {
        Player.Play();
        EndMuteAndLoop();
    }

    //重置视频到开头
    public void ReastVideo()
    {
        Player.Head();
        EndMuteAndLoop();
    }

    //改变声音状态
    public void ChangeMute()
    {
        Player.Mute(!isMute);
        isMute = !isMute;
    }



    //开始静音循环
    public void StartLoop()
    {
        Player.Loop(true);
        Player.Mute(true);
        isLoop = true;
        isMute = true;
    }

   //根据路径加载视频
    public void FileType(int sourceId)
    {
        string file = videoPaths[sourceId];
        string extension = Path.GetExtension(file).ToLower(); // 获取文件扩展名，并转换为小写字母

        if (IsImageFile(extension))
        {
            changePlayer(true);       
        }
        else if (IsVideoFile(extension))
        {
            changePlayer(false);
        }
        Player.LoadPlay(file);
    }

    // 判断是否为图片
    private bool IsImageFile(string extension)
    {
        foreach (string ext in imageExtensions)
        {
            if (ext == extension)
            {
                return true;
            }
        }
        return false;
    }
    // 判断是否为视频
    private bool IsVideoFile(string extension)
    {
        foreach (string ext in videoExtensions)
        {
            if (ext == extension)
            {
                return true;
            }
        }
        return false;
    }

    //改变显示的播放器
    private void changePlayer(bool isType)
    {
        Debug.Log(isType);
        imagePanel.SetActive(isType);
        videoPanel.SetActive(!isType);
        Player = isType? imagePlayer : ctrlVideoPlayer;
    }
}
