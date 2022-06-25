using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_AudioComp : MonoBehaviour
{
    [SerializeField]
    private AudioSource Speaker1, Speaker_2;
    [SerializeField]
    private AudioClip AUD_Shot_Lowest, AUD_Shot_Lower, AUD_Shot_Medium, AUD_Shot_Higher, AUD_Shot_Highest, AUD_Bounce, AUD_Rolling;


    public void PlaySound_Ball_Shot_Lowest()
    {
        Speaker1.PlayOneShot(AUD_Shot_Lowest);
    }
    public void PlaySound_Ball_Shot_Lower()
    {
        Speaker1.PlayOneShot(AUD_Shot_Lower);
    }
    public void PlaySound_Ball_Shot_Medium()
    {
        Speaker1.PlayOneShot(AUD_Shot_Medium);
    }
    public void PlaySound_Ball_Shot_Higher()
    {
        Speaker1.PlayOneShot(AUD_Shot_Higher);
    }
    public void PlaySound_Ball_Shot_Highest()
    {
        Speaker1.PlayOneShot(AUD_Shot_Highest);
    }

    public void PlaySound_Ball_Bounce()
    {
        Speaker1.PlayOneShot(AUD_Bounce);
    }

    public void PlaySound_Ball_Rolling()
    {
        Speaker_2.Play();
    }

    public void Stop_Playing_Rolling_Sound()
    {
        Speaker_2.Stop();
    }

    public void Set_Golf_Roll_Pitch(float pitch)
    {
        Speaker_2.pitch = pitch;
    }
}
