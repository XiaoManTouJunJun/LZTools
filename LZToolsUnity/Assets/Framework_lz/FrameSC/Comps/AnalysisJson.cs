using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using QPU3DWebNetworkNS.NetworkNS;
/// <summary>
/// func: json解析器
/// 
/// author: lz910608@gmail.com
/// 
/// time: 2019-1-30
/// </summary>
namespace FrameWork_lz
{
    public enum WebResType
    {
        RawData,
        String,
        Texture
    }

    public class AnalysisJson
    {

        public Action<string, string> successLoadStrHandle;
        public Action<byte[]> successLoadRawDataHandle;
        public Action<Texture2D> successLoadTexHandle;
        public Action<string> errorLoadHandle;
        
        public void GetWebJsonData(string webUrl,WWWForm dataForm, WebResType webResType)
        {
            switch(webResType)
            {
                case WebResType.RawData:
                    WebDataHandler.Instance.RequestWebRawData(webUrl, successLoadRawDataHandle, true,errorLoadHandle,dataForm);
                    break;
                case WebResType.String:
                    WebDataHandler.Instance.RequestWebTextData(webUrl, successLoadStrHandle, dataForm, errorLoadHandle);
                    break;
                case WebResType.Texture:
                    WebDataHandler.Instance.RequestWebTexData(webUrl, successLoadTexHandle,true , errorLoadHandle, dataForm);
                    break;
                default:
                    break;
            }
                       
        }
 
        
     
    }

}

