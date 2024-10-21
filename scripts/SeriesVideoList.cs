using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class SeriesVideoList : MonoBehaviour
{
    public string FolderName;

    [SerializeField]
    private VideoPlayer videoPlayer;
    private List<string> videoPaths;
    private int currentVideoIndex = 0;


    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPaths = new List<string>();
        string streamingAssetsPath = Application.streamingAssetsPath; //获取 StreamingAssets 文件夹下的所有视频文件
        string[] supportedExtensions = { ".mp4", ".mov", ".avi", ".mkv", ".m4v" , ".png" , ".jpg" , ".jpeg"};

        var files = Directory.GetFiles(streamingAssetsPath + "/" + FolderName);

        var sortedFiles = files.OrderBy(file =>
        {
            var fullNameArr = file.Split("\\");
            var fullName = fullNameArr[^1];
            var name = fullName.Split('.')[0];
            return int.Parse(new string(name.Where(char.IsDigit).ToArray()));
        });

        foreach (var file in sortedFiles)
        {
            string extension = Path.GetExtension(file).ToLower();
            if (System.Array.Exists(supportedExtensions, ext => ext == extension))
            {
                videoPaths.Add(file);
            }
        }

        if (videoPaths.Count > 0)
        {
            PlayVideo(videoPaths[currentVideoIndex]);
        }
    }

    //播放上一个视频
    public void ToPrevious()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        if (currentVideoIndex - 1 >= 0)
        {
            ChangeVideo(videoPaths[currentVideoIndex - 1]);
            currentVideoIndex--;
        }
        else
        {
            ChangeVideo(videoPaths[videoPaths.Count - 1]);
            currentVideoIndex = videoPaths.Count - 1;
        }
    }


    //更换视频路径并重新播放
    public void ChangeVideo(string path)
    {
        // 将文件路径转换为 URL
        string url = new System.Uri(path).AbsoluteUri;
        videoPlayer.url = url;
        videoPlayer.Play();
    }


    //播放下一个视频
    public void ToNext()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        if (currentVideoIndex + 1 < videoPaths.Count)
        {
            ChangeVideo(videoPaths[currentVideoIndex + 1]);
            currentVideoIndex++;
        }
        else
        {
            ChangeVideo(videoPaths[0]);
            currentVideoIndex = 0;
        }
    }

    //全部视频循环
    public void PlayVideo(string path)
    {
        // 将文件路径转换为 URL
        string url = new System.Uri(path).AbsoluteUri;
        videoPlayer.url = url;

        // 注册视频播放结束事件
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 解除注册事件
        videoPlayer.loopPointReached -= OnVideoEnd;
        // 播放下一个视频
        currentVideoIndex = (currentVideoIndex + 1) % videoPaths.Count;
        PlayVideo(videoPaths[currentVideoIndex]);
    }


    //结束全部视频轮播
    public void EndAllVideoLoop()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    //开始全部视频轮播
    public void PlayAllVideoLoop()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }


}
