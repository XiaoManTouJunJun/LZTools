using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProMovieCapture;

namespace FrameWork_lz
{
    public class RecordVideoManager : Singleton<RecordVideoManager>
{





        [SerializeField]
        private CaptureBase ScreencaptureBase;
        [SerializeField]
        private CaptureBase cameraCaptureBase;
        [SerializeField]
        private CaptureBase captureBase;

   


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.LogFormat("开始录制视频");
                //starResultShareView.CutVideoScreenIMG();//截屏
                RecordVideoManager.Instance.StartRecording(0, "MagpieBridgeVideo");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.LogFormat("暂停录制视频");
                RecordVideoManager.Instance.PauseRecording(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.LogFormat("继续录制视频");
                RecordVideoManager.Instance.ResumeRecording(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.LogFormat("完成录制视频");
                RecordVideoManager.Instance.StopRecording(0);
            }
        }


        /// <summary>
        /// 开启待机分享录制
        /// </summary>
        /// <param name="recordId">0为屏幕录制 1相机录制</param>
        public void StartRecording(int recordId, string FolderName)
        {
            captureBase._outputFolderPath = FolderName;

            captureBase.StartCapture();
        }


        /// <summary>
        /// 恢复录制
        /// </summary>
        /// <param name="recordId">0为屏幕录制 1视频录制</param>
        public void ResumeRecording(int recordId)
        {
            if (captureBase != null)
                captureBase.ResumeCapture();
        }


        /// <summary>
        /// 暂停录制
        /// </summary>
        /// <param name="recordId">0为屏幕录制 1视频录制</param>
        public void PauseRecording(int recordId)
        {
            if (captureBase != null)
                captureBase.PauseCapture();
        }

        /// <summary>
        /// 结束录制
        /// </summary>
        /// <param name="recordId">0为屏幕录制 1视频录制</param>
        public void StopRecording(int recordId)
        {
            if (captureBase != null)
                captureBase.StopCapture();
        }



    }

}
