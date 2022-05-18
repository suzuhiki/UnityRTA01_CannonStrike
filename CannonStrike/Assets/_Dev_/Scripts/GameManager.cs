using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonStrike
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (GameManager) FindObjectOfType(typeof(GameManager));
                    if (_instance == null)
                    {
                        Debug.Log("Error : instance not found");
                    }
                }

                return _instance;
            }
        }
        
        public enum GameState
        {
            Title,
            Playing,
            Result
        }

        public GameState _state = GameState.Title;

        [SerializeField] private GameObject[] _targets;
        private int _targetCount;

        [SerializeField] private TextMeshProUGUI _targetCountText;

        [SerializeField] private Color _defaultTargetColor;

        //0:title 1:play 2:result
        [SerializeField] private GameObject[] _panels;

        [SerializeField] private TextMeshProUGUI _timeDisplay;
        [SerializeField] private TextMeshProUGUI _timeResultDisplay;

        private float _timer;
        private int _min;
        private int _sec;

        [SerializeField] private CannonController _cannonController;

        private void Awake()
        {
            CheckInstance();
        }

        private void Start()
        {
            RefreshStage();
        }

        private void Update()
        {
            if (_state == GameState.Playing)
            {
                _timer += Time.deltaTime;
                CulTime();
                _timeDisplay.text = _min.ToString("00") + ":" + _sec.ToString("00");
            }
        }

        void CheckInstance()
        {
            if (_instance == null)
            {
                _instance = this;
                return;
            }
            else if (_instance == this)
            {
                return;
            }
            
            Destroy(this);
        }

        public void OnDestroyTarget()
        {
            _targetCount--;
            _targetCountText.text = _targetCount.ToString();

            if (_targetCount <= 0)
            {
                _cannonController._inputAction.Disable();
                _timeResultDisplay.text = _min.ToString("00") + ":" + _sec.ToString("00");
                ActivatePanel(2);
                CulTime();
            }
        }

        void RefreshStage()
        {
            _targetCount = _targets.Length;
            _targetCountText.text = _targetCount.ToString();

            foreach (var obj in _targets)
            {
                obj.GetComponent<MeshRenderer>().material.color = _defaultTargetColor;
                obj.tag = "Target";
            }

            _timer = 0;
        }

        public void OnClickStartButton()
        {
            RefreshStage();
            ActivatePanel(1);
            _cannonController._inputAction.Enable();
        }

        private void ActivatePanel(int index)
        {
            foreach (var obj in _panels)
            {
                obj.gameObject.SetActive(false);
            }
            _panels[index].gameObject.SetActive(true);
        }

        void CulTime()
        {
            _min = (int) _timer / 60;
            _sec = (int) _timer % 60;
        }
    }
}

