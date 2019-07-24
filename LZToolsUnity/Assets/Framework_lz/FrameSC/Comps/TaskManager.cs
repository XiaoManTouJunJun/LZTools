using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// func: 协程任务管理器
/// 
/// author: lz910608@gmail.com
/// 
/// time: 2019-1-29
/// </summary>
namespace FrameWork_lz
{
    public class Task
    {

        /// Returns true if and only if the coroutine is running.  Paused tasks


        /// are considered to be running.

        public bool Running
        {
            get
            {

                return task.Running;
            }
        }



        /// Returns true if and only if the coroutine is currently paused.


        public bool Paused
        {

            get
            {

                return task.Paused;

            }

        }



        /// Delegate for termination subscribers.  manual is true if and only if


        /// the coroutine was stopped with an explicit call to Stop().


        public delegate void FinishedHandler(bool manual);



        /// Termination event.  Triggered when the coroutine completes execution.


        public event FinishedHandler Finished;



        /// Creates a new Task object for the given coroutine.


        ///


        /// If autoStart is true (default) the task is automatically started


        /// upon construction.


        public Task(IEnumerator c, bool autoStart = true)
        {

            task = TaskManager.CreateTask(c);

            task.Finished += TaskFinished;

            if (autoStart)

                Start();
        }



        /// Begins execution of the coroutine


        public void Start()

        {

            task.Start();

        }



        /// Discontinues execution of the coroutine at its next yield.


        public void Stop()

        {

            task.Stop();

        }



        public void Pause()

        {

            task.Pause();

        }



        public void Unpause()
        {

            task.Unpause();

        }



        void TaskFinished(bool manual)

        {

            FinishedHandler handler = Finished;

            if (handler != null)

                handler(manual);

        }



        TaskManager.TaskState task;

    }



    class TaskManager : MonoBehaviour

    {

        public class TaskState

        {

            public bool Running
            {

                get
                {

                    return running;

                }

            }



            public bool Paused
            {

                get
                {

                    return paused;

                }

            }



            public delegate void FinishedHandler(bool manual);
            public event FinishedHandler Finished;


            IEnumerator coroutine;

            bool running;

            bool paused;

            bool stopped;


            public TaskState(IEnumerator c)

            {

                coroutine = c;

            }



            public void Pause()

            {

                paused = true;

            }



            public void Unpause()

            {

                paused = false;

            }



            public void Start()

            {

                running = true;

                singleton.StartCoroutine(CallWrapper());

            }



            public void Stop()

            {
                stopped = true;

                running = false;

            }



            IEnumerator CallWrapper()

            {

                yield return null;

                IEnumerator e = coroutine;

                while (running)
                {

                    if (paused)

                        yield return null;

                    else
                    {

                        if (e != null && e.MoveNext())
                        {

                            yield return e.Current;

                        }

                        else
                        {

                            running = false;

                        }

                    }

                }



                FinishedHandler handler = Finished;

                if (handler != null)

                    handler(stopped);
            }

        }



        static TaskManager singleton;



        public static TaskState CreateTask(IEnumerator coroutine)
        {

            if (singleton == null)
            {

                GameObject go = new GameObject("TaskManager");

                singleton = go.AddComponent<TaskManager>();

            }

            return new TaskState(coroutine);
        }
    }


    //使用示例
    /*       
     *       task = new Task(TestIE());
             task.Finished += TestIEFinish;

     *       
            void TestIEFinish(bool bl)
            {
                Debug.LogFormat("完成啦 ！！！{0}",bl);
            }

            IEnumerator TestIE()
            {
                yield return new WaitForSeconds(0.2f);
                Debug.LogFormat("开始进入协程");
                float interval = 3.0f;
                while(true)
                {
                    yield return new WaitForEndOfFrame();
                    Debug.LogFormat("****");
                    interval -= Time.deltaTime;
                    if (interval <= 0) break;
                }
                Debug.LogFormat("协程结束啦");

            } 

     */

}



