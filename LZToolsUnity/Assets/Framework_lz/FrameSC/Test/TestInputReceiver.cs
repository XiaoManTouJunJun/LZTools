using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace FrameWork_lz
{
    public class TestInputReceiver : MonoBehaviour
    {

        // Use this for initialization
        void Start ()
        {
            RegistInputHandle ();
        }

        void RegistInputHandle ()
        {
            //接受鼠标数据
//            InputManager.Instance.mouseV2.Where (v2 => v2 != Vector2.zero)
//                .Subscribe (mouseVector => {
//                Debug.LogFormat ("Mouse x {0} mouse y {1}", mouseVector.x, mouseVector.y);
//            });


            //接受按键数据
            InputManager.Instance.testInputKey.Where (isOnClick => isOnClick == true)
                .Subscribe (isOnClick => {
                 
                Debug.LogFormat ("按到T键啦");
            });
        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                //AudioController.Instance.SoundPlay("ball1");
                AudioController.Instance.BGMPlay("bg01");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                //AudioController.Instance.SoundPlay("ball1");
                AudioController.Instance.BGMPlay("bg02");
            }
        }

    }
}

