using System.Collections;
using UnityEngine;


    public interface IMiniGolf 
    {

    #region GameEvents

        void BallStay(bool bShouldStop, int StayLimit) { }

        void BallBounce() { }

        void BallDragged(Vector3 Normal,float StreamStr) { }

        void BallStuck() { }

        void BallUnStuck() { }

        void BallSunk() { }

        void OnBallSunk() { }

        void OnBallSunk(int HitCount) { }
    
        void OnBallSunk(int HitCount, IMiniGolf Ball) { }


    #endregion


    #region UIFunks

        void UpdateScore(int HitCount) { }
        void UpdateText(string NewText) { }

    public bool GetExitReady() { return false; }
    public void OnGameEnded() { }
    #endregion


    #region CameraFunks

        public void PlayCamAnim(string AnimName) { }
        public void PlayUiAnim(string AnimName) { }

    #endregion


    }
    