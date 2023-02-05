using System.Runtime.Serialization;
using GGJ.Levels;
using GGJ.Objectives;
using UnityEngine;
using UnityEngine.UI;
using GGJ.Player;
using GGJ.Utilities.FolderGeneration;
using TMPro;
using UnityEngine.SceneManagement;

namespace GGJ.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField, Header("HealthWindow")]
        private Image _playerHealthbar;
        [SerializeField]
        private TMP_Text _playerHealthText;
        
        [SerializeField, Header("Main UI")]
        private TMP_Text directoryText;
        [SerializeField]
        private TMP_Text objectiveText;

        [SerializeField, Header("Lost Window")]
        private GameObject lostWindow;
        [SerializeField]
        private Button restartButton;

        [SerializeField, Header("Pause Window")]
        private GameObject pauseWindow;
        [SerializeField]
        private Button resumeButton;


        
        //Unity Functions
        //============================================================================================================//
        private void OnEnable()
        {
            PlayerHealth.OnPlayerHealthChanged += OnPlayerHealthChanged;
            RoomManager.OnRoomLoaded += OnRoomLoaded;
            RoomManager.OnNewRoomLoaded += OnRoomLoaded;
            ObjectiveController.OnNewObjective += OnNewObjective;
            PlayerHealth.OnPlayerDied += OnPlayerDied;
        }


        // Start is called before the first frame update
        private void Start()
        {
            InitUi();
        }

        private void OnDisable()
        {
            PlayerHealth.OnPlayerHealthChanged -= OnPlayerHealthChanged;
            RoomManager.OnRoomLoaded -= OnRoomLoaded;
            RoomManager.OnNewRoomLoaded += OnRoomLoaded;
            ObjectiveController.OnNewObjective -= OnNewObjective;
            PlayerHealth.OnPlayerDied -= OnPlayerDied;
        }

        //UI Init Function
        //============================================================================================================//

        private void InitUi()
        {
            lostWindow.SetActive(false);
            pauseWindow.SetActive(false);
            
            _playerHealthbar.fillAmount = 0f;
            
            resumeButton.onClick.AddListener(OnResumePressed);
            restartButton.onClick.AddListener(OnRestartPressed);

            directoryText.text = "\\ROOT";
            _playerHealthText.text = "0 %";
        }

        //Button Functions
        //============================================================================================================//
        
        private void OnResumePressed()
        {
            Time.timeScale = 1f;
            pauseWindow.SetActive(false);
        }

        private void OnRestartPressed()
        {
            SceneManager.LoadScene(1);
        }
        
        //Callbacks
        //============================================================================================================//
        
        private void OnPlayerHealthChanged(float playerHealth)
        {
            _playerHealthbar.fillAmount = 1f - playerHealth;

            _playerHealthText.text = $"{1f - playerHealth:P0}";
        }

        private void OnRoomLoaded(int _) => OnRoomLoaded();
        private void OnRoomLoaded()
        {
            directoryText.text = RoomManager.CurrentRoom.FolderRoomData.GetAbsolutePath();
        }
        
        private void OnNewObjective((OBJECTIVE_TYPE objective, File targetFile, FolderRoom targetRoom) obj)
        {
            if (obj.objective == OBJECTIVE_TYPE.MOVE)
            {
                objectiveText.text =
                    $"Objective is \n{obj.objective.ToString()}</b>" +
                    $"\n<b>{obj.targetFile.GetFileNameExtension()}</b>" +
                    $"\nto\n<b>{obj.targetRoom.GetAbsolutePath()}</b>";
            }
            else
                objectiveText.text = $"Objective is" +
                                     $"\n<b>{obj.objective.ToString()}</b>" +
                                     $"\n<b>{obj.targetFile.GetFileNameExtension()}</b>";
        }
        
        private void OnPlayerDied()
        {
            lostWindow.SetActive(true);
        }

        //============================================================================================================//
    }
}
