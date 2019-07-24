using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using U3DConfigManager;

namespace FrameWork_lz
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager : MonoBehaviour
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static ConfigManager instance;

        /// <summary>
        /// 客户端配置
        /// </summary>
        public ClientConfigInfo clientConfig = new ClientConfigInfo();

        /// <summary>
        /// 初始化
        /// </summary>
        void Awake()
        {
            instance = this;
            // 添加配置监听
            if (ClientConfigManager.Instance != null)
            {
                ClientConfigManager.Instance.OnClientConfigUpdate += OnClientConfigUpdate;
            }
            // 启动时主动解析一次配置
            if (ClientConfigManager.Instance != null)
            {
                string config = ClientConfigManager.Instance.GetClientConfigStr();
                if (string.IsNullOrEmpty(config) == false)
                {
                    if (ResolveConfig(config))
                    {
                        StartCoroutine(ShowThemeFrameVideo());
                    }
                }
            }
        }



        /// <summary>
        /// 主循环
        /// </summary>
        void Update()
        {
            //调试按键
            //按键C:打印配置信息Json文本
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log(JsonConvert.SerializeObject(clientConfig, Formatting.Indented));

            }
        }

        /// <summary>
        /// 配置文件更新事件
        /// </summary>
        /// <param name="json"></param>
        private void OnClientConfigUpdate(string json)
        {
            Debug.Log("======> 客户端配置文件更新：OnClientConfigUpdate=" + json);
            //解析配置
            if (ResolveConfig(json))
            {
                //刷新程序
                RefreshProgram();
            }
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="json"></param>
        private bool ResolveConfig(string json)
        {
            try
            {
                // 转换Json字符串为数据对象
                ClientConfigInfo data = JsonConvert.DeserializeObject<ClientConfigInfo>(json);
                // 更新配置字段
                clientConfig = data;
            }
            catch (System.Exception error)
            {
                Debug.LogError("客户端配置文件解析出错，Error:" + error.ToString());
                return false;
            }
            return true;
        }



        /// <summary>
        /// 显示主题框视频
        /// </summary>
        private IEnumerator ShowThemeFrameVideo()
        {

            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (ConfigManager.instance != null)
                {
                    if (ConfigManager.instance.clientConfig != null)
                    {
                        break;
                    }
                }

            }

        }

        /// <summary>
        /// 刷新后台数据
        /// </summary>
        private void RefreshProgram()
        {

        }

    }

    #region 配置类

    /// <summary>
    /// 客户端配置信息
    /// </summary>
    public class ClientConfigInfo
    {
        /// <summary>
        /// 主题皮肤配置
        /// </summary>
        public ThemeSkinConfigInfo themeSkin = new ThemeSkinConfigInfo();


        public MagicForestConfigInfo clientName = new MagicForestConfigInfo();


    }

    /// <summary>
    /// 主题皮肤配置信息
    /// </summary>
    public class ThemeSkinConfigInfo
    {
        /// <summary>
        /// 当前主题类型
        /// 后端暂定：Normal("普通"),NewYears("元旦节"),Spring("春节"),Valentine("情人节"),Lantern("元宵节"),Women("妇女节");
        /// 后端切换主题配置时只会发一份当前主题配置给客户端，客户端一般不需判断主题类型，直接换装即可。
        /// </summary>
        public string theme = "Normal";
        /// <summary>
        /// 主题边框视频列表
        /// </summary>
        public List<string> borderVideoList = new List<string>();
        /// <summary>
        /// 弹窗文本列表
        /// </summary>
        public List<string> dialogList = new List<string>();
        /// <summary>
        /// 弹窗背景图片
        /// </summary>
        public string dialogBgImage = "";
        /// <summary>
        /// 弹窗弹出时间间隔
        /// </summary>
        public float dialogOpenTime = 10;

        /// <summary>
        /// 随机相框视频地址
        /// </summary>
        /// <returns></returns>
        public string RandomBorderVideoPath()
        {
            if(borderVideoList.Count>0)
            {
                int id = Random.Range(0, borderVideoList.Count);
                return borderVideoList[id];
            }
            return string.Empty;
        }

        /// <summary>
        /// 随机对话框
        /// </summary>
        /// <returns></returns>
        public string RandomDialogVal()
        {
            if(dialogList.Count>0)
            {
                int id = Random.Range(0, dialogList.Count);
                return dialogList[id];
            }
            return string.Empty;
        }
    }





    /// <summary>
    /// 悬挂屏AR配置
    /// </summary>
    public class MagicForestConfigInfo
    {
        public int time1;
        public int time2;
        public int time3;
        public int time4;
        public int time5;
        public int time6;

        public int point1;
        public int point2;
        public int point3;
        public int point4;
        public int point5;
        public int point6;

    }

    #endregion

}
