using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerCtrl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture; // 用于显示视频的RenderTexture
    public SeriesVideoList ctrVideoSeries;


    private bool isMute = false;
    private bool isLoop = true;
    public bool IsMute
    {
        get => isMute;
    }
    public bool IsLoop
    {
        get => isLoop;
    }

    private void Start()
    {
        videoPlayer.SetDirectAudioMute(0, true);
        isMute = true;
    }

    //继续播放视频 有声播放 停止全部视频循环
    public void CtrlVideoPlay()
    {
        videoPlayer.Play();
        isMute = true;
        CtrlMute();
        ctrVideoSeries.EndAllVideoLoop();
        isLoop = false;
    }

    public void CtrlMute()
    {
        isMute = !isMute;
        videoPlayer.SetDirectAudioMute(0, isMute);
    }

    //控制视频暂停
    public void CtrlVideoPause()
    {
        videoPlayer.Pause();
    }

    //控制视频回到第一帧有声播放
    public void resetVideoTime()
    {
        videoPlayer.frame = 0;
        videoPlayer.Play();
        isMute = true;
        CtrlMute();
        ctrVideoSeries.EndAllVideoLoop();
        isLoop = false;
    }
    //控制视频静音循环播放
    public void CtrlLoop()
    {
        isMute = false;
        CtrlMute();
        ctrVideoSeries.PlayAllVideoLoop();
        isLoop = true;
        videoPlayer.Play();
    }

    //切换上一个视频
    public void ToLastVideo()
    {
        ctrVideoSeries.ToPrevious();
        isMute = true;
        CtrlMute();
        isLoop = false;
    }
    //切换下一个视频
    public void ToNextVideo()
    {
        ctrVideoSeries.ToNext();
        isMute = true;
        CtrlMute();
        isLoop = false;
    }
}
