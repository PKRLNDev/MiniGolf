using UnityEngine;
using Cinemachine;

public class Component_CameraMan : MonoBehaviour
{
    [SerializeField]
    private Struct_LocalData Local_Data;
    [SerializeField]
    private CinemachineFreeLook CM_FreeLook_Cam;

    [Range(0.0001f, 0.8f)]
    [SerializeField]
    private float Sensitivity = 0.2f;

    private float CM_X_Axis_Default_MaxSpeed, CM_X_Axis_Adjusted_MaxSpeed;
    private bool bool_TouchScreen_Rotation_isActive = false;





    private void Awake()
    {
        CM_X_Axis_Default_MaxSpeed = CM_FreeLook_Cam.m_XAxis.m_MaxSpeed;
        CM_X_Axis_Adjusted_MaxSpeed = CM_X_Axis_Default_MaxSpeed * Sensitivity;
    }


    private void Update()
    {
        CM_X_Axis_Adjusted_MaxSpeed = CM_X_Axis_Default_MaxSpeed * Sensitivity;
        F_Rotate_Cam_With_CineMachine();
    }



    private void F_Rotate_Cam_With_CineMachine()
    {
        if (bool_TouchScreen_Rotation_isActive)
        {
            /////// Set The Touch Value To CM FreeLook///////
            CM_FreeLook_Cam.m_XAxis.m_InputAxisValue = -Local_Data.Touch_GetAxis_Raw.x;
            /////// Set The Touch Value To CM FreeLook///////
     
            //////// Set Cinemachine Speed //////////////////
            CM_FreeLook_Cam.m_XAxis.m_MaxSpeed = CM_X_Axis_Adjusted_MaxSpeed * Local_Data.Touch_Speed;
            //////// Set Cinemachine Speed //////////////////                
        }
        else
        {
            /////// Set The Touch Value To CM FreeLook///////
            CM_FreeLook_Cam.m_XAxis.m_InputAxisValue = 0;
            /////// Set The Touch Value To CM FreeLook///////
        }
    }

    public void F_Adjust_Sensitivity(float new_Sensitivity)
    {
        Sensitivity = new_Sensitivity;
    }

    public void F_Enable_Touch_Screen_Rotation()
    {
        bool_TouchScreen_Rotation_isActive = true;
    }

    public void F_Disable_Touch_Screen_Rotation()
    {
        bool_TouchScreen_Rotation_isActive = false;
    }

}
