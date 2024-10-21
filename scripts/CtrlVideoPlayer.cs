using UnityEngine;
using UnityEngine.Video;

public class CtrlVideoPlayer : Players
{
    public VideoPlayer videoPlayer;
    public PlayerChangeCtrl playerChangeCtrl;

    public override void LoadPlay(string url)
    {
        // ���ļ�·��ת��Ϊ URL
        string fileUrl = new System.Uri(url).AbsoluteUri;
        videoPlayer.url = fileUrl;
        Loop(playerChangeCtrl.IsLoop);
        Mute(playerChangeCtrl.IsMute);
        videoPlayer.Play();
    }

    //����Ƶ���õ���ͷ
    public override void Head()
    {
        videoPlayer.Stop();
        videoPlayer.frame = 0;
        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    //��������
    public override void Mute(bool isMute)
    {
        videoPlayer.SetDirectAudioMute(0,isMute);
    }

    //������Ƶ
    public override void Play()
    {
        videoPlayer.Play();
    }

    //��ͣ��Ƶ
    public override void Pause()
    {
        videoPlayer.Pause();
    }

    
    //����ѭ��
    public override void Loop(bool isLoop)
    {
        if (isLoop)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        //����ԭ�ű�������һ������
        playerChangeCtrl.NextLoadSource();
    }



    private void OnEnable()
    {
        videoPlayer.targetTexture.Release();
    }
}
