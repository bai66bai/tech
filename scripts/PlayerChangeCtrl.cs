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
    // ֧�ֵ�ͼƬ��չ��
    private string[] imageExtensions = { ".png", ".jpg", ".jpeg"};

    // ֧�ֵ���Ƶ��չ��
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

        // ʹ��UnityWebRequest��ȡJSON�ļ�
        using UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
        // �������󲢵ȴ���Ӧ
        yield return www.SendWebRequest();

        // ����Ƿ��д���
        if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error reading JSON file: " + www.error);
        }
        else
        {
            // ����JSON����
            string json = www.downloadHandler.text;

            // ����ΪSourceData����
            SourceData sourceData = JsonUtility.FromJson<SourceData>(json);
            imagePlayer.waitTime = sourceData.imgTime;

            videoPaths = new List<string>();
            string streamingAssetsPath = Application.streamingAssetsPath; //��ȡ StreamingAssets �ļ��е�·��
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

    //������һ���ز�
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

    //����ѭ���;���
    private void EndMuteAndLoop()
    {
        Player.Loop(false);
        Player.Mute(false);
        isMute = false;
        isLoop = false;
    }


    //�л���һ��
    public void Next()
    {
        EndMuteAndLoop();
        NextLoadSource();   
    }


    //�л���һ��
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

    //��ͣ
    public void Pause()
    {
        EndMuteAndLoop();
        Player.Pause();
    }

    //����
    public void Play()
    {
        Player.Play();
        EndMuteAndLoop();
    }

    //������Ƶ����ͷ
    public void ReastVideo()
    {
        Player.Head();
        EndMuteAndLoop();
    }

    //�ı�����״̬
    public void ChangeMute()
    {
        Player.Mute(!isMute);
        isMute = !isMute;
    }



    //��ʼ����ѭ��
    public void StartLoop()
    {
        Player.Loop(true);
        Player.Mute(true);
        isLoop = true;
        isMute = true;
    }

   //����·��������Ƶ
    public void FileType(int sourceId)
    {
        string file = videoPaths[sourceId];
        string extension = Path.GetExtension(file).ToLower(); // ��ȡ�ļ���չ������ת��ΪСд��ĸ

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

    // �ж��Ƿ�ΪͼƬ
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
    // �ж��Ƿ�Ϊ��Ƶ
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

    //�ı���ʾ�Ĳ�����
    private void changePlayer(bool isType)
    {
        Debug.Log(isType);
        imagePanel.SetActive(isType);
        videoPanel.SetActive(!isType);
        Player = isType? imagePlayer : ctrlVideoPlayer;
    }
}
