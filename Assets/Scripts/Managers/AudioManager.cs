using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        private const string soundKey = "SoundOn";
        private const string musicKey = "MusicOn";

        private GameController GameController { get; set; }
        private CollectibleManager CollectibleManager { get; set; }
        private ScoreManager ScoreManager { get; set; }

        [SerializeField] private AudioSource source;

        [SerializeField] private AudioClip ambient;

        [SerializeField] List<Sound> clips = new List<Sound>();

        public bool IsSoundOn { get; set; }
        public bool IsMusicOn { get; set; }

        [Inject]
        private void Construct(
            GameController gameController,
            CollectibleManager collectibleManager,
            ScoreManager scoreManager
        )
        {
            GameController = gameController;
            CollectibleManager = collectibleManager;
            ScoreManager = scoreManager;
        }

        private void Awake()
        {
            IsMusicOn = PlayerPrefs.GetInt(musicKey, 1) == 1;
            IsSoundOn = PlayerPrefs.GetInt(soundKey, 1) == 1;
            if (IsMusicOn)
                Play(ambient);

            GameController.OnLose += PlayLose;
            GameController.OnPause += PlayClick;
            GameController.OnRetry += PlayClick;
            CollectibleManager.OnUpdateCollectiblesScore += PlayCollect;
            ScoreManager.OnScoreChanged += PlayScoreChange;
        }

        private void PlayClick() => Play(SoundEnum.Click);

        private void PlayCollect() => Play(SoundEnum.Collect);

        private void PlayColorChange() => Play(SoundEnum.ColorChange);

        private void PlayScoreChange(int value) => Play(SoundEnum.ScoreChange);

        private void PlayLose() => Play(SoundEnum.Lose);

        private void Play(AudioClip clip) => source.PlayOneShot(clip);

        private void Play(SoundEnum sound)
        {
            if (IsSoundOn)
                Play(clips.FirstOrDefault(x => x.SoundValue == sound).Clip);
        }

        public void ToggleSound(bool value)
        {
            IsSoundOn = value;
            PlayerPrefs.SetInt(soundKey, IsSoundOn ? 1 : 0);
        }

        public void ToggleMusic(bool value)
        {
            IsMusicOn = value;
            PlayerPrefs.SetInt(musicKey, IsMusicOn ? 1 : 0);
            if (value)
                Play(ambient);
            else
                source.Stop();
        }

        private void OnDestroy()
        {
            GameController.OnLose -= PlayLose;
            GameController.OnPause -= PlayClick;
            GameController.OnRetry -= PlayClick;
            CollectibleManager.OnUpdateCollectiblesScore -= PlayCollect;
            ScoreManager.OnScoreChanged -= PlayScoreChange;
        }
    }

    public enum SoundEnum
    {
        Click = 0,
        Collect = 1,
        Lose = 2,
        ColorChange = 3,
        ScoreChange = 4,
    }

    [Serializable]
    public class Sound
    {
        [SerializeField]
        private SoundEnum sound;

        [SerializeField]
        private AudioClip clip;

        public SoundEnum SoundValue => sound;
        public AudioClip Clip => clip;
    }
}
