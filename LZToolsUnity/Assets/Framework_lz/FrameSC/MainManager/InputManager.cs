using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace FrameWork_lz
{
    /// <summary>
    /// 键盘输入管理器
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {


        public IObservable<Vector2> mouseV2{ get; private set; }

        public IObservable<bool> testInputKey{ get; private set; }

   

        private void Start()
        {
            LZLoger.InitColor();//注册调试颜色输出
            InputInitHandle();
        }


        void InputInitHandle ()
        {
            //鼠标坐标数据注册
            mouseV2 = this.UpdateAsObservable ()
                .Select (_ => {
                var x = Input.GetAxis ("Mouse X");
                var y = Input.GetAxis ("Mouse Y");
                return new Vector2 (x, y);
            });

            //测试按键
            testInputKey = this.UpdateAsObservable ()
                .Select (_ => {
                if (Input.GetKeyDown (KeyCode.T))
                    return true;
                else
                    return false;
            });
        }


  

    }
}

