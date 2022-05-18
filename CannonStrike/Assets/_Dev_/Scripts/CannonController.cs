using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CannonStrike
{
    public class CannonController : MonoBehaviour
    {

        [SerializeField] private GameObject _cannonStation;
        [SerializeField] private GameObject _cannonBarrel;

        public CannonStrike _inputAction;

        private Vector2 _cannonRotate;

        [SerializeField] private float _cannonMaxRotate;
        [SerializeField] private float _cannonMinRotate;
        [SerializeField] private float _cannonCurrentRotate;

        [SerializeField] private float _maxChargeTime = 4f;
        [SerializeField] private float _chargeTimer = 0;

        [SerializeField] private GameObject _shellPrefab;
        [SerializeField] private Transform _shellSpawnPoint;

        [SerializeField] private float _shootPower;

        enum CannonState
        {
            Disable,
            Waiting,
            Charging
        }

        private CannonState _state = CannonState.Waiting;
        
        void Start()
        {
            _inputAction = new CannonStrike();

            _inputAction.Player.Move.started += OnMove;
            _inputAction.Player.Move.performed += OnMove;
            _inputAction.Player.Move.canceled += OnMove;
            _inputAction.Player.Fire.performed += OnFire;
            _inputAction.Player.Charge.performed += OnCharging;
            
        }

        private void FixedUpdate()
        {
            
            _cannonStation.transform.Rotate(0,_cannonRotate.x,0,Space.World);

            _cannonCurrentRotate = _cannonBarrel.transform.rotation.eulerAngles.x;

            if (_cannonCurrentRotate > _cannonMaxRotate && _cannonRotate.y == 1 ||
                _cannonCurrentRotate < _cannonMinRotate && _cannonRotate.y == -1 ||
                _cannonCurrentRotate >= _cannonMinRotate && _cannonCurrentRotate <= _cannonMaxRotate)
            {
                _cannonBarrel.transform.Rotate(-_cannonRotate.y,0,0,Space.Self);
            }

            if (_state == CannonState.Charging)
            {
                _chargeTimer += Time.deltaTime;
            }
            
        }

        void OnMove(InputAction.CallbackContext context)
        {
            _cannonRotate = context.ReadValue<Vector2>();
        }

        void OnCharging(InputAction.CallbackContext context)
        {
            if (_state == CannonState.Waiting)
            {
                _chargeTimer = 0;
                _state = CannonState.Charging;
            }
        }

        void OnFire(InputAction.CallbackContext context)
        {
            if (_state == CannonState.Charging)
            {
                if (_chargeTimer >= _maxChargeTime)
                {
                    _chargeTimer = _maxChargeTime;
                }

                var shell = Instantiate(_shellPrefab, _shellSpawnPoint.position, _shellSpawnPoint.rotation);
                shell.GetComponent<Rigidbody>().AddForce(_shellSpawnPoint.forward * _shootPower * _chargeTimer);

                _state = CannonState.Waiting;
            }
            
        }
    }
}

