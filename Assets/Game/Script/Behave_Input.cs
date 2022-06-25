using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behave_Input : MonoBehaviour
{
    [SerializeField]
    private Struct_LocalData Local_Data;
    private float Max_Swipe_Velocity = 6000;
    private Vector2 Touch_Axis_Raw = new Vector2(0, 0);
    private Touch touch;
    private Vector2 Current_Touch_Position_Value = new Vector2(0, 0), Previous_Touch_Position_Value = new Vector2(0, 0), Current_Touch_DeltaPosition_Mathf_Absolute_Value = new Vector2(0, 0);
    private bool bool_Touch_Detected = false, bool_Horizontal_Touch_Detected = false, bool_Vertical_Touch_Detected = false, bool_Stationary_Touch_Detected = false;
    private bool bool_PositiveX_Direction_Detected = false, bool_NegativeX_Direction_Detected = false, bool_PositiveY_Direction_Detected = false, bool_NegativeY_Direction_Detected = false;
    private float TouchSpeed = 0;


    void Update()
    {
        F_Touch_Detection();
        F_Save_Current_Touch_Absolute_Value();
        F_Detect_Touch_Drag_Axis();
        F_Detect_Touch_Drag_Direction();
        F_Save_Current_DeltaPosition_To_Previous_Delta_Position();
        F_Calculate_Touch_Result_Raw();
        F_Detect_Touch_Speed();
        F_Save_Results_To_Local_Data();
        F_Delete_Touch_Flags();
    }

    private void F_Touch_Detection()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && Input.touchCount > 0) // Input.GetKeyDown(key:KeyCode.Mouse0) : Input.touchCount > 0
        {
            touch = Input.GetTouch(0);
            Current_Touch_Position_Value = touch.position;
            bool_Touch_Detected = true;
        }
        else
        {
            bool_Touch_Detected = false;
        }
    }

    private void F_Save_Current_Touch_Absolute_Value()
    {
        Current_Touch_DeltaPosition_Mathf_Absolute_Value.x = Mathf.Abs(touch.deltaPosition.x);
        Current_Touch_DeltaPosition_Mathf_Absolute_Value.y = Mathf.Abs(touch.deltaPosition.y);
    }


    private void F_Detect_Touch_Drag_Axis()
    {
        if (bool_Touch_Detected)
        {
            if (Current_Touch_DeltaPosition_Mathf_Absolute_Value.x > Current_Touch_DeltaPosition_Mathf_Absolute_Value.y)
            {
                bool_Horizontal_Touch_Detected = true;
                bool_Vertical_Touch_Detected = false;
                bool_Stationary_Touch_Detected = false;
            }
            else if (Current_Touch_DeltaPosition_Mathf_Absolute_Value.x < Current_Touch_DeltaPosition_Mathf_Absolute_Value.y)
            {
                bool_Horizontal_Touch_Detected = false;
                bool_Vertical_Touch_Detected = true;
                bool_Stationary_Touch_Detected = false;
            }
            else
            {
                bool_Horizontal_Touch_Detected = false;
                bool_Vertical_Touch_Detected = false;
                bool_Stationary_Touch_Detected = true;
            }
        }
    }

    private void F_Detect_Touch_Drag_Direction()
    {
        if (bool_Horizontal_Touch_Detected)
        {
            if (Current_Touch_Position_Value.x > Previous_Touch_Position_Value.x)
            {
                bool_PositiveX_Direction_Detected = true;
                bool_NegativeX_Direction_Detected = false;
                bool_PositiveY_Direction_Detected = false;
                bool_NegativeY_Direction_Detected = false;
            }
            else if (Current_Touch_Position_Value.x < Previous_Touch_Position_Value.x)
            {
                bool_PositiveX_Direction_Detected = false;
                bool_NegativeX_Direction_Detected = true;
                bool_PositiveY_Direction_Detected = false;
                bool_NegativeY_Direction_Detected = false;
            }
            else
            {
                // undefined(never seen in simulation)
            }
        }
        else if (bool_Vertical_Touch_Detected)
        {
            if (Current_Touch_Position_Value.y > Previous_Touch_Position_Value.y)
            {
                bool_PositiveX_Direction_Detected = false;
                bool_NegativeX_Direction_Detected = false;
                bool_PositiveY_Direction_Detected = true;
                bool_NegativeY_Direction_Detected = false;
            }
            else if (Current_Touch_Position_Value.y < Previous_Touch_Position_Value.y)
            {
                bool_PositiveX_Direction_Detected = false;
                bool_NegativeX_Direction_Detected = false;
                bool_PositiveY_Direction_Detected = false;
                bool_NegativeY_Direction_Detected = true;
            }
            else
            {
                // undefined(never seen in simulation)
            }
        }
        else
        {
            // not needed (slot of stationary touch(done previously)
        }
    }


    private void F_Save_Current_DeltaPosition_To_Previous_Delta_Position()
    {
        Previous_Touch_Position_Value = Current_Touch_Position_Value;
    }

    private void F_Calculate_Touch_Result_Raw()
    {
        if (bool_PositiveX_Direction_Detected)
        {
            Touch_Axis_Raw = new Vector2(1, 0);
        }
        else if (bool_NegativeX_Direction_Detected)
        {
            Touch_Axis_Raw = new Vector2(-1, 0);
        }
        else if (bool_PositiveY_Direction_Detected)
        {
            Touch_Axis_Raw = new Vector2(0, 1);
        }
        else if (bool_NegativeY_Direction_Detected)
        {
            Touch_Axis_Raw = new Vector2(0, -1);
        }
        else
        {
            Touch_Axis_Raw = new Vector2(0, 0);
        }
    }


    private void F_Detect_Touch_Speed()
    {
        if ((Touch_Axis_Raw.x != 0) || (Touch_Axis_Raw.y != 0))
        {
            TouchSpeed = touch.deltaPosition.magnitude / touch.deltaTime;
            TouchSpeed = Mathf.Clamp(TouchSpeed, 0, Max_Swipe_Velocity);
            TouchSpeed = TouchSpeed / Max_Swipe_Velocity;
        }
        else
        {
            TouchSpeed = 0;
        }
    }

    private void F_Save_Results_To_Local_Data()
    {
        //////// Save Screen Touch Detection////////
        if (Input.GetAxisRaw("Fire1") > 0) //bool_touch_detected
        {
            Local_Data.Touch_isTouched = true;
            
        }
        else
        {
            Local_Data.Touch_isTouched = false;
        }
        //////// Save Screen Touch Detection////////

        ///////////// Save Touch Axis Raw///////////
        Local_Data.Touch_GetAxis_Raw = Touch_Axis_Raw;
        ///////////// Save Touch Axis Raw///////////

        ///////////// Save Touch Speed///////////
        Local_Data.Touch_Speed = TouchSpeed;
        ///////////// Save Touch Speed///////////

    }


    private void F_Delete_Touch_Flags()
    {
        bool_Touch_Detected = false;
        bool_Stationary_Touch_Detected = false;
        bool_Horizontal_Touch_Detected = false;
        bool_Vertical_Touch_Detected = false;
        bool_PositiveX_Direction_Detected = false;
        bool_NegativeX_Direction_Detected = false;
        bool_PositiveY_Direction_Detected = false;
        bool_NegativeY_Direction_Detected = false;
        // Donot clear fags of bool_Current_AxisX / Y
    }
}
