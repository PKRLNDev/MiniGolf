using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryScript : MonoBehaviour, IMiniGolf
{
    public GameObject TrajectoryLine;
    public float TrajectoryScale;
    public float TrajectoryRotation;
    public float ScaleNormal = 180;
    //public Color32 Color = new Color32(55,200,255,255);

    Material TrajectoryMaterial;
    float InitScaleZ;
    float InitScaleX;

    private void Awake()
    {
        TrajectoryLine.SetActive(false);
        TrajectoryMaterial = TrajectoryLine.GetComponent<MeshRenderer>().material;
        InitScaleZ = gameObject.transform.localScale.z;
        InitScaleX = gameObject.transform.localScale.x;
    }



    public void ActivateTrajectory(bool bActive) { TrajectoryLine.SetActive(bActive); }

    public void SetTrajectoryRotation(Quaternion NewRotation) { transform.rotation = NewRotation; }
    public void HitMagnitudeToScale(float HitMagnitude) 
    { 
        TrajectoryScale = (HitMagnitude / ScaleNormal) * InitScaleZ; 
        TrajectoryMaterial.SetFloat("_YTile", TrajectoryScale*2); 
        gameObject.transform.localScale = new Vector3(InitScaleX, 1, TrajectoryScale); 
    }
    public void HitMagnitudeToColor(Color32 NewColor) { TrajectoryMaterial.SetColor("_Color", NewColor); }
}
