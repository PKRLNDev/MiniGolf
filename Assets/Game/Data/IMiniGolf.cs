using System.Collections;
using UnityEngine;


    public interface IMiniGolf 
    {

    #region GameEvents

        void LevelStart() { }

        void BallStay(bool bShouldStop, int StayLimit) { }

        void BallBounce() { }

        void BallDragged(Vector3 Normal,float StreamStr) { }

        void BallStuck() { }

        void BallUnStuck() { }

        void BallSunk() { }

        void OnBallSunk() { }

        void OnBallSunk(int HitCount) { }
    
        void OnBallSunk(int HitCount, IMiniGolf Ball) { }

        public void OnBallGrabbed() { }
        public void OnBallReleased() { }

        public void OnBallReady() { }
    #endregion


    #region UIFunks

        void UpdateScore(int HitCount) { }
        void UpdateText(string NewText) { }

        public bool GetExitReady() { return false; }
        public void OnGameEnded(int HitCount) { }
        #endregion
        public void AdjustBallUiPos(Vector2 OnScreenPos) { }

    #region CameraFunks

        public void PlayCamAnim(string AnimName) { }
        public void PlayCamAnim(string AnimName, int Layer) { }
        public void PlayUiAnim(string AnimName) { }
        public void PlayUiAnim(string AnimName, int Layer) { }

    #endregion


    }
    