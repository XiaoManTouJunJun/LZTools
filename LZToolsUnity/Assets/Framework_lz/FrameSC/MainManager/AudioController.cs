
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// func: 音频管理器
/// 
/// author: lz910608@gmail.com
/// 
/// time: 2019-1-29
/// </summary>
namespace FrameWork_lz
{
    public class AudioController : Singleton<AudioController>
    {
        
       

        private const int MaxAudioCount = 10;
        private const string ResourcePath = "Audio/";
        private const string StreamingAssetsPath = "";
        private AudioSource BGMAudioSource;
        private AudioSource LastAudioSource;

        private Dictionary<string, AudioClip> exitAudioClipResDic = new Dictionary<string, AudioClip>();
        #region Mono Function

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                //SoundPlay("S_NiuQunPao", 0.50f,true,true);
                AudioController.Instance.SoundPlay("S_XuLi", 1.0f, false, true);
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                //SoundStop("S_NiuQunPao");
                SoundStop("S_XuLi",false);
            }
        }



        #endregion

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="audioname"></param>
        public void SoundPlay(string audioname, float volume = 1, bool onlyOnce = false, bool isLoop = false)
        {
            if (exitAudioClipResDic.ContainsKey(audioname))
            {
                AudioClip sound;
                exitAudioClipResDic.TryGetValue(audioname, out sound);
                if(onlyOnce)
                {
                    AudioSource[] exitSoundArr = this.transform.GetComponentsInChildren<AudioSource>();
                    if(exitSoundArr.Length>0)
                    {
                        for (int i = 0; i < exitSoundArr.Length; i++)
                        {
                            if (exitSoundArr[i].gameObject.name == audioname)
                                return;
                        }
                    }
                    if(ChectIsPlayAudio(audioname))
                    {
                        return;
                    }else
                    {
                        if (sound != null) this.PlayClip(sound, volume, audioname, isLoop);
                    }

             

                }else
                {
                    if (sound != null) this.PlayClip(sound, volume, audioname, isLoop);
                }
                
            }
            else
            {
                AudioClip sound = this.GetAudioClip(audioname);

                if (sound != null)
                {
                    exitAudioClipResDic.Add(audioname, sound);
                    this.PlayClip(sound, volume,audioname,isLoop);
                }
            }
        }

        

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="audioname"></param>
        public void SoundPause(string audioname)
        {
            if (this.LastAudioSource != null)
            {
                this.LastAudioSource.Pause();
            }
        }

        /// <summary>
        /// 暂停所有音效音乐
        /// </summary>
        public void SoundAllPause()
        {
            AudioSource[] allsource = FindObjectsOfType<AudioSource>();
            if (allsource != null && allsource.Length > 0)
            {
                for (int i = 0; i < allsource.Length; i++)
                {
                    allsource[i].Pause();
                }
            }
        }

        /// <summary>
        /// 停止特定的音效
        /// </summary>
        /// <param name="audioname"></param>
        public void SoundStop(string audioname,bool isAll = true)
        {
            if(this.transform != null)
            {
                AudioSource[] gameObjects = this.transform.GetComponentsInChildren<AudioSource>();
                foreach(var item in gameObjects)
                {
                    if(item.gameObject.name == audioname)
                    {
                        Destroy(item.gameObject);
                        if (!isAll)
                            break;
                    }
                }
     
            }
        }
   

        /// <summary>
        /// 设置音量
        /// </summary>
        public void BGMSetVolume(float volume)
        {
            if (this.BGMAudioSource != null)
            {
                this.BGMAudioSource.volume = volume;
            }
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="bgmname"></param>
        /// <param name="volume"></param>
        public void BGMPlay(string bgmname, float volume = 1f)
        {
            BGMStop();

            if (bgmname != null)
            {
                AudioClip bgmsound = this.GetAudioClip(bgmname);
                if (bgmsound != null)
                {
                    this.PlayLoopBGMAudioClip(bgmsound, volume,bgmname);
                }
            }
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void BGMPause()
        {
            if (this.BGMAudioSource != null)
            {
                this.BGMAudioSource.Pause();
            }
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void BGMStop()
        {
            if (this.BGMAudioSource != null && this.BGMAudioSource.gameObject)
            {
                Destroy(this.BGMAudioSource.gameObject);
                this.BGMAudioSource = null;
            }
        }

        /// <summary>
        /// 重新播放
        /// </summary>
        public void BGMReplay()
        {
            if (this.BGMAudioSource != null)
            {
                this.BGMAudioSource.Play();
            }
        }

        bool ChectIsPlayAudio(string audioName)
        {
            AudioSource[] gameObjects = this.transform.GetComponentsInChildren<AudioSource>();
            foreach (var item in gameObjects)
            {
                if (item.gameObject.name == audioName)
                {
                    return true;
                }
            }
            return false;
        }

       

        #region 音效资源路径

        enum eResType
        {
            AB = 0,
            CLIP = 1
        }

        /// <summary>
        /// 下载音效
        /// </summary>
        /// <param name="aduioname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private AudioClip GetAudioClip(string aduioname, eResType type = eResType.CLIP)
        {
            AudioClip audioclip = null;
            switch (type)
            {
                case eResType.AB:
                    break;
                case eResType.CLIP:
                    audioclip = GetResAudioClip(aduioname);
                    break;
                default:
                    break;
            }
            return audioclip;
        }

        private IEnumerator GetAbAudioClip(string aduioname)
        {
            yield return null;
        }

        private AudioClip GetResAudioClip(string aduioname)
        {
            string clipPath = ResourcePath + aduioname;
         
            AudioClip tempClip = Resources.Load(clipPath) as AudioClip;
            if(tempClip == null)
            {
                LZLoger.LogMsg(clipPath+" 音频文件查找失败！请检查资源命名或资源是否存在", LogColor.Red, LogLevel.Important);
            }else
            {
                LZLoger.LogMsg(clipPath + " 音频加载成功！！！", LogColor.Green, LogLevel.Important);
            }
            return tempClip;
        }
        #endregion

        #region 背景音乐
        /// <summary>
        /// 背景音乐控制器
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="volume"></param>
        /// <param name="isloop"></param>
        /// <param name="name"></param>
        private void PlayBGMAudioClip(AudioClip audioClip, float volume = 1f, bool isloop = false, string name = null)
        {
            if (audioClip == null)
            {
                return;
            }
            else
            {
                GameObject obj = new GameObject(name);
                obj.transform.parent = this.transform;
                AudioSource LoopClip = obj.AddComponent<AudioSource>();
                LoopClip.clip = audioClip;
                LoopClip.volume = volume;
                LoopClip.loop = true;
                LoopClip.pitch = 1f;
                LoopClip.Play();
                this.BGMAudioSource = LoopClip;
            }
        }

        /// <summary>
        /// 播放一次的背景音乐
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="volume"></param>
        /// <param name="name"></param>
        private void PlayOnceBGMAudioClip(AudioClip audioClip, float volume = 1f, string name = null)
        {
            PlayBGMAudioClip(audioClip, volume, false, name == null ? "BGMSound" : name);
        }

        /// <summary>
        /// 循环播放的背景音乐
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="volume"></param>
        /// <param name="name"></param>
        private void PlayLoopBGMAudioClip(AudioClip audioClip, float volume = 1f, string name = null)
        {
            PlayBGMAudioClip(audioClip, volume, true, name == null ? "LoopSound" : name);
        }

        #endregion

        #region  音效
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="volume"></param>
        /// <param name="name"></param>
        private void PlayClip(AudioClip audioClip, float volume = 1f, string name = null,bool isLoop = false)
        {

            if (audioClip == null)
            {
                return;
            }
            else
            {
                GameObject obj = new GameObject(name == null ? "SoundClip" : name);
                obj.transform.parent = this.transform;
                AudioSource source = obj.AddComponent<AudioSource>();
                if(isLoop == false)
                {
                    StartCoroutine(this.PlayClipEndDestroy(audioClip, obj));
                }
                
                source.pitch = 1f;
                source.volume = volume;
                source.clip = audioClip;
                source.loop = isLoop;
                source.Play();
                this.LastAudioSource = source;
            }
        }

        /// <summary>
        /// 播放玩音效删除物体
        /// </summary>
        /// <param name="audioclip"></param>
        /// <param name="soundobj"></param>
        /// <returns></returns>
        private IEnumerator PlayClipEndDestroy(AudioClip audioclip, GameObject soundobj)
        {
            if (soundobj == null || audioclip == null)
            {
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(audioclip.length * Time.timeScale);
                Destroy(soundobj);
            }
        }


        #endregion
    }
}


