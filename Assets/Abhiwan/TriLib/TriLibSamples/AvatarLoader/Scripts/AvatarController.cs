﻿using UnityEngine;
using StarterAssets;

namespace TriLibCore.Samples
{
    /// <summary>Represents a class used to control an avatar on TriLib samples.</summary>
    public class AvatarController : MonoBehaviour
    {
        /// <summary>The Avatar Controller Singleton instance.</summary>
        public static AvatarController Instance { get; private set; }

        /// <summary>
        /// Maximum avatar speed in units/second.
        /// </summary>
        private const float MaxSpeed = 2f;

        /// <summary>
        /// Avatar acceleration in units/second.
        /// </summary>
        private const float Acceleration = 5f;

        /// <summary>
        /// Avatar Friction in units/second.
        /// </summary>
        private const float Friction = 2f;

        /// <summary>
        /// Avatar smooth rotation factor.
        /// </summary>
        private const float RotationSpeed = 60f;

        /// <summary>
        /// Avatar character controller.
        /// </summary>
        public CharacterController CharacterController;

        /// <summary>
        /// Avatar animator.
        /// </summary>
        public Animator Animator;

        /// <summary>
        /// Game object that wraps the actual avatar.
        /// </summary>
        public GameObject InnerAvatar;

        /// <summary>
        /// Camera offset relative to the avatar.
        /// </summary>
        private Vector3 _cameraOffset;

        /// <summary>
        /// Current avatar speed.
        /// </summary>
        private float _speed;

        /// <summary>
        /// Camera height offset relative to the avatar.
        /// </summary>
        private Vector3 _cameraHeightOffset;

        /// <summary>
        /// Current smooth rotation velocity.
        /// </summary>
        private float _currentVelocity;

        private ThirdPersonController thirdPersonController;
        public bool IsLvlLoading = false;
        private Vector3 velocity = Vector3.zero;

        private float smoothTime = 0.03f;
        /// <summary>Configures this instance and calculates the Camera offsets.</summary>
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _cameraHeightOffset = new Vector3(0f, CharacterController.height * 0.9f, 0f);
            _cameraOffset = Camera.main.transform.position - transform.position;
        }

        private void Start()
        {
            thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
        }

        /// <summary>Handles input (controls the Camera and moves the Avatar character).</summary>
        //private void Update()
          private void LateUpdate()
        {
            // var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            // var direction = Camera.main.transform.TransformDirection(input);
            // direction.y = 0f;
            // direction.Normalize();
            // var targetEulerAngles = direction.magnitude > 0 ? Quaternion.LookRotation(direction).eulerAngles : transform.rotation.eulerAngles;
            // var eulerAngles = transform.rotation.eulerAngles;
            // eulerAngles.y = Mathf.SmoothDampAngle(eulerAngles.y, targetEulerAngles.y, ref _currentVelocity, Time.deltaTime * RotationSpeed * input.magnitude);
            // transform.rotation = Quaternion.Euler(eulerAngles);
            // _speed += input.magnitude * (Acceleration * MaxSpeed) * Time.deltaTime;
            // _speed -= Friction * MaxSpeed * Time.deltaTime;
            // _speed = Mathf.Clamp(_speed, 0f, MaxSpeed);
            // CharacterController.SimpleMove(transform.forward * _speed);
            // Animator.SetFloat("SpeedFactor", _speed / MaxSpeed);

            if (IsLvlLoading)
                return;
            var pivotedPosition = Quaternion.AngleAxis(AssetViewerBase.Instance.CameraAngle.x, Vector3.up) * Quaternion.AngleAxis(-AssetViewerBase.Instance.CameraAngle.y, Vector3.right) * _cameraOffset;
         // Camera.main.transform.position = transform.position + _cameraHeightOffset + pivotedPosition;
         Vector3 targetPos = transform.position + _cameraHeightOffset + pivotedPosition;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPos, ref velocity,smoothTime);
            Camera.main.transform.LookAt(transform.position + _cameraHeightOffset);

            if (thirdPersonController.animationRun)
            {
                this.Animator.SetFloat("SpeedFactor", 0.3f);
            }
            else
            {
                this.Animator.SetFloat("SpeedFactor", 0.0f);
            }
        }
    }
}