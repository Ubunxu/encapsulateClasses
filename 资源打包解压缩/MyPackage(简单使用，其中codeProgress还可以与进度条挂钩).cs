using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

/*
四川麻将
时间：2017.6.12
作者：风一样的程序员
版本：2.6
*/
namespace SC_MahJong
{
    public class MyPackage : MonoBehaviour
    {
        //进度条
        CodeProgress cp = null;
        public Slider slider;
        public Text sliderText;
        float progress = 0;
        string path = "";
        string urlPath = "http://192.168.1.105/ab/js.zupk";
        bool isReleaseOver = false;
        // Use this for initialization
        void Start()
        {
            cp = new CodeProgress(SetProgress);
            path = Application.dataPath;
            //path = System.Environment.CurrentDirectory;
            //path = path.Replace('\\','/')+"/Assets";
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = progress;
            if (isReleaseOver)
            {
                //跳转到地形场景，然后进入读取配置文件
                SceneManager.LoadScene("Jungle Canyon");
            }
        }
        private void LateUpdate()
        {
            this.sliderText.text = (int)(slider.value * 100) + "%";
        }

        public void DownLoadFile()
        {
            this.StartCoroutine(Dw());
        }

        IEnumerator Dw()
        {
            WWW www = new WWW(urlPath);
            string fileName = urlPath.Substring(urlPath.LastIndexOf('/') + 1);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(Application.dataPath + "/FilesFromServer/"+fileName, www.bytes);
                File.WriteAllBytes(Application.dataPath + "/PackFolder/"+fileName, www.bytes);
                print("下载完成");
                //解压,里面包含释放
                Decompress();
            }
            else
            {
                print(www.error);
            }
        }

        /// <summary>
        /// 打包
        /// </summary>
        public void Pack()
        {
            this.slider.value = 0;
            MyPackRes.PackFolder(Application.dataPath + "/剑圣", path+"/PackFolder/xml.upk", cp,(str)=> {
                //this.sliderText.text = str.ToString();      
                print(str);
            });

            //this.StartCoroutine(WaitPack());
        }
        IEnumerator WaitPack()
        {
            yield return new WaitForSeconds(1);
            MyPackRes.PackFolder(Application.dataPath + "/DataFile", Application.dataPath + "/PackFolder/xml.upk", cp);
        }

        /// <summary>
        /// 压缩
        /// </summary>
        public void Compress()
        {
            this.slider.value = 0;
            new Thread(() =>
            {
                LZMAHelper.Compress(path + "/PackFolder/xml.upk",  path+"/PackFolder/xml.zupk", cp, (str) =>
                {
                    print(str);
                });
            }).Start();
           

            //this.StartCoroutine(WaitCompress());
        }
        IEnumerator WaitCompress()
        {
            yield return new WaitForSeconds(1);
            LZMAHelper.Compress(Application.dataPath + "/PackFolder/xml.upk", Application.dataPath + "/PackFolder/xml.zupk", cp);
        }
        /// <summary>
        /// 解压
        /// </summary>
        public void Decompress()
        {
            this.progress = 0;
            new Thread(() =>
            {
                LZMAHelper.DeCompress(path + "/Enemy/Enemy.zupk", path + "/Enemy/EnemyDe.upk", cp,(str)=> {
                    print(str);
                    Release();
                    //释放完成之后就将isReleaseOver设置为true；
                    isReleaseOver = true;                    
                });
            }).Start();

            //this.StartCoroutine(this.WaitDecompress());
        }
        IEnumerator WaitDecompress()
        {
            yield return new WaitForSeconds(1);
            LZMAHelper.DeCompress(Application.dataPath + "/PackFolder/xml.zupk", Application.dataPath + "/PackFolder/xmlDe.upk", cp);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            this.progress = 0;
            UPKExtra.ExtraUPK(path + "/Enemy/EnemyDe.upk", path+"/Enemy", cp);
            print("释放完成");

            //this.StartCoroutine(WaitRelease());
        }
        IEnumerator WaitRelease()
        {
            yield return new WaitForSeconds(1);
            UPKExtra.ExtraUPK(Application.dataPath + "/PackFolder/xmlDe.upk", Application.dataPath + "/PackFolder", cp);
        }

        public void SetProgress(long fileSize,long processSize)
        {
            this.progress = (float)processSize / fileSize;
        }

        
    }
}

