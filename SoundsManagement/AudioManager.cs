using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
四川麻将
时间：2017.6.12
作者：风一样的程序员
版本：2.6
*/
namespace SC_MahJong
{
    public class AudioManager : MonoBehaviour
    {
        
        private static AudioManager instance = null;
        //背景音乐的声音源
        private AudioSource audioSource = null;
        //保存所有声音的声音剪辑。
        private Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
        //保存多个音效源，这样可以同时播放多个
        private List<AudioSource> effectSourceList = new List<AudioSource>();

        /// <summary>
        /// 得到该声音控制类的对象
        /// </summary>
        /// <returns></returns>
        public static AudioManager InitAudioManager()
        {
            if (instance == null)
            {
                //这个bgSounds之所以也会在DontDestroyOnLoad区域，
                //主要是因为此类对象使用了DontDestroyOnLoad,而该游戏对象是挂载了AudioManager;
                GameObject gameObject = new GameObject("bgSounds");
                instance = gameObject.AddComponent<AudioManager>();
                instance.audioSource = gameObject.AddComponent<AudioSource>();
                instance.audioSource.playOnAwake = false;
                
                ///
                for(int i = 0; i < 5; ++i)
                {
                    GameObject obj = new GameObject("effectSounds"+i);
                    AudioSource source =  obj.AddComponent<AudioSource>();
                    source.playOnAwake = false;
                    instance.effectSourceList.Add(source);
                    DontDestroyOnLoad(obj);
                    //obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="bgName">背景音乐的名字</param>
        /// <param name="isLoop"></param>
        public void PlayBackGroundMusic(string bgName,bool isLoop = false)
        {
            AudioClip clip;
            if (clipDic.ContainsKey(bgName))
            {
                clip = clipDic[bgName];
            }
            else
            {
                clip = Resources.Load<AudioClip>(bgName);
                clipDic.Add(bgName, clip);
            }
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }

        /// <summary>
        /// 停止后，继续播放
        /// </summary>
        public void PlayAgain()
        {
            this.audioSource.Play();
        }

        /// <summary>
        /// 暂停播放音乐
        /// </summary>
        public bool PauseBackGroundMusic
        {
            set
            {
                if (value)
                {
                    this.audioSource.Pause();
                }
                else
                {
                    this.audioSource.UnPause();
                }
            }
        }

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        public void StopBackMusic()
        {
            this.audioSource.Stop();
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        public float SetVolume
        {
            set
            {
                this.audioSource.volume = value;
            }
            get
            {
                return this.audioSource.volume;
            }
        }
        /// <summary>
        /// 同时播放
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="isLoop"></param>
        public void PlaySoundsEffect(string effectName,bool isLoop = false)
        {
            foreach (AudioSource source in effectSourceList)
            {
                if (!source.isPlaying)//当一个播放源正在播放时，则不会再次播放，给另外一个播放源播放
                {
                    AudioClip clip;
                    if (this.clipDic.ContainsKey(effectName))
                    {
                        clip = this.clipDic[effectName];
                    }
                    else
                    {
                        clip = Resources.Load<AudioClip>(effectName);
                        this.clipDic.Add(effectName, clip);
                    }
                    source.clip = clip;
                    source.loop = isLoop;
                    source.Play();
                    print("-------===="+source.name);
                    break;//当其中一个播放源播放后那么就停止循环。
                }
                print("========");
            }
        }
        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

