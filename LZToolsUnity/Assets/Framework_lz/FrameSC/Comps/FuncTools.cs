using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using G58NS.NewNetworkNS;
using UnityEngine.Networking;

namespace FrameWork_lz
{

    /// <summary>
    /// 通用工具类
    /// 截图功能 
    /// 生成不重复数 数组功能
    /// 延时等待功能
    /// </summary>
    public class FuncTools
    {

        #region ============== 全屏截图小功能 ==================
        private Texture2D picTex2D;
        /// <summary>
        /// 截图功能
        /// </summary>
        /// <param name="picQuality">图片质量 1-100 默认75</param>
        /// <param name="UploadAction">截图完自动上传回调</param>
        /// <returns></returns>
        public IEnumerator ShootScreenPic(int picQuality, Action<byte[]> UploadAction = null)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            // 先创建一个的空纹理，大小可根据实现需要来设置  
            picTex2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            yield return new WaitForEndOfFrame();
            // 读取屏幕像素信息并存储为纹理数据，  
            picTex2D.ReadPixels(rect, 0, 0);
            picTex2D.Apply();
            if (UploadAction != null) UploadAction(picTex2D.EncodeToJPG(picQuality));
            // 然后将这些纹理数据，成一个png图片文件        
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 显示截图信息
        /// </summary>
        /// <param name="showTarget"></param>
        public void ShowPicValue(object showTarget)
        {
            var imgTag = (Image)showTarget;
            if (picTex2D != null)
                imgTag.sprite = Sprite.Create(picTex2D, new Rect(0, 0, picTex2D.width, picTex2D.height), Vector2.zero);
        }

