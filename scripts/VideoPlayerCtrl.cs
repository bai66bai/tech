using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerCtrl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture; // 用于显示视频的RenderTexture

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
    private bool isFrist = false;

    private void Start()
    {
        videoPlayer.SetDirectAudioMute(0, true);
        isMute = true;
    }


    private bool isplaying = true;

    public void CtrlVideoStatus()
    {
        if (!isFrist)
        {
            isMute = true;
            CtrlMute();
            videoPlayer.isLooping = false;
            isLoop = false;
            videoPlayer.frame = 0;
            isFrist=true;
        }
        videoPlayer.Play();
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

    //控制视频静音循环播放
    public void CtrlLoop()
    {
        isMute = false;
        CtrlMute();
        videoPlayer.isLooping = true;
        isLoop=true;    
        videoPlayer.Play();
        isFrist = false;
    }
}
