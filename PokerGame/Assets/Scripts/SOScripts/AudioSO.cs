using UnityEngine;

namespace SOScripts
{
    [CreateAssetMenu(fileName = "AudioSo", menuName = "SO/AudioSo", order = 5)]
    public class AudioSO : ScriptableObject
    {
        private float _soundSlider;

        public float SoundSlider
        {
            get => _soundSlider;
            set => _soundSlider = value;
        }
    }
}