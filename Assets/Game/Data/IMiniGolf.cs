using System.Collections;
using UnityEngine;


    public interface IMiniGolf 
    {
        void BallStay(bool bShouldStop, int StayLimit) { }

        void BallBounce() { }

        void BallDragged(Vector3 Normal,float StreamStr) { }

        void BallStuck() { }

        void BallUnStuck() { }

        void BallSunk() { }

        void OnBallSunk(int HitCount) { }

        void UpdateScore(int HitCount) { }
    }
    