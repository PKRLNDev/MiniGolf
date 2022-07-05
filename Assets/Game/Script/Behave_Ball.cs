using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Behave_Ball : MonoBehaviour, IMiniGolf
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

    private float Sphere_Overlap_Radius = 0.085f;


    [SerializeField]
    private float GolfBall_Bounce = 0.5f;
    private float GolfBall_StopSpeed = 0.0005f;
    private float HitMagnitude = 0;

    private float Golf_Ball_Rolling_Audio_Pitch = 0;

    private Rigidbody GolfBall_Rb;
    private bool bMoving = false;
    //private bool bool_GolfBall_isShot = false;
    //private bool bool_Reseting_Power_Slider = false;
    private int hitColliders = 0, Roll_Countdown = 20;
    Collider[] hitCollider = new Collider[2];
    private float Power_Effectivity = 0, Shoot_Time_Power_Effectivity = 0;

    private bool bool_Golf_Ball_hit = false;
    private bool bool_GolfBall_Bounce_Sound_isPlayed = false;
    private bool bool_GolfBall_Shoot_Sound_isPlayed = true;
    private bool bool_GolfBall_Rolling_Sound_isPlayed = false;
    private bool bBallReady = false;
    private bool bSunk = false;

    #endregion

    #region TouchInput

    private Vector2? InitTouchLocation;
    private Vector2 EndTouchLocation;
    private Vector2 GrabPos;

    bool bBallGrabbed= false;

    [SerializeField]
    Cinemachine.CinemachineFreeLook CinemachineRig;


    private bool DraggingCamera = false;
    #endregion

    #region Score

    //private bool bSunk = false;
    int HitCount;

    #endregion

    #region ShootVars

    private Vector3 LaunchLocation;
    private float Stay;

    private float DefaultDrag;
    private float DefaultAngularDrag = 0.5f;
    private bool bStuck;
    #endregion

    #region References

    private IMiniGolf CameraManager;

    private IMiniGolf UiInterface;

    private IMiniGolf GMInterface;

    [SerializeField]
    private ParticleSystem HitEffect;

    #endregion


    /// <summary>
    /// Gets references. Sets up default values.
    /// </summary>
    private void Start()
    {

        GetCameraMan();
        GetUi();
        GetGameMode();

        GolfBall_Rb = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -20, 0);
        DefaultDrag = GolfBall_Rb.drag;
        GolfBall_Rb.maxAngularVelocity = 50000;
    }
   
    /// <summary>
    /// Calculations
    /// </summary>
    private void FixedUpdate()
    {

        SetMovementInfo();
        CalculateDrag();
        AimAtHorizon();
        PullLookAtActorToGolfBall();
        IsGrounded();
        PlayAudio();
        AdjustRollingPitch();

    }

    /// <summary>
    /// Input Registry
    /// </summary>
    private void Update()
    {

        // INPUT
        GrabPull();

        //DEBUGINPUT
        OnReset();

    }

    #region Get

    /// <summary>
    /// Gets IminiGolf GameMode reference
    /// </summary>
    private void GetGameMode() { if (GameObject.FindGameObjectWithTag("GameMode").TryGetComponent(out IMiniGolf GameMode)) { GMInterface = GameMode; } }


    /// <summary>
    /// Gets IminiGolf Ui reference
    /// </summary>
    private void GetUi() {  if (GameObject.FindGameObjectWithTag("IngameUi").TryGetComponent(out IMiniGolf _UiInterface)) { UiInterface = _UiInterface; } }


    /// <summary>
    /// Gets IminiGolf Cameraman reference
    /// </summary>
    private void GetCameraMan() {  if (GameObject.FindGameObjectWithTag("CameraManager").TryGetComponent(out IMiniGolf _CameraMan)) { CameraManager = _CameraMan; } }
    #endregion
    
    #region Helpers


    private void AimAtHorizon()
    {
        GolfBall_LookAt_Camera_Transform.LookAt(new Vector3(Camera_Transform.position.x, GolfBall_LookAt_Camera_Transform.position.y, Camera_Transform.position.z));
    }


    /// <summary>
    /// Drags LookAtPoint Along
    /// </summary>
    private void PullLookAtActorToGolfBall()
    {
        GolfBall_LookAt_Camera_Transform.position = Original_GolfBall_Transform.position;
    }




    public void CalcPower(float Power_Sensitivity)
    {
        Power_Effectivity = Power_Sensitivity;
        HitMagnitude = Power_Effectivity * Club_Max_Power;
    }





    #endregion

    #region Movement

    /// <summary>
    /// Gets RigidBody LinearVelocity
    /// </summary>
    /// <returns></returns>
    private float GetLinearVelocity()
    {
        return GolfBall_Rb.velocity.sqrMagnitude;
    }

    /// <summary>
    /// Gets RigidBody AngularVelocity
    /// </summary>
    /// <returns></returns>
    private float GetAngularVelocity()
    {

        return GolfBall_Rb.angularVelocity.sqrMagnitude;
    }

    private void SetMovementInfo()
    {

        if (GetLinearVelocity() <= GolfBall_StopSpeed)
        {
            if (!bStuck && bBallReady && bMoving && !bSunk) { UiInterface.OnBallReady(); Debug.LogWarning("BallReady"); }
            bMoving = false;
            //bool_GolfBall_isShot = false;
            LocalData.bMoving = false;
            LocalData.bool_GolfBall_isShot = false;
            //Power_Slider.interactable = true;
            //UiInterface.PlayUiAnim("BallGrabCome",2);
        }
        else
        {
            //Power_Slider.interactable = false;
            LocalData.bMoving = true;
            bMoving = true;
        }
    }

    private void CalculateDrag()
    {
        if (bStuck)
        {
            GolfBall_Rb.angularDrag = 10;
            GolfBall_Rb.drag = 10;
            return;
        }
        if (bMoving)
        {
            if (GetLinearVelocity() < GolfBall_Speed_Decrasing_Velocity)
            {
                GolfBall_Rb.angularDrag = Mathf.MoveTowards(GolfBall_Rb.angularDrag, Brake_Angular_Drag, Time.deltaTime);
            }
        }
        else
        {
            ResetDrag();
        }
    }

    private void ResetDrag()
    {
        GolfBall_Rb.angularDrag = DefaultAngularDrag;
        GolfBall_Rb.drag = DefaultDrag;
    }


    /// <summary>
    /// Check if grounded.
    /// TODO RETURN BOOL IF GROUND = VALID
    /// </summary>
    private void IsGrounded()
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


    #endregion

    #region Audio


    private void PlayAudio()
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

            //if (Shoot_Time_Power_Effectivity < 0.2f)
            //{
            //    AudioComp.PlaySound_Ball_Shot_Lowest();
            //}
            //else if (Shoot_Time_Power_Effectivity < 0.4f)
            //{
            //    AudioComp.PlaySound_Ball_Shot_Lower();
            //}
            //else if (Shoot_Time_Power_Effectivity < 0.6f)
            //{
            //    AudioComp.PlaySound_Ball_Shot_Medium();
            //}
            //else if (Shoot_Time_Power_Effectivity < 0.8f)
            //{
            //    AudioComp.PlaySound_Ball_Shot_Higher();
            //}
            //else
            //{
            //    AudioComp.PlaySound_Ball_Shot_Highest();
            //}


            AudioComp.PlaySound_Ball_Shot_Higher();
        }
        /////////////// Play GolfBall Shot Sound//////////////////

        /////////////// Play GolfBall Rolling Sound  //////////////////
        if (bool_Golf_Ball_hit && bMoving && !bool_GolfBall_Rolling_Sound_isPlayed)
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
        else if (!bool_Golf_Ball_hit && bool_GolfBall_Rolling_Sound_isPlayed || !bMoving && bool_GolfBall_Rolling_Sound_isPlayed)
        {
            bool_GolfBall_Rolling_Sound_isPlayed = false;
            Roll_Countdown = 20;
            AudioComp.Stop_Playing_Rolling_Sound();
        }
        ///////////////  Play GolfBall Rolling Sound //////////////////
    }


    /// <summary>
    /// Adjust roll pitch
    /// lerp between 0.6 and 0.3 , max 8k, min <10
    /// </summary>
    private void AdjustRollingPitch()
    {
        Golf_Ball_Rolling_Audio_Pitch = 0.25f + (GetAngularVelocity() * 0.0000375f);
        Golf_Ball_Rolling_Audio_Pitch = Mathf.Clamp(Golf_Ball_Rolling_Audio_Pitch, 0.25f, 0.7f);
        AudioComp.Set_Golf_Roll_Pitch(Golf_Ball_Rolling_Audio_Pitch);
    }


    #endregion

    #region Functions


    private void OnCollisionEnter(Collision collision)
    {
        if (GolfBall_Rb.velocity.sqrMagnitude > GolfBall_Bounce)
        {

            AudioComp.PlaySound_Ball_Bounce();
        }
    }

    /// <summary>
    /// Shoots the ball at aiming location with calculated hitPower
    /// </summary>
    public void Shoot()
    {

        if (TraceForGround().HasValue)
        {
            HitEffect.transform.position = transform.position;
            HitEffect.Play();
            HitCount++;

            LaunchLocation = transform.position;
            GMInterface.UpdateScore(HitCount);


            // OnScreen Calculation
            Vector2 XY = GrabPos - EndTouchLocation;     
            // OnScreenToWorld
            Vector3 XyToXZY = new Vector3(XY.x, 0, XY.y);
            // WorldtoLocal
            XyToXZY = Camera_Transform.rotation * XyToXZY;
            
            GolfBall_Rb.AddForce(XyToXZY.normalized * HitMagnitude, ForceMode.Force);

        }
        
        LocalData.bool_GolfBall_isShot = true;


        bool_GolfBall_Shoot_Sound_isPlayed = false;
        Shoot_Time_Power_Effectivity = Power_Effectivity;
        BallUnStuck();
        ResetDrag();
    }



    /// <summary>
    /// Checks if we are grabbing ball or screen.
    /// Calculates rotation or hitting power
    /// Hits ball on release
    /// </summary>
    private void GrabPull()
    {
        if (Input.GetAxisRaw("Fire1") > 0 )
        {
            // CALCULATE POW AND RETURN
            if (bBallGrabbed) // bBallGrabbed | InitTouchLocation.HasValue
            {
                EndTouchLocation = Input.mousePosition;
                HitMagnitude = Vector2.Distance(GrabPos, EndTouchLocation);


                DrawLaunchLine();

                return;
            }
            
            // DRAG CAMERA AND RETURN
            if (DraggingCamera)
            {

                GrabPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - GrabPos;

                CinemachineRig.m_XAxis.Value += GrabPos.x * Time.deltaTime * 90.0f;
                //CinemachineRig.m_YAxis.Value += GrabPos.y * Time.deltaTime * 0.5f;

                GrabPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return;

            }
            

            // IF ALL FAILS WE ARE DRAGGING CAMERA
            DraggingCamera = true;

        }
        else
        {
            // SHOOT IF WE WERE HITTING BALL
            if (bBallGrabbed)
            {
                CalcPower(HitMagnitude);
                Shoot();

                bBallGrabbed = false;
                LineRenderer_GO.SetActive(false);
            }
            // CLEAR DRAG OPERATION
            DraggingCamera = false;
            GrabPos = Input.mousePosition;// new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
        }
    }


    /// <summary>
    /// Trace for calculating RealWorldPointerPosition.
    /// returns null if points at nothing.
    /// </summary>
    /// <returns>GroundLocation</returns>
    private Vector3? TraceForGround() 
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
        if (Physics.Raycast(ScreentoWorldNear, ScreentoWorldFar - ScreentoWorldNear, out hitResult, float.PositiveInfinity))
        {
            return hitResult.point;
        }

        return null;
    }

    /// <summary>
    /// This will be triggered when Ball enters the hole. 
    /// Trigger is outside this class. 
    /// This function should also register this object to GameMode so GameMode can make endgame calculations. 
    /// </summary>
    [SerializeField]
    public void BallSunk ()
    {
        //TODO This trigger just resets the level for now. | turn this into a reporter for GameMode.

        if (gameObject.tag=="UnSunkBall")
        {
            gameObject.tag = "Untagged";
            bSunk = true;
            
            if (GameObject.FindGameObjectWithTag("GameMode").TryGetComponent(out IMiniGolf GameMode))
            {
                GameMode.OnBallSunk(HitCount);
            }
        }
        CameraManager.PlayCamAnim("FarCamera");
    }

    /// <summary>
    /// Draws Launch Line from Pointer to Ball
    /// </summary>
    private void DrawLaunchLine()
    {
        if (TraceForGround().HasValue)
        {
            if (!LineRenderer_GO.activeInHierarchy)
            {
                LineRenderer_GO.SetActive(true);
            }
    

            LineRenderer.SetPosition(0, transform.position-new Vector3(0,0.05f,0));
            LineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));



            return;
        }

        if (LineRenderer_GO.activeInHierarchy)
        {
            LineRenderer_GO.SetActive(false);
        }

    }
    
    /// <summary>
    /// Set actor location to initial launching position
    /// </summary>
    public void ReturnToSender() 
    {
        transform.position = LaunchLocation;
        GolfBall_Rb.velocity = new Vector3(0, 0, 0);
        GolfBall_Rb.angularVelocity = new Vector3(0, 0, 0);

        bStuck = false;
        bBallReady= true;
        bMoving = true;
    }

    #endregion

    #region DEBUG

    private void OnReset() 
    {
        if (Input.GetButtonDown("Reset"))
        {
            ReturnToSender();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GolfBall_LookAt_Camera_Transform.position, -GolfBall_LookAt_Camera_Transform.forward * 2);
        Gizmos.DrawWireSphere(GolfBall_LookAt_Camera_Transform.position, Sphere_Overlap_Radius);
    }

    #endregion

    #region MiniGolfInterface
    public void BallStay(bool bShouldStop, int StayLimit) 
    {
        
        if (bShouldStop)
        {
            if (bMoving)
            {
                return;
            }
        }

        Stay += Time.deltaTime;
        
        if (Stay >= StayLimit)
        {
            ReturnToSender();
            Stay = 0;
        }
    }
    public void BallStuck() { bStuck = true; }
    public void BallUnStuck() { bStuck = false; Stay = 0.0f; }

    public void BallBounce() { }


    public void BallDragged(Vector3 normal, float StreamStr) 
    {
        GolfBall_Rb.AddForce(- normal * StreamStr);
    }

    public void OnBallGrabbed() { bBallGrabbed = true; }
    public void OnBallReleased(float Magnitude) 
    {
        
        if (Magnitude > 0.1)
        {
            //HitMagnitude = Magnitude;
            CalcPower(HitMagnitude);
            Shoot();
        }

        bBallGrabbed = false; 
    }

    public void OnBallReady() { bBallReady = true; }

    #endregion

    #region DEPRECATED

    /// <summary>
    /// DEPRECATED returns world location of ball if we are grabbing ball.
    /// </summary>
    /// <returns>Ball World Location</returns>
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
        if (Physics.Raycast(ScreentoWorldNear, ScreentoWorldFar - ScreentoWorldNear, out hitResult, float.PositiveInfinity))
        {
            if (hitResult.transform == transform)
            {
                InitTouchLocation = hitResult.transform.position;
                return InitTouchLocation;
            }
        }

        return null;
    }

    #endregion

}

