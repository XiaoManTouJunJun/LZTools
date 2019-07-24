/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPU3DWebNetworkNS.NetworkNS;
using Newtonsoft.Json;
using G58NS.NewNetworkNS.DataNS;
using G58NS.NewNetworkNS;
using System.IO;
using System;


namespace FrameWork_lz
{
    public class NetManager : Singleton<NetManager>
    {



        // Use this for initialization
        void Start ()
        {

        }




        void OnEnable ()
        {
            ClientDataHandler.m_OnReceiveData += OnReceiveData;
            // 增加接收网络状态方法的委托
            ClientDataHandler.m_OnStatusInfoChange += OnStatusInfoChange;
            ClientDataHandler.m_OnGetRegisterResult += RegisterResult;
           
        }

        void OnDisable ()
        {
            ClientDataHandler.m_OnReceiveData -= OnReceiveData;
            // 移除接收网络状态方法的委托
            ClientDataHandler.m_OnStatusInfoChange -= OnStatusInfoChange;
            ClientDataHandler.m_OnGetRegisterResult = RegisterResult;
        }


        /// <summary>
        /// 上报服务器客户端当前状态
        /// </summary>
        /// <param name="stateId">0为待机1为激活</param>
        public void ReportState (int stateId)
        {
            if (stateId == 0) {//待机
                ClientDataHandler.ReportIdleStatus ();
            } else {
                ClientDataHandler.ReportActiveStatus ();
            }
           
        }


