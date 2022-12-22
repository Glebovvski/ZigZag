using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class SettingsView : MonoBehaviour
    {
        private PlayerInput PlayerInput { get; set; }
        private GameController GameController { get; set; }
        private AudioManager AudioManager { get; set; }

        [SerializeField] private GameObject settingsPanel;

        [SerializeField] private Toggle soundToggle;

        [SerializeField] private Toggle musicToggle;

        [SerializeField] private Slider speedSlider;

        [SerializeField] private Text speedText;

        [SerializeField] private Toggle aiToggle;

        [Inject]
        private void Construct(
            PlayerInput playerInput,
            GameController gameController,
            AudioManager audioManager
        )
        {
            PlayerInput = playerInput;
            GameController = gameController;
            AudioManager = audioManager;
        }

        private void Start()
        {
            GameController.OnOpenSettings += Open;
            GameController.OnCloseSettings += Close;

            aiToggle.onValueChanged.AddListener(AIToggle);

            speedText.text = PlayerInput.Speed.ToString();
            speedSlider.value = PlayerInput.Speed;
            speedSlider.onValueChanged.AddListener(SpeedChange);

            soundToggle.isOn = AudioManager.IsSoundOn;
            musicToggle.isOn = AudioManager.IsMusicOn;
            soundToggle.onValueChanged.AddListener(ToggleSound);
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }

        private void ToggleMusic(bool value) => AudioManager.ToggleMusic(value);

        private void ToggleSound(bool value) => AudioManager.ToggleSound(value);

        private void AIToggle(bool value)
        {
            PlayerInput.ToggleAI(value);
        }

        private void SpeedChange(float value)
        {
            speedText.text = value.ToString();
            PlayerInput.UpdateSpeed(value);
        }

        public void Open()
        {
            Time.timeScale = 0;
            settingsPanel.SetActive(true);
        }

        public void Close()
        {
            Time.timeScale = 1;
            settingsPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            GameController.OnOpenSettings -= Open;
            GameController.OnCloseSettings -= Close;
        }
    }
}
