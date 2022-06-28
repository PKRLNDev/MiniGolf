using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Struct_LocalData : MonoBehaviour
{
    //\\\\\\\\\\\\ Input Module Variable ///////////////////////
    public Vector2 Touch_GetAxis_Raw = new Vector2(0, 0);
    public bool Touch_isTouched = false;
    public float Touch_Speed = 0;


    ////////////// Game Variables //////////////////////////////
    public float GolfBall_Rb_Sqr_Velocity = 0, Vertical_Aim_Angle = 0, GolfClub_Power_01 = 0;
    public bool bool_GolfBall_isShot = false;
    public bool bMoving = false;
}
