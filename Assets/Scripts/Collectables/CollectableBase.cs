using System;
using GGJ.Player;
using UnityEngine;

namespace GGJ.Collectables
{
    [Serializable]
    public struct CollectableBehaviourData
    {
        public float pickupDistance;
        public float drag;
        public float accel;
    }
    public class CollectableBase : MonoBehaviour
    {
        private enum STATE
        {
            NONE,
            LAUNCHING,
            WAITING_FOR_PLAYER,
            MOVE_TO_PLAYER
        }
        //------------------------------------------------//

        private static PlayerHealth _playerHealth;
        private static Transform _playerTransform;

        [SerializeField, Min(0)]
        private int healthToAdd;

        private CollectableBehaviourData _behaviourData;

        private STATE _currentState;
        
        private Vector3 _velocity;
        private float _pickupCountdown;
        private float _moveSpeed;


        //Unity Functions
        //============================================================================================================//
        
        // Start is called before the first frame update
        private void Start()
        {
            if (_playerTransform == null)
            {
                _playerHealth = FindObjectOfType<PlayerHealth>();
                _playerTransform = _playerHealth.transform;
            }
        }

        //TODO Set this up as a state machine
        // Update is called once per frame
        private void Update()
        {
            //------------------------------------------------//
            
            void Slow()
            {
                _velocity -= _velocity * (Time.deltaTime * _behaviourData.drag);
                transform.position += _velocity * Time.deltaTime;
            }

            //------------------------------------------------//
            
            var dirToPlayer = _playerTransform.position - transform.position;

            switch (_currentState)
            {
                //------------------------------------------------//
                case STATE.NONE:
                    return;
                //------------------------------------------------//
                case STATE.LAUNCHING:
                    if (_pickupCountdown > 0f)
                    {
                        _pickupCountdown -= Time.deltaTime;
                    }
                    else
                        _currentState = STATE.WAITING_FOR_PLAYER;

                    //Drag
                    Slow();
                    break;
                //------------------------------------------------//
                case STATE.WAITING_FOR_PLAYER:
                    //Distance check
                    if (dirToPlayer.magnitude < _behaviourData.pickupDistance)
                        _currentState = STATE.MOVE_TO_PLAYER;

                    Slow();
                    break;
                //------------------------------------------------//
                case STATE.MOVE_TO_PLAYER:
                    if (dirToPlayer.magnitude < 0.5f)
                    {
                        //TODO Pickup
                        _playerHealth.AddHealth(healthToAdd);
                        Destroy(gameObject);
                        return;
                    }
                    
                    _moveSpeed += Time.deltaTime * _behaviourData.accel;
                    var currentPosition = transform.position;

                    currentPosition = Vector3.MoveTowards(
                        currentPosition, 
                        _playerTransform.position,
                        _moveSpeed * Time.deltaTime);
                
                    transform.position = currentPosition;
                    break;
                //------------------------------------------------//
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //============================================================================================================//

        public void Launch(CollectableBehaviourData behaviourData, Vector3 direction, float speed, float pickupDelay = 1f)
        {
            _behaviourData = behaviourData;
            _velocity = direction.normalized * speed;

            _pickupCountdown = pickupDelay;
            
            _currentState = STATE.LAUNCHING;
        }
        
        //============================================================================================================//
    }
}
