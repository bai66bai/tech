using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Players : MonoBehaviour
{
    public virtual void LoadPlay(string url) { }
    public virtual void Head() { }
    public virtual void Loop(bool isLoop) { }
    public virtual void Mute(bool isMute) { }
    public virtual void Pause() { }
    public virtual void Play() { }
}
