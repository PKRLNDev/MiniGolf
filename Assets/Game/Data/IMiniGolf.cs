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

        void OnBallSunk() { }
        void OnBallSunk(int HitCount) { }
        void OnBallSunk(int HitCount, IMiniGolf Ball) { }

        void UpdateScore(int HitCount) { }



    #region CameraFunks


    public void UpdateText(string NewText) { }

    public void PlayCamAnim(string AnimName) { }
    public void PlayUiAnim(string AnimName) { }


    #endregion


}
    