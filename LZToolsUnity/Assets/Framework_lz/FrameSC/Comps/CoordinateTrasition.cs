using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrameWork_lz
{
    /// <summary>
    /// 坐标转换
    /// </summary>
    public class CoordinateTrasition
    {


        /// <summary>
        /// 世界坐标转UGUI坐标
        /// </summary>
        /// <param name="worldTran">处当前世界Transform</param>
        /// <param name="canvasUI">当前UGUI canvas</param>
        /// <param name="curWorldCam">当前世界相机</param>
        /// <returns></returns>
        public static Vector2 WordPosToUguiPos(Transform worldTran,Canvas canvasUI,Camera curWorldCam)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasUI.transform as RectTransform,
                curWorldCam.WorldToScreenPoint(worldTran.position), canvasUI.worldCamera, out pos);

            return pos;
        }

        /// <summary>
        /// 世界坐标转UGUI坐标
        /// </summary>
        /// <param name="worldTran">处当前世界坐标</param>
        /// <param name="canvasUI">当前UGUI canvas</param>
        /// <param name="curWorldCam">当前世界相机</param>
        /// <returns></returns>
        public static Vector2 WordPosToUguiPos(Vector3 worldPos, Canvas canvasUI, Camera curWorldCam)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasUI.transform as RectTransform,
                curWorldCam.WorldToScreenPoint(worldPos), canvasUI.worldCamera, out pos);

            return pos;
        }


        /// <summary>
        /// 屏幕坐标转UGUI坐标
        /// </summary>
        /// <param name="screenPos">屏幕坐标</param>
        /// <param name="uiCanvas">当前UGUI  canvas</param>
        /// <param name="isCanvasUseCameraMode">当前canvas是否开启相机模式</param>
        /// <returns></returns>
        public static Vector2 ScreenPosToUguiPos(Vector3 screenPos, Canvas uiCanvas,bool isCanvasUseCameraMode)
        {
            

            RectTransform canvasRect = uiCanvas.GetComponent<RectTransform>();

            Camera curCam = isCanvasUseCameraMode ? uiCanvas.worldCamera : null;

            Vector2 outVec;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, curCam, out outVec))
            {
                Debug.Log("Setting anchored positiont to: " + outVec);

            }
            return outVec;
        }

    }
}


