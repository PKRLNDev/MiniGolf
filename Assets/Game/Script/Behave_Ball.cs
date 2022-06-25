using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Behave_Ball : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Component_AudioComp AudioComp;
    [SerializeField]
    private Struct_LocalData LocalData;
    [SerializeField]
    private LineRenderer LineRenderer;
    [SerializeField]
    private Transform Camera_Transform, Original_GolfBall_Transform, GolfBall_LookAt_Camera_Transform;
    [SerializeField]
    private GameObject LineRenderer_GO;
    //[SerializeField]
    //private Slider Power_Slider;
    [SerializeField]
    private float Club_Max_Power;
    [SerializeField]
    private float GolfBall_Speed_Decrasing_Velocity;
    [SerializeField]
    private float Brake_Angular_Drag = 3f;

    [SerializeField]
    private LayerMask CollidableLayers;
    private float Normal_Angular_Drag = 0.5f;
    private float Sphere_Overlap_Radius = 0.085f;



    private float GolfBall_StopSpeed = 0.00001f;
    private float HitMagnitude = 0;
    private float GolfBall_Linear_Velocity = 0;
    private float GolfBall_Angular_Velocity = 0;
    private float Golf_Ball_Rolling_Audio_Pitch = 0;

    private Rigidbody GolfBall_Rb;
    private bool bool_GolfBall_isMoving = false;
    private bool bool_GolfBall_isShot = false;
    private bool bool_Reseting_Power_Slider = false;
    private int hitColliders = 0, Roll_Countdown = 20;
    Collider[] hitCollider = new Collider[2];
    private float Power_Effectivity = 0, Shoot_Time_Power_Effectivity = 0;

    private bool bool_Golf_Ball_hit = false;
    private bool bool_GolfBall_Bounce_Sound_isPlayed = false;
    private bool bool_GolfBall_Shoot_Sound_isPlayed = true;
    private bool bool_GolfBall_Rolling_Sound_isPlayed = false;

    #endregion

    #region ShootInput

    private Vector2? InitTouchLocation;
    private Vector2 EndTouchLocation;
    private Vector2 GrabPos;

    #endregion



    // Start is called before the first frame update
    private void Start()
    {
        GolfBall_Rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -20, 0);
        GolfBall_Rb.maxAngularVelocity = 50000;
    }
    private void FixedUpdate()
    {
        F_Find_GolfBall_Linear_And_Angular_Velocity();
        F_Determine_GolfBall_Movement();
        F_Lerp_GolfBallAngular_Drag();
        F_Aim_Horizontal();
        F_Set_Golf_LookAt_Position_To_Real_Golf_Ball_Position();
        F_Create_Trajectory_Line();
        //F_Create_Shot_Effect();
        F_Detect_Ground_Touch();
        F_Play_Golf_Audio();
        F_Adjust_Ball_Rolling_Pitch_Sound();

        GrabPull();

    }

    private void GrabPull()
    {
        if (Input.GetAxisRaw("Fire1")>0)
        {
            if (InitTouchLocation.HasValue)
            {
                EndTouchLocation = Input.mousePosition;
                HitMagnitude = Vector2.Distance(GrabPos, EndTouchLocation);
                Debug.Log("Hit Magnitude = " + HitMagnitude.ToString());
                Debug.LogWarning("GrabPos = " + GrabPos.ToString());
                Debug.LogError("EndGrabPos = " + EndTouchLocation.ToString());

                return;
            }else if (TraceForSelection().HasValue)
            {
                GrabPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                InitTouchLocation = GrabPos;
                GrabPull();
            }
        }
        else
        {
            if (InitTouchLocation.HasValue)
            {
                CalcPower(HitMagnitude);
                F_Shoot();
                InitTouchLocation = null;
            }
        }
    }

    private void F_Find_GolfBall_Linear_And_Angular_Velocity()
    {
        GolfBall_Linear_Velocity = GolfBall_Rb.velocity.sqrMagnitude;
        GolfBall_Angular_Velocity = GolfBall_Rb.angularVelocity.sqrMagnitude;
    }



    private void F_Determine_GolfBall_Movement()
    {

        if (GolfBall_Linear_Velocity <= GolfBall_StopSpeed)
        {
            bool_GolfBall_isMoving = false;
            bool_GolfBall_isShot = false;
            LocalData.bool_GolfBall_isMoving = false;
            LocalData.bool_GolfBall_isShot = false;
            //Power_Slider.interactable = true;
        }
        else
        {
            //Power_Slider.interactable = false;
            LocalData.bool_GolfBall_isMoving = true;
            bool_GolfBall_isMoving = true;
        }
    }

    private void F_Lerp_GolfBallAngular_Drag()
    {
        if (bool_GolfBall_isMoving)
        {
            if (GolfBall_Linear_Velocity < GolfBall_Speed_Decrasing_Velocity)
            {
                GolfBall_Rb.angularDrag = Mathf.MoveTowards(GolfBall_Rb.angularDrag, Brake_Angular_Drag, Time.deltaTime);
            }
        }
        else
        {
            GolfBall_Rb.angularDrag = Normal_Angular_Drag;
        }
    }


    private void F_Aim_Horizontal()
    {
        GolfBall_LookAt_Camera_Transform.LookAt(new Vector3(Camera_Transform.position.x, GolfBall_LookAt_Camera_Transform.position.y, Camera_Transform.position.z));
    }

    private void F_Set_Golf_LookAt_Position_To_Real_Golf_Ball_Position()
    {
        GolfBall_LookAt_Camera_Transform.position = Original_GolfBall_Transform.position;
    }

    private void F_Create_Trajectory_Line()
    {
        if (!bool_GolfBall_isMoving)
        {
            if (!LineRenderer_GO.activeInHierarchy)
            {
                LineRenderer_GO.SetActive(true);
            }
            LineRenderer.SetPosition(0, new Vector3(0, 0, 0));
            LineRenderer.SetPosition(1, new Vector3(0, 0, -2));
        }
        else
        {
            if (LineRenderer_GO.activeInHierarchy)
            {
                LineRenderer_GO.SetActive(false);
            }
        }
    }

    // this is just the visual drainage effect of slider Might move to an inworld graphic
    /*
    private void F_Create_Shot_Effect()
    {
        if (Power_Slider.value != 0 && bool_Reseting_Power_Slider)
        {
            Power_Slider.value = Mathf.Lerp(Power_Slider.value, 0, Time.deltaTime * 35);
        }
        else
        {
            bool_Reseting_Power_Slider = false;
        }
 
    }

     */
    private void F_Detect_Ground_Touch()
    {
        hitColliders = Physics.OverlapSphereNonAlloc(GolfBall_LookAt_Camera_Transform.position, Sphere_Overlap_Radius, hitCollider, CollidableLayers);
        if (hitColliders > 0)
        {
            if (!bool_Golf_Ball_hit)
            {
                bool_Golf_Ball_hit = true;
            }
        }
        else
        {
            if (bool_Golf_Ball_hit)
            {
                bool_Golf_Ball_hit = false;
            }
        }
    }

    private void F_Play_Golf_Audio()
    {
        /////////////// Play GolfBall Bounce Sound//////////////////
        if (bool_Golf_Ball_hit && !bool_GolfBall_Bounce_Sound_isPlayed)
        {
            bool_GolfBall_Bounce_Sound_isPlayed = true;
            AudioComp.PlaySound_Ball_Bounce();
        }
        else if (!bool_Golf_Ball_hit)
        {
            bool_GolfBall_Bounce_Sound_isPlayed = false;
        }
        /////////////// Play GolfBall Bounce Sound//////////////////


        /////////////// Play GolfBall Shot Sound//////////////////
        if (!bool_GolfBall_Shoot_Sound_isPlayed)
        {
            bool_GolfBall_Shoot_Sound_isPlayed = true;

            if (Shoot_Time_Power_Effectivity < 0.2f)
            {
                AudioComp.PlaySound_Ball_Shot_Lowest();
            }
            else if (Shoot_Time_Power_Effectivity < 0.4f)
            {
                AudioComp.PlaySound_Ball_Shot_Lower();
            }
            else if (Shoot_Time_Power_Effectivity < 0.6f)
            {
                AudioComp.PlaySound_Ball_Shot_Medium();
            }
            else if (Shoot_Time_Power_Effectivity < 0.8f)
            {
                AudioComp.PlaySound_Ball_Shot_Higher();
            }
            else
            {
                AudioComp.PlaySound_Ball_Shot_Highest();
            }
        }
        /////////////// Play GolfBall Shot Sound//////////////////

        /////////////// Play GolfBall Rolling Sound  //////////////////
        if (bool_Golf_Ball_hit && bool_GolfBall_isMoving && !bool_GolfBall_Rolling_Sound_isPlayed)
        {
            if (Roll_Countdown < 0)
            {
                bool_GolfBall_Rolling_Sound_isPlayed = true;
                AudioComp.PlaySound_Ball_Rolling();
            }
            else
            {
                Roll_Countdown--;
            }
        }
        else if (!bool_Golf_Ball_hit && bool_GolfBall_Rolling_Sound_isPlayed || !bool_GolfBall_isMoving && bool_GolfBall_Rolling_Sound_isPlayed)
        {
            bool_GolfBall_Rolling_Sound_isPlayed = false;
            Roll_Countdown = 20;
            AudioComp.Stop_Playing_Rolling_Sound();
        }
        ///////////////  Play GolfBall Rolling Sound //////////////////
    }

    private void F_Adjust_Ball_Rolling_Pitch_Sound()
    {
        Golf_Ball_Rolling_Audio_Pitch = 0.25f + (GolfBall_Angular_Velocity * 0.0000375f);         //lerp between 0.6 and 0.3 , max 8k, min <10
        Golf_Ball_Rolling_Audio_Pitch = Mathf.Clamp(Golf_Ball_Rolling_Audio_Pitch, 0.25f, 0.7f);
        AudioComp.Set_Golf_Roll_Pitch(Golf_Ball_Rolling_Audio_Pitch);
    }

    public void F_Shoot()
    {
        GolfBall_Rb.AddForce(-GolfBall_LookAt_Camera_Transform.forward * HitMagnitude, ForceMode.Force);
        //GolfBall_Rb.AddForce(-GolfBall_LookAt_Camera_Transform.forward * 2, ForceMode.Force);
        LocalData.bool_GolfBall_isShot = true;
        bool_GolfBall_isShot = true;
        bool_Reseting_Power_Slider = true;
        bool_GolfBall_Shoot_Sound_isPlayed = false;
        Shoot_Time_Power_Effectivity = Power_Effectivity;
    }


    public void CalcPower(float Power_Sensitivity)
    {
        Power_Effectivity = Power_Sensitivity;
        HitMagnitude = Power_Effectivity * Club_Max_Power;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GolfBall_LookAt_Camera_Transform.position, -GolfBall_LookAt_Camera_Transform.forward * 2);
        Gizmos.DrawWireSphere(GolfBall_LookAt_Camera_Transform.position, Sphere_Overlap_Radius);
    }


    private Vector3? TraceForSelection() 
    {
        Vector3 PosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 PosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);


        Vector3 ScreentoWorldFar = Camera.main.ScreenToWorldPoint(PosFar);
        Vector3 ScreentoWorldNear = Camera.main.ScreenToWorldPoint(PosNear);


        RaycastHit hitResult;
        if (Physics.Raycast(ScreentoWorldNear,ScreentoWorldFar-ScreentoWorldNear,out hitResult,float.PositiveInfinity))
        {
            if (hitResult.transform == transform)
            {
                InitTouchLocation = hitResult.transform.position;
                return InitTouchLocation;
            }
        }
        
        return null;
    }

}

