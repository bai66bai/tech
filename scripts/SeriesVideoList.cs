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
        string streamingAssetsPath = Application.streamingAssetsPath; //��ȡ StreamingAssets �ļ����µ�������Ƶ�ļ�
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

    //������һ����Ƶ
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


    //������Ƶ·�������²���
    public void ChangeVideo(string path)
    {
        // ���ļ�·��ת��Ϊ URL
        string url = new System.Uri(path).AbsoluteUri;
        videoPlayer.url = url;
        videoPlayer.Play();
    }


    //������һ����Ƶ
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

    //ȫ����Ƶѭ��
    public void PlayVideo(string path)
    {
        // ���ļ�·��ת��Ϊ URL
        string url = new System.Uri(path).AbsoluteUri;
        videoPlayer.url = url;

        // ע����Ƶ���Ž����¼�
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // ���ע���¼�
        videoPlayer.loopPointReached -= OnVideoEnd;
        // ������һ����Ƶ
        currentVideoIndex = (currentVideoIndex + 1) % videoPaths.Count;
        PlayVideo(videoPaths[currentVideoIndex]);
    }


    //����ȫ����Ƶ�ֲ�
    public void EndAllVideoLoop()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    //��ʼȫ����Ƶ�ֲ�
    public void PlayAllVideoLoop()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }


}
