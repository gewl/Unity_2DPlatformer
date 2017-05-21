//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerState : MonoBehaviour {

//    public static WalkingState walking;
//    public static InvulnState invuln;
//    public static JumpingState jumping;
//    public static OnLadderState onLadder;
//    public static DeadState dead;

//    public class WalkingState : PlayerState
//    {
//        public void FixedUpdate(Rigidbody2D rb)
//        {
//            if (Input.GetKey(KeyCode.D))
//            {
//                rb.AddForce(transform.right * 20f);
//            }
//            else if (Input.GetKey(KeyCode.A))
//            {
//                rb.AddForce(transform.right * -20f);
//            }
//        }
//    }

//    public class InvulnState : PlayerState
//    {
//        void FixedUpdate()
//        {

//        }
//    }

//    public class JumpingState : PlayerState
//    {
//        void FixedUpdate()
//        {
//        }

//    }

//    public class OnLadderState : PlayerState
//    {
//        void FixedUpdate()
//        {

//        }
   
//    }
        
//    public class DeadState : PlayerState
//    {
//        void FixedUpdate()
//        {

//        }
   
//    }
//}
