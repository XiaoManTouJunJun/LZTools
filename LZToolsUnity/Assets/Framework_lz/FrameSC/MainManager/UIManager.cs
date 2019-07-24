using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// func: UI总资源管理器
/// author: lz910608@gmail.com
/// time: 2019-1-29
/// </summary>
namespace FrameWork_lz
{
    public class UIManager : MonoBehaviour
    {

        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        #region ========================== Unity ====================================
        void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }


        #endregion
    }
}