        /// <summary>
        /// Uploads the data handle.
        /// </summary>
        /// <param name="form">Form.</param>
        /// <param name="uploadFinishHandle">Upload finish handle.</param>
        /// <param name="uploadErrHandle">Upload error handle.</param>
        /// <param name="uploadPath">Upload path.</param>
        public void UploadDataHandle (Action<string,string> uploadFinishHandle, Action<string> uploadErrHandle, string  uploadPath, WWWForm form = null)
        {

            WebDataHandler.Instance.RequestWebTextData (uploadPath, uploadFinishHandle, true, form, false, uploadErrHandle);

        }


        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="uploadByte"></param>
        public void UploadVideoDataHandle (byte[] uploadByte, Action<string> errorHandle, Action<string,string> uploadFinishHandle, string uploadPath)
        {
            string url_upload = uploadPath;
            WWWForm upload_form = new WWWForm ();
            Debug.LogFormat ("video byte {0}", uploadByte.Length);
            upload_form.AddField ("type", 10);  
            upload_form.AddBinaryData ("picVideo", uploadByte, "movie.mp4", "video/mpeg4");
            //上传
            WebDataHandler.Instance.RequestWebTextData (url_upload, uploadFinishHandle, true, upload_form, false, errorHandle);

        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="uploadByte"></param>
        public void UploadVideoDataHandleForTV (byte[] uploadByte, Action<string> errorHandle, Action<string, string> uploadFinishHandle, string uploadPath)
        {
            string url_upload = uploadPath;
            WWWForm upload_form = new WWWForm ();
            Debug.LogFormat ("video byte {0}", uploadByte.Length);
            upload_form.AddField ("nodeId", QingPuNS.NewNetworkNS.ConfigNS.Config.m_ConfigData.RegisterConfigData.nodeId);

            upload_form.AddBinaryData ("picVideo", uploadByte, "movie.mp4", "video/mpeg4");
            //上传
            WebDataHandler.Instance.RequestWebTextData (url_upload, uploadFinishHandle, false, upload_form, false, errorHandle, true, true, WebDataHandler.EUrlType.INNER_URL);
        }


        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="uploadByte"></param>
        public void UploadPicDataHandle (byte[] uploadByte, Action<string> errorHandle, Action<string, string> uploadFinishHandle, string uploadPath)
        {
            string url_upload = uploadPath;
            WWWForm upload_form = new WWWForm ();
            Debug.LogFormat ("video byte {0}", uploadByte.Length);
            upload_form.AddField ("type", 8);
            upload_form.AddBinaryData ("picImg", uploadByte, "ScreenShot.jpg", "image/jpg");
            //上传
            WebDataHandler.Instance.RequestWebTextData (url_upload, uploadFinishHandle, true, upload_form, false, errorHandle);

        }

        public void UploadTravelViewPic (List<byte[]> texture2Ds, Action<string> errorHandle, Action<string, string> uploadFinishHandle, string uploadPath)
        {
            string url_upload = uploadPath;
            WWWForm upload_form = new WWWForm ();

            //GetVideoByte(System.Environment.CurrentDirectory + "/" + PathHelper.RelateProjectVideoPath);
            for (int i = 0; i < texture2Ds.Count; i++) {
                Debug.LogFormat ("screen byte {0}", texture2Ds [i].Length);
                if (i == 0) {
                    upload_form.AddBinaryData ("picImg", texture2Ds [i], "ScreenShot" + ".jpg", "image/jpg");
                } else {
                    upload_form.AddBinaryData ("picImg" + i.ToString (), texture2Ds [i], "ScreenShot" + i.ToString () + ".jpg", "image/jpg");
                }

                upload_form.AddField ("type", 9);
            }

            //上传
            WebDataHandler.Instance.RequestWebTextData (url_upload, uploadFinishHandle, true, upload_form, false, errorHandle);
        }


        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="uploadByte"></param>
        public void UploadVideoAndPicDataHandle (byte[] uploadByte, byte[] uploadVideo, Action<string> errorHandle, Action<string, string> uploadFinishHandle, string uploadPath)
        {
            string url_upload = uploadPath;
            WWWForm upload_form = new WWWForm ();
            Debug.LogFormat ("screen byte {0} video byte {1}", uploadByte.Length, uploadVideo.Length);
            upload_form.AddField ("type", 11);
            upload_form.AddBinaryData ("picImg", uploadByte, "ScreenShot.jpg", "image/jpg");//上传图片
            upload_form.AddBinaryData ("picVideo", uploadVideo, "movie.mp4", "video/mpeg4");
            //上传
            WebDataHandler.Instance.RequestWebTextData (url_upload, uploadFinishHandle, true, upload_form, false, errorHandle);

        }


        /// <summary>
        /// 自定义状态发送服务器
        /// </summary>
        public void SendCustomActiveStateHandle ()
        {
            CommonCommand cc = new CommonCommand ();
            cc.want = "DataTrans";

            DataTranCommand cc_child = new DataTranCommand ();
            cc_child.nodeIds = "FilmStudioV2GroundScreen";
            cc_child.dataStr = "active";

            string json1 = JsonConvert.SerializeObject (cc_child);
            cc.dataStr = json1;
            // 数据转换为Json字符串
            string json = JsonConvert.SerializeObject (cc);
            Debug.LogFormat ("json cc {0}", json);
            // 发送Json字符串
            ClientDataHandler.SendJsonStrToServer (json, NetworkCommand.TCP_U3D_SEND);
        }



        /// <summary>
        /// 注册成功
        /// </summary>
        /// <param name="networkId"></param>
        private void RegisterResult (string networkId)
        {
           
        }

 

   
 

        void OnReceiveData (int iCmd, int iCode, string sDataStr, string sMessage)
        {
            if (sDataStr == "") {
                return;
            }
            CommonCommand cc = JsonConvert.DeserializeObject<CommonCommand> (sDataStr);
            switch (cc.want) {
            case "SetScanSuccessServer"://MoveChangeStatus
                {
                    Debug.Log ("收到扫码成功数据：" + cc.dataStr);
                    //PosterInfo posterInfo = JsonConvert.DeserializeObject<PosterInfo>(cc.dataStr);
                    //Debug.LogFormat("posterInfo url {0}", posterInfo.imgUrl);      
                    break;
                }
            }
        }


        /// <summary>
        /// 反馈状态信息
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="errorInfo">状态信息</param>
        private void OnStatusInfoChange (NetStatusType statusType, string errorInfo)
        {
            switch (statusType) {
            // 连接时发生错误
            case NetStatusType.CONNECT_ERROR:
                Debug.Log (errorInfo);
                    //m_LogShower.text += errorInfo;
                break;
            // 连接成功
            case NetStatusType.CONNECTION_SUCCESSFULLY:
                Debug.Log (errorInfo);
                    // m_LogShower.text += errorInfo;
                break;
            // 断开连接
            case NetStatusType.LOSE_CONNECTION:
                Debug.Log (errorInfo);
                    // m_LogShower.text += errorInfo;
                break;
            // 重新连接中
            case NetStatusType.RECONNECTING:
                Debug.Log (errorInfo);
                    //m_LogShower.text += errorInfo;
                break;
            // 恢复连接（断线后重连成功时）
            case NetStatusType.RESTORE_CONNECTION:
                Debug.Log (errorInfo);
                    //m_LogShower.text += errorInfo;
                break;
            // 注册失败
            case NetStatusType.REGISTER_FAILED:
                Debug.Log (errorInfo);
                    // m_LogShower.text += errorInfo;
                break;
            // 注册成功
            case NetStatusType.REGISTER_SUCCEED:
                Debug.Log (errorInfo);
                    // m_LogShower.text += errorInfo;
                break;
            default:
                Debug.Log (errorInfo);
                    //m_LogShower.text += errorInfo;
                break;
            }
            //m_LogShower.text += "\n";
        }

    }

    /// <summary>
    /// 自定义状态
    /// </summary>
    public class DataTranCommand
    {
        public string nodeIds;
        public string dataStr;
    }

    public class ScanUploadData
    {
        public string codeUrl;
        //二维码图片
    }
}

*/