
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork_lz
{
    public class Timer
    {
        static List<Timer> timers = new List<Timer>();
        Action<float> UpdateEvent;
        Action EndEvent;
        private float _time = -1;   // 用户设定的定时时长
        private bool _loop;          // 是否循环执行
        private bool _ignorTimescale;  // 是否忽略Timescale
        private string _flag;// 用户指定的定时器标志，便于手动清除、暂停、恢复
        private static TimerDriver driver = null;//拿驱动器的引用只是为了初始化驱动器
        private float CurrentTime { get { return _ignorTimescale ? Time.realtimeSinceStartup : Time.time; } }// 获得当前时间
        private float cachedTime;//缓存时间
        float timePassed;        //已经过去的时间
        private bool _isFinish = false; //计时器是否结束
        private bool _isPause = false; //计时器是否暂停

        private static bool showLog = true;  //确认是否输出Debug信息
        public static bool ShowLog { set { showLog = value; } }
        public bool IsPause // 暂停计时器
        {
            get { return _isPause; }
            set
            {
                if (value)
                {
                    Pause();
                }
                else
                {
                    Resum();
                }
            }

        }
        /// <summary>
        /// 构造定时器
        /// </summary>
        /// <param name="time">定时时长</param>
        /// <param name="flag">定时器标识符</param>
        /// <param name="loop">是否循环</param>
        /// <param name="ignorTimescale">是否忽略TimeScale</param>
        private Timer(float time, string flag, bool loop = false, bool ignorTimescale = true)
        {
            if (null == driver) driver = TimerDriver.Get; //初始化Time驱动
            _time = time;
            _loop = loop;
            _ignorTimescale = ignorTimescale;
            cachedTime = CurrentTime;
            if (timers.Exists((v) => { return v._flag == flag; }))
            {
                if (showLog) Debug.LogWarningFormat("【TimerTrigger（容错）】:存在相同的标识符【{0}】！", flag);
            }
            _flag = string.IsNullOrEmpty(flag) ? GetHashCode().ToString() : flag;//设置辨识标志符
        }
        private void Pause() // 暂停计时  
        {
            if (!_isFinish)
            {
                _isPause = true;
            }
        }
        private void Resum() // 继续计时  
        {
            if (!_isFinish && _isPause)
            {
                cachedTime = CurrentTime - timePassed;
                _isPause = false;
            }
        }
        private void Update() // 刷新定时器
        {
            if (!_isFinish && !_isPause) //运行中
            {
                timePassed = CurrentTime - cachedTime;
                if (null != UpdateEvent) UpdateEvent(Mathf.Clamp01(timePassed / _time));
                if (timePassed >= _time)
                {
                    if (null != EndEvent) EndEvent();
                    if (_loop)
                    {
                        cachedTime = CurrentTime;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }
        private void Stop() // 回收定时器
        {
            if (timers.Contains(this))
            {
                timers.Remove(this);
            }
            _time = -1;
            _isFinish = true;
            _isPause = false;
            UpdateEvent = null;
            EndEvent = null;
        }
        #region--------------------------静态方法扩展-------------------------------------
        #region-------------添加定时器---------------
        /// <summary>
        /// 添加定时触发器
        /// </summary>
        /// <param name="time">定时时长</param>
        /// <param name="flag">定时器标识符</param>
        /// <param name="loop">是否循环</param>
        /// <param name="ignorTimescale">是否忽略TimeScale</param>
        public static Timer AddTimer(float time, string flag = "", bool loop = false, bool ignorTimescale = true)
        {
            Timer timer = new Timer(time, flag, loop, ignorTimescale);
            timers.Add(timer);
            return timer;
        }
        #endregion

        #region-------------刷新所有定时器---------------
        public static void UpdateAllTimer()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                if (null != timers[i])
                {
                    timers[i].Update();
                }
            }
        }
        #endregion

        #region-------------暂停和恢复定时器---------------
        /// <summary>
        /// 暂停用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的标识符</param>
        public static void PauseTimer(string flag)
        {
            Timer timer = timers.Find((v) => { return v._flag == flag; });
            if (null != timer)
            {
                timer.Pause();
            }
        }
        /// <summary>
        /// 恢复用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的标识符</param>
        public static void ResumTimer(string flag)
        {
            Timer timer = timers.Find((v) => { return v._flag == flag; });
            if (null != timer)
            {
                timer.Resum();
            }
        }

        #endregion
        #region-------------删除定时器---------------
        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的标识符</param>
        public static void DelTimer(string flag)
        {
            Timer timer = timers.Find((v) => { return v._flag == flag; });
            if (null != timer)
            {
                timer.Stop();
            }
        }
        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的定时器</param>
        public static void DelTimer(Timer timer)
        {
            if (timers.Contains(timer))
            {
                timer.Stop();
            }
        }
        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="completedEvent">指定的完成事件(直接赋值匿名函数无效)</param>
        public static void DelTimer(Action completedEvent)
        {
            Timer timer = timers.Find((v) => { return v.EndEvent == completedEvent; });
            if (null != timer)
            {
                timer.Stop();
            }
        }
        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="updateEvent">指定的Update事件(直接赋值匿名函数无效)</param>
        public static void DelTimer(Action<float> updateEvent)
        {
            Timer timer = timers.Find((v) => { return v.UpdateEvent == updateEvent; });
            if (null != timer)
            {
                timer.Stop();
            }
        }
        #endregion
        #endregion

        #region-------------添加事件-------------------
        public Timer OnCompleted(Action completedEvent) //添加完成事件
        {
            if (null == EndEvent)
            {
                EndEvent = completedEvent;
            }
            return this;
        }
        public Timer OnUpdated(Action<float> updateEvent) //添加update更新事件
        {
            if (null == UpdateEvent)
            {
                UpdateEvent = updateEvent;
            }
            return this;
        }

        #endregion

        #region ---------------运行中的定时器参数修改-----------
        public void Setloop(bool loop) // 设置运行中的定时器的loop状态
        {
            if (!_isFinish)
            {
                _loop = loop;
            }
        }
        public void SetIgnoreTimeScale(bool ignoreTimescale)// 设置运行中的定时器的ignoreTimescale状态
        {
            if (!_isFinish)
            {
                _ignorTimescale = ignoreTimescale;
            }
        }
        #endregion

    }

    public class TimerDriver : MonoBehaviour
    {
        #region 单例
        private static TimerDriver _instance;
        public static TimerDriver Get
        {
            get
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType<TimerDriver>() ?? new GameObject("TimerEntity").AddComponent<TimerDriver>();
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        private void Update()
        {
            Timer.UpdateAllTimer();
        }
    }
}

