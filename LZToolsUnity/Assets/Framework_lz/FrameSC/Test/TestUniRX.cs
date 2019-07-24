using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using FrameWork_lz.Data;


namespace FrameWork_lz
{
    public class TestUniRX 
    {

        public Image textImg;

        // Use this for initialization
        void Start()
        {
            /*
            Observable.EveryUpdate ()
                .Where (_ => Input.GetMouseButton (0))
                .First ()
                .Subscribe (_ => {
                Debug.LogFormat ("点击鼠标一次");
            });
    */
            //TestThreading ();
            //TestFrameOnClickSum ();

            TestRXWWW();
            //TestDelayStream ();
        }


        /// <summary>
        /// Tests the threading.
        /// </summary>
        //void TestThreading()
        //{

        //    #region =============== 线程启动 合并 
        //    this.OnMouseDownAsObservable();
        //    var thread01 = Observable.Start<TestData>(() => {
        //        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
        //        TestData tmpe = new TestData(10);
        //        return tmpe;
        //    }, Scheduler.MainThreadIgnoreTimeScale);//设置线程不受 Time Scale 影响

        //    var thread02 = Observable.Start<TestData>(() => {
        //        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
        //        TestData tmpe = new TestData(18);
        //        return tmpe;
        //    }, Scheduler.MainThreadIgnoreTimeScale);

        //    Observable.WhenAll(thread01, thread02)
        //        .ObserveOnMainThread()
        //        .Subscribe(xs => {
        //            Debug.LogFormat("thread val 01 {0} thread val 02 {1}", xs[0].v3, xs[1].v3);
        //        });
        //    #endregion

        //}



        /// <summary>
        /// Tests the frame on click sum.
        /// </summary>
        void TestFrameOnClickSum()
        {

            #region ======= 每隔0.025秒 内记录鼠标左键点击次数
            var onClickStream = Observable.EveryUpdate()
                .Where(xs => Input.GetMouseButtonDown(0));

            onClickStream.Buffer(onClickStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(xs => {
                    Debug.LogFormat("xs count {0}", xs.Count);

                });
            #endregion

            #region ===================== 数据筛选
            //		var createNumStream = Observable.Range (0, 100)
            //			.Where (x => x % 2 == 0)
            //			.Subscribe (x => {
            //			Debug.LogFormat ("select val {0}", x);
            //		});
            #endregion


        }


        /// <summary>
        /// Tests the RXWW.
        /// </summary>
        void TestRXWWW()
        {

            //		var testWWW = ObservableWWW.Get ("http://bing.com/")
            //			.Subscribe (
            //			              x => Debug.Log (x.Substring (0, 100)), // onSuccess
            //			              ex => Debug.LogException (ex)); //


            #region 下载图片
            //		var teseWPicData = ObservableWWW.GetAndGetBytes ("http://www.g58mall.com/upload/20181114/79f865acf1b04d72ad33860e6d0afe80.jpg")//
            //			.Subscribe (
            //			                   x => {
            //				Debug.LogFormat ("下载完成！ length {0}", x.Length);//onSuccess
            //				int tempwidth = 800;
            //				int tempHeight = 1200;
            //				string path = Application.dataPath + "/testPic.jpg";
            //				File.WriteAllBytes (path, x);
            //				Texture2D tpTex = new Texture2D (tempwidth, tempHeight);
            //				tpTex.LoadImage (x);
            //				Sprite tp_sp = Sprite.Create (tpTex, new Rect (0, 0, tpTex.width, tpTex.height), Vector2.zero);
            //				textImg.sprite = tp_sp;
            //				Resources.UnloadUnusedAssets ();
            //			},
            //			                   ex => {
            //				Debug.LogException (ex);
            //			});
            #endregion

            #region 下载音频
            var teseWPicData = ObservableWWW.GetAndGetBytes("http://fjdx.sc.chinaz.com/Files/DownLoad/sound1/201811/10868.wav")//
                        .Subscribe(
                                   x => {
                                       Debug.LogFormat("下载完成！ length {0}", x.Length);//onSuccess


                                   

                                       GameObject newGo = new GameObject();

                                       WAV tmp_wav = new WAV(x);
                                       AudioClip audioClip = AudioClip.Create("testSound", tmp_wav.SampleCount, 1, tmp_wav.Frequency, false, false);
                                       audioClip.SetData(tmp_wav.LeftChannel, 0);

                                       AudioSource player = newGo.AddComponent<AudioSource>();
                                       player.clip = audioClip;
                                       player.Play();



                                       Resources.UnloadUnusedAssets();
                                   },
                                   ex => {

                                       Debug.LogException(ex);
                                   });
            #endregion





            //		var query = from baidu in ObservableWWW.Get ("http://google.com/")
            //		            from bing in ObservableWWW.Get ("http://bing.com")
            //		            from unknow in ObservableWWW.Get (baidu + bing)
            //		            select new {baidu,bing,unknow};
            //		var cancel = query.Subscribe (x => Debug.Log (x));
            //
            //		//call dispose is cancel
            //		cancel.Dispose ();
        }


        /// <summary>
        /// Tests the delay stream.
        /// </summary>
        void TestDelayStream()
        {
            Debug.LogFormat("ob start");
            Observable.Timer(TimeSpan.FromSeconds(1.5f))
                .Subscribe(_ => {
                    Debug.LogFormat("Do SomeThing else");
                });
        }


        private float[] ConvertByteTofloat(byte[] data)
        {
            float[] floatArr = new float[data.Length / 4];
            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(floatArr, i * 4, 4);
                floatArr[i] = BitConverter.ToSingle(data, i * 4);
            }
            return floatArr;
        }
    }

    public class TestData
    {
        public int v3;

        public TestData(int v3)
        {
            this.v3 = v3;
        }

    }


}