        /// <summary>
        /// 获取截图字节
        /// </summary>
        /// <param name="picQua">图片质量</param>
        /// <param name="picType">图片类型 0 png 1 jpg</param>
        /// <returns></returns>
        public byte[] GetUploadByte(int picQua, int picType)
        {
            if (picTex2D != null)
            {
                if (picType == 0)
                    return picTex2D.EncodeToPNG();
                else
                    return picTex2D.EncodeToJPG(picQua);
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region ================== 通用功能方法 ===========


        #region 生成随机不重复数 数组
        /// <summary>
        /// 随机不重复的数据
        /// </summary>
        /// <param name="iMax"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static List<int> CreateRanArrVal(int iMax, int length)
        {
            List<int> tmpList = new List<int>();
            while (tmpList.Count < length)
            {
                int temp = UnityEngine.Random.Range(0, iMax);
                if (!tmpList.Contains(temp))
                    tmpList.Add(temp);
            }
            return tmpList;
        }


        /// <summary>
        /// 取num在n位上的数字,取个位,取十位
        /// </summary>
        /// <param name="num">正整数</param>
        /// <param name="n">所求数字位置(个位 1,十位 2 依此类推)</param>
        public static  int FindNum(int num, int n)
        {
            int power = (int)Math.Pow(10, n);
            return (num - num / power * power) * 10 / power;
        }

        #endregion


        #region  延时等待功能
        /// <summary>
        /// 延时等待时间
        /// </summary>
        /// <param name="momo"></param>
        /// <param name="waitTime"></param>
        /// <param name="action"></param>
        public static void WaitTimeHandle(MonoBehaviour momo, float waitTime, System.Action action)
        {
            momo.StartCoroutine(WaitTimeAct(waitTime, action));
        }

        static IEnumerator WaitTimeAct(float val, System.Action action)
        {
            yield return new WaitForSeconds(val);
            if (action != null) action();
        }
        #endregion






        #endregion


    }




    /// <summary>
    /// 网络下载工具
    /// </summary>
    public class WebAssetTools
    {
        /// <summary>
        /// 网络连接检查 false 断线 
        /// </summary>
        /// <returns></returns>
        public static bool NetWorkConect()
        {
            //当网络不可用时              
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        /// <param name="onFail"></param>
        /// <param name="timeOut"></param>
        public static void _GetWebRequest(MonoBehaviour mono, string url, Action<UnityWebRequest> actionResult, System.Action<string> onFail, int timeOut = 10)
        {
            if (string.IsNullOrEmpty(url))
                return;
            if (!url.StartsWith("http") && !url.StartsWith("HTTP") && ClientDataHandler.Instance)
            {
                Action<string> OutResourceLoad = (error) =>
                {
                    var outUrl = ClientDataHandler.Instance.RegisterResult.urlMap.outResourceUrl + url;
                    mono.StartCoroutine(_Get(outUrl, actionResult, onFail));
                };
                var inUrl = ClientDataHandler.Instance.RegisterResult.urlMap.inResourceUrl + url;
                mono.StartCoroutine(_Get(inUrl, actionResult, OutResourceLoad));
            }
            else
            {
                mono.StartCoroutine(_Get(url, actionResult, onFail));
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
        /// <param name="action">请求发起后处理回调结果的委托</param>
        /// <returns></returns>
        static IEnumerator _Get(string url, Action<UnityWebRequest> actionResult,System.Action<string> onFail,int timeOut = 10)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {

                uwr.timeout = timeOut;
#if UNITY_2018
                 yield return uwr.SendWebRequest();
#elif UNITY_2017
                yield return uwr.Send();
#endif
                if(uwr.isHttpError || uwr.isNetworkError)
                {
                    if (onFail != null) onFail.Invoke(uwr.error);
                }else
                {
                    if (actionResult != null)
                    {
                        actionResult(uwr);
                    }
                }           
            }
        }





        #region 下载部分
        /// <summary>
        /// 异步加载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        /// <returns></returns>
        public static IEnumerator AnsyLoadFile(string url, System.Action<byte[]> onSuccess, System.Action<string> onFail, int timeOut = 10)
        {
            var request = UnityWebRequest.Get(url);
            request.timeout = timeOut;
#if UNITY_2018
            yield return request.SendWebRequest();
#elif UNITY_2017
            yield return request.Send();
#endif
            if (request.isHttpError || request.isNetworkError)
            {
                if (onFail != null) onFail.Invoke(request.error);
            }
            else
            {
                var dlHandler = request.downloadHandler;
                if (onSuccess != null) onSuccess.Invoke(dlHandler.data);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        public static void DownloadFile(MonoBehaviour mono, string url, System.Action<byte[]> onSuccess, System.Action<string> onFail)
        {
            if (string.IsNullOrEmpty(url))
                return;
            if (!url.StartsWith("http") && !url.StartsWith("HTTP") && ClientDataHandler.Instance)
            {
                Action<string> OutResourceLoad = (error) =>
                {
                    var outUrl = ClientDataHandler.Instance.RegisterResult.urlMap.outResourceUrl + url;
                    mono.StartCoroutine(AnsyLoadFile(outUrl, onSuccess, onFail, 60));
                };
                var inUrl = ClientDataHandler.Instance.RegisterResult.urlMap.inResourceUrl + url;
                mono.StartCoroutine(AnsyLoadFile(inUrl, onSuccess, OutResourceLoad, 180));
            }
            else
            {
                mono.StartCoroutine(AnsyLoadFile(url, onSuccess, onFail, 180));
            }
        }

        /// <summary>
        /// 下载音频文件
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="url"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        public static void DownloadAudio(MonoBehaviour mono, string url, System.Action<AudioClip> onSuccess, System.Action<string> onFail)
        {
            if (string.IsNullOrEmpty(url))
                return;
            if (!url.StartsWith("http") && !url.StartsWith("HTTP") && ClientDataHandler.Instance)
            {
                Action<string> OutResourceLoad = (error) =>
                {
                    var outUrl = ClientDataHandler.Instance.RegisterResult.urlMap.outResourceUrl + url;
                    mono.StartCoroutine(AnsyLoadAudio(url, onSuccess, onFail));
                };
                var inUrl = ClientDataHandler.Instance.RegisterResult.urlMap.inResourceUrl + url;
                mono.StartCoroutine(AnsyLoadAudio(url, onSuccess, OutResourceLoad, 3));
            }
            else
            {
                mono.StartCoroutine(AnsyLoadAudio(url, onSuccess, onFail));
            }
        }

        /// <summary>
        /// 加载音频文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        public static void LoadAudio(MonoBehaviour mono, string url, System.Action<AudioClip> onSuccess, System.Action<string> onFail)
        {
            mono.StartCoroutine(AnsyLoadAudio(url, onSuccess, onFail));
        }

        /// <summary>
        /// 异步加载音频文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        /// <returns></returns>
        public static IEnumerator AnsyLoadAudio(string url, System.Action<AudioClip> onSuccess, System.Action<string> onFail, int timeOut = 10)
        {
            var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            request.timeout = timeOut;
#if UNITY_2018
            yield return request.SendWebRequest();
#elif UNITY_2017
            yield return request.Send();
#endif
            if (request.isHttpError || request.isNetworkError)
            {
                if (onFail != null) onFail.Invoke(request.error);
            }
            else
            {
                var dlHandler = (DownloadHandlerAudioClip)request.downloadHandler;
                if (onSuccess != null) onSuccess.Invoke(dlHandler.audioClip);
            }
        }

        /// <summary>
        /// 下载图片资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onFinish"></param>
        /// <param name="onError"></param>
        public static void DownloadTexture(MonoBehaviour mono, string url, Action<Texture2D> onFinish, Action<string> onError)
        {
            if (string.IsNullOrEmpty(url)
                //|| !ClientDataHandler.Instance
                //|| null == ClientDataHandler.Instance.RegisterResult
                //|| null == ClientDataHandler.Instance.RegisterResult.urlMap
                )
            {
                if (null != onError) onError("网络初始化失败！");
                return;
            }
            if (!url.StartsWith("file://") && !url.StartsWith("http") && !url.StartsWith("HTTP") && ClientDataHandler.Instance)
            {
                Action<string> OutResourceLoad = (error) =>
                {
                    var outUrl = ClientDataHandler.Instance.RegisterResult.urlMap.outResourceUrl + url;
                    mono.StartCoroutine(AnsyDowloadTexture(outUrl, onFinish, onError));
                };
                var inUrl = ClientDataHandler.Instance.RegisterResult.urlMap.inResourceUrl + url;
                mono.StartCoroutine(AnsyDowloadTexture(inUrl, onFinish, OutResourceLoad, 3));
            }
            else
            {
                mono.StartCoroutine(AnsyDowloadTexture(url, onFinish, onError));
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onFinish"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IEnumerator AnsyDowloadTexture(string url, Action<Texture2D> onFinish, Action<string> onError, int timeOut = 10)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            request.timeout = timeOut;
#if UNITY_2018
            yield return request.SendWebRequest();
#elif UNITY_2017
            yield return request.Send();
#endif
            if (request.isHttpError || request.isNetworkError || !string.IsNullOrEmpty(request.error))
            {
                if (onError != null) onError.Invoke(request.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                if (!texture)
                {
                    if (onError != null) onError.Invoke("Donwload texture is empty.");
                }
                else
                {
                    if (onFinish != null) onFinish.Invoke(texture);
                }
            }
        }

        #endregion


        #region 上传部分


        #endregion


    }



}

