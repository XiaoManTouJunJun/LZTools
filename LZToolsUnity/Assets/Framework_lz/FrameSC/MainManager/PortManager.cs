using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeSerialPort;
using System;
using UnityEngine.Events;

/// <summary>
/// func: 硬件控制管理器
/// 
/// author: lz910608@gmail.com
/// 
/// time: 2019-1-29
/// 
/// </summary>
namespace FrameWork_lz
{
    public class PortManager : Singleton<PortManager>
    {




        private UnityEvent onClickNormalHandle;

        private Queue<QingPuHardwareDevCtrlType> type_queue = new Queue<QingPuHardwareDevCtrlType>();

        #region ================= Unity =====================
  

        // Use this for initialization
        void Start()
        {
            Init();
        }


        #endregion


        



        /// <summary>
        /// 初始化连接
        /// </summary>
        void Init()
        {
            //初始化连接
            QingPuHardwareManager.instance.Init(true);//参数true表示使用socket通信，false表示直连串口(COM8)
                                                      //接收数据事件
            QingPuHardwareManager.instance.OnDataReceived += OnHardwareDataReceived;
        }




        /// <summary>
        /// 发送单个硬件设备
        /// </summary>
        /// <param name="qingPuHardwareDevCtrlType"></param>
        /// <param name="dev_id"></param>
        public void SendOpenHardware(QingPuHardwareDevCtrlType qingPuHardwareDevCtrlType, int dev_id)
        {
            QingPuHardwareManager.instance.SendOpenHardware(qingPuHardwareDevCtrlType);
        }

        /// <summary>
        /// 关闭单个硬件设备
        /// </summary>
        /// <param name="qingPuHardwareDevCtrlType"></param>
        /// <param name="dev_id"></param>
        public void SendCloseHardware(QingPuHardwareDevCtrlType qingPuHardwareDevCtrlType, int dev_id = 0)
        {
            QingPuHardwareManager.instance.SendCloseHardware(qingPuHardwareDevCtrlType);
        }


        /// <summary>
        /// 发送硬件队列
        /// </summary>
        /// <param name="qingPuHardwareDevCtrlType">设备类型</param>
        /// <param name="dev_id">设备id</param>
        /// <param name="interval">间隔时间</param>
        public void SendOpenHardwareQue(QingPuHardwareDevCtrlType qingPuHardwareDevCtrlType, float interval, int dev_id = 0)
        {
            QingPuHardwareManager.instance.SendOpenHardware(qingPuHardwareDevCtrlType, dev_id);
            type_queue.Enqueue(qingPuHardwareDevCtrlType);
            Invoke("CloseHardAction", interval);
            Debug.LogFormat("ss {0}", qingPuHardwareDevCtrlType);
        }

        /// <summary>
        /// 队列关闭硬件设备
        /// </summary>
        void CloseHardAction()
        {
            QingPuHardwareManager.instance.SendCloseHardware(type_queue.Dequeue());
        }



        /// <summary>
        /// 发送硬件数据
        /// </summary>
        private void SendHardwareData()
        {
            //---便捷发送数据---
            //开启风机1
            QingPuHardwareManager.instance.SendOpenHardware(QingPuHardwareDevCtrlType.Fan, 1);
            //关闭风机1
            QingPuHardwareManager.instance.SendCloseHardware(QingPuHardwareDevCtrlType.Fan, 1);

            //---自定义发送数据---
            //方式1：通过代码构建协议数据
            string data = QingPuHardwareProtocolUtil.BuildPacketData(
                QingPuHardwareMsgType.HardwareDevCtrlMsg.ToString(),
                QingPuHardwareDevCtrlType.Fan.ToString(), 1, 1);
            //方式2：直接编写原始数据
            //string data = "*HardwareDevCtrlMsg;Fan,1&0,2&1;Bubble,1&0#";
            //发送data数据
            QingPuHardwareManager.instance.Send(data);
        }

        /// <summary>
        /// 收到硬件数据
        /// </summary>
        /// <param name="data"></param>
        private void OnHardwareDataReceived(string data)
        {
            //UI显示
            Debug.LogFormat(string.Format("[{0}] \n {1}", DateTime.Now.ToString("HH:mm:ss"), data));
            //测试数据
            //data = "*HardwareBtnMsg;Button,1&0,2&1#";

            //---解析协议---
            var myData = QingPuHardwareProtocolUtil.ParsePacketData(data);
            print("解析出来的协议数据内容：" + myData.ToString());

            //判断处理按钮消息
            if (myData.msgType == QingPuHardwareMsgType.HardwareBtnMsg.ToString())
            {
                //设备列表
                foreach (var dev in myData.devList)
                {
                    //普通按钮
                    if (dev.devType == QingPuHardwareBtnType.Button.ToString())
                    {
                        Debug.LogFormat("收到普通按钮消息 >>> 按钮编号:{0}，按下状态:{1}", dev.devNum, int.Parse(dev.devParams[0]) == 1);

                        if (onClickNormalHandle != null) onClickNormalHandle.Invoke();//启动普通按钮回调

                        if (dev.devNum == 1)
                        {
                            int start_id = int.Parse(dev.devParams[0]);
                            if (start_id == 1) //
                            {
                                Debug.LogFormat("按下按钮");
                            }

                        }
                    }
                }
            }

            //判断处理传感器消息
            if (myData.msgType == QingPuHardwareMsgType.HardwareSensorMsg.ToString())
            {
                //设备列表
                foreach (var dev in myData.devList)
                {
                    //压力传感器
                    if (dev.devType == QingPuHardwareSensorType.Press.ToString())
                    {
                        Debug.LogFormat("收到压力传感器消息 >>> 编号:{0}，数据:{1}", dev.devNum, float.Parse(dev.devParams[0]));
                    }
                    //LED灯颜色
                    if (dev.devType == QingPuHardwareSensorType.LedColor.ToString())
                    {
                        Debug.LogFormat("收到LED灯颜色消息 >>> 编号:{0}，数据:{1}", dev.devNum, dev.devParams[0]);
                    }
                }
            }
        }

    }
}


