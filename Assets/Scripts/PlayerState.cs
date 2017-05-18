using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public static WalkingState walking;
    public static JumpingState jumping;
    public static OnLadderState onLadder;
    public static DeadState dead;

    public class WalkingState : PlayerState
    {
        void FixedUpdate()
        {
            
        }
    }

    public class JumpingState : PlayerState
    {
        void FixedUpdate()
        {

        }

    }

    public class OnLadderState : PlayerState
    {
        void FixedUpdate()
        {

        }
   
    }
        
    public class DeadState : PlayerState
    {
        void FixedUpdate()
        {

        }
   
    }
}
