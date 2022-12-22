using UnityEngine;
using Zenject;

namespace Views
{
    public class PauseView : MonoBehaviour
    {
        private PlayerInput PlayerInput { get; set; }

        [SerializeField] private GameObject pausePanel;

        [Inject]
        private void Construct(PlayerInput playerInput)
        {
            PlayerInput = playerInput;
        }

        private void Awake()
        {
            PlayerInput.OnPauseToggle += TogglePause;
        }

        public void TogglePause(bool value) => pausePanel.SetActive(value);
    }
}
