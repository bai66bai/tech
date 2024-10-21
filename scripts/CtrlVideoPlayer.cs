using UnityEngine;
using UnityEngine.Video;

public class CtrlVideoPlayer : Players
{
    public VideoPlayer videoPlayer;
    public PlayerChangeCtrl playerChangeCtrl;

    public override void LoadPlay(string url)
    {
        // 将文件路径转换为 URL
        string fileUrl = new System.Uri(url).AbsoluteUri;
        videoPlayer.url = fileUrl;
        Loop(playerChangeCtrl.IsLoop);
        Mute(playerChangeCtrl.IsMute);
        videoPlayer.Play();
    }

    //将视频重置到开头
    public override void Head()
    {
        videoPlayer.Stop();
        videoPlayer.frame = 0;
        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    //控制声音
    public override void Mute(bool isMute)
    {
        videoPlayer.SetDirectAudioMute(0,isMute);
    }

    //播放视频
    public override void Play()
    {
        videoPlayer.Play();
    }

    //暂停视频
    public override void Pause()
    {
        videoPlayer.Pause();
    }

    
    //控制循环
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
        //调用原脚本加载下一个方法
        playerChangeCtrl.NextLoadSource();
    }



    private void OnEnable()
    {
        videoPlayer.targetTexture.Release();
    }
}
