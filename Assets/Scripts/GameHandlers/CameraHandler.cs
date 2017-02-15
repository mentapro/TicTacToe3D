using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class CameraHandler : IInitializable, ILateTickable, IDisposable
    {
        private bool _tick = false;
        private Camera Camera { get; set; }
        private float Distance { get; set; }
        private float RotationYAxis { get; set; }
        private float RotationXAxis { get; set; }
        private float VelocityX { get; set; }
        private float VelocityY { get; set; }
        private Vector3 LookAtPosition { get; set; }
        
        private Settings _Settings { get; set; }
        private GameInfo Info { get; set; }

        public CameraHandler(Settings settings, GameInfo info)
        {
            _Settings = settings;
            Info = info;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Initialize()
        {
            Camera = Camera.main;
            Distance = _Settings.StartDistance;

            var angles = Camera.transform.eulerAngles;
            RotationYAxis = angles.y;
            RotationXAxis = angles.x;

            LookAtPosition = new Vector3(_Settings.Target.position.x, _Settings.Target.position.y + _Settings.YOffset, _Settings.Target.position.z);
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        public void LateTick()
        {
            if (_tick == false) return;
            if (!_Settings.Target) return;

            if (Input.GetMouseButton(1))
            {
                VelocityX += _Settings.XSpeed * Input.GetAxis("Mouse X") * Distance * 0.02f;
                VelocityY += _Settings.YSpeed * Input.GetAxis("Mouse Y") * 0.02f;
            }
            RotationYAxis += VelocityX;
            RotationXAxis -= VelocityY;
            RotationXAxis = ClampAngle(RotationXAxis, _Settings.YMinLimit, _Settings.YMaxLimit);
            var rotation = Quaternion.Euler(RotationXAxis, RotationYAxis, 0);

            Distance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * 5, _Settings.MinDistance, _Settings.MaxDistance);
            var negDistance = new Vector3(0.0f, 0.0f, -Distance);
            var position = rotation * negDistance + LookAtPosition;

            Camera.transform.rotation = rotation;
            Camera.transform.position = position;

            VelocityX = Mathf.Lerp(VelocityX, 0, Time.deltaTime * 2 * _Settings.SmoothTime);
            VelocityY = Mathf.Lerp(VelocityY, 0, Time.deltaTime * 2 * _Settings.SmoothTime);
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                OnGameStateChanged(Info.GameState);
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            _tick = state == GameStates.Started || state == GameStates.GameEnded;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        [Serializable]
        public class Settings
        {
            public Transform Target;
            public float XSpeed;
            public float YSpeed;
            public float YMinLimit;
            public float YMaxLimit;
            public float StartDistance;
            public float MinDistance;
            public float MaxDistance;
            public float SmoothTime;
            public float YOffset;
        }
    }
}