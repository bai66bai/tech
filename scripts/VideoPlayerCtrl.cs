using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerCtrl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture; // ������ʾ��Ƶ��RenderTexture
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

    //����������Ƶ �������� ֹͣȫ����Ƶѭ��
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

    //������Ƶ��ͣ
    public void CtrlVideoPause()
    {
        videoPlayer.Pause();
    }

    //������Ƶ�ص���һ֡��������
    public void resetVideoTime()
    {
        videoPlayer.frame = 0;
        videoPlayer.Play();
        isMute = true;
        CtrlMute();
        ctrVideoSeries.EndAllVideoLoop();
        isLoop = false;
    }
    //������Ƶ����ѭ������
    public void CtrlLoop()
    {
        isMute = false;
        CtrlMute();
        ctrVideoSeries.PlayAllVideoLoop();
        isLoop = true;
        videoPlayer.Play();
    }

    //�л���һ����Ƶ
    public void ToLastVideo()
    {
        ctrVideoSeries.ToPrevious();
        isMute = true;
        CtrlMute();
        isLoop = false;
    }
    //�л���һ����Ƶ
    public void ToNextVideo()
    {
        ctrVideoSeries.ToNext();
        isMute = true;
        CtrlMute();
        isLoop = false;
    }
}
