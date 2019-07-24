using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWork_lz.FSM;
public class TestFSM : MonoBehaviour {

    public enum GameState
    {
        Init,
        Idle,
        Active,
        Result,
    }
    private StateMachine<GameState> fsm;
	// Use this for initialization
	void Start () {

       
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.E))
        {
            fsm = StateMachine<GameState>.Initialize(this, GameState.Init);
        }	

	}


    private IEnumerator Init_Enter()
    {
        Debug.LogFormat("init animation");
        yield return new WaitForSeconds(0.5f);
        Debug.LogFormat("init uiRes");
        yield return new WaitForSeconds(0.5f);
        Debug.LogFormat("init Model");
        yield return new WaitForSeconds(0.5f);
        Debug.LogFormat("init Successed");
        fsm.ChangeState(GameState.Idle);
    }

    private void Idle_Enter()
    {
        Debug.LogFormat("enter Idle");
    }

    private void Idle_Exit()
    {
        Debug.LogFormat("Exit Idle");
    }

    private void Active_Enter()
    {
        Debug.LogFormat("Active enter ");
    }

    private void Active_Update()
    {
        Debug.LogFormat("Active Update ");
    }


    private void Active_Exit()
    {
        Debug.LogFormat("Active Exit ");
    }

    private void Result_Enter()
    {
        Debug.LogFormat("Result enter ");
    }

    private void Result_Exit()
    {
        Debug.LogFormat("Result Exit ");
    }


}
