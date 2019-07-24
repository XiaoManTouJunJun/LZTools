using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWork_lz.FSM;

namespace FrameWork_lz
{
    public class GameMainManager : Singleton<GameMainManager>
    {

 
        public MainState mainState = MainState.Idle;
        private StateMachine<MainState> fsm;   
        // Use this for initialization
        void Start ()
        {
            fsm = StateMachine<MainState>.Initialize (this, MainState.Idle);
        }

        /// <summary>
        /// 设置当前状态
        /// </summary>
        public void SetCurState(MainState state)
        {
            mainState = state;
            fsm.ChangeState(mainState);
        }



        #region  === 状态机接口实例 ===



        private void Idle_Enter ()
        {
            Debug.LogFormat ("进入待机状态");
        }

        private void Idle_Update ()
        {
            Debug.LogFormat ("待机中。。。");
        }

        private void Idle_Exit ()
        {
            Debug.LogFormat ("退出待机 ");
        }

 

        private IEnumerator Active_Enter ()
        {
            yield return new WaitForEndOfFrame ();
            Debug.LogFormat ("游戏状态");
        }

 

        private void Active_Uptade ()
        {
            Debug.LogFormat ("游戏中。。。");
        }

        private void Active_Exit ()
        {
            Debug.LogFormat ("退出游戏状态");
        }


        private void End_Enter ()
        {
            Debug.LogFormat ("进入游戏结束状态");
        }

        private void End_Exit ()
        {
            Debug.LogFormat ("退出游戏结束状态");
        }

        #endregion


    }

    public enum MainState
    {
        Idle,
        Active,
        End
    }
}



