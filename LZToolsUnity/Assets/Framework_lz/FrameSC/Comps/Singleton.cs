using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// func: 基础单例模板类
/// autor: lz910608@gmail.com
/// time:2019.2.19.11:04
/// </summary>
namespace FrameWork_lz
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {

        private static readonly object syslock = new object();
        private static T _instance;
        public static T Instance {
            get
            {
                if(_instance == null)
                {
                    lock(syslock)
                    {
                        _instance = FindObjectOfType(typeof(T)) as T;
                        if(_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            obj.hideFlags = HideFlags.DontSave;
                            obj.hideFlags = HideFlags.HideAndDontSave;
                            _instance = (T)obj.AddComponent(typeof(T));
                        }
                    }
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if(_instance == null)
            {
                _instance = this as T;
            }else
            {
                Destroy(gameObject);
            }
        }



     
    }
}

