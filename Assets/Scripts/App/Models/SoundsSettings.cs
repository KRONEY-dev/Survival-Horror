using System.Collections.Generic;
using static SoundManager;

namespace Models
{
    public class SoundsSettings
    {
        public Dictionary<SoundType, SoundSetting> SettingsData;

        public SoundsSettings()
        {
            SettingsData = new Dictionary<SoundType, SoundSetting>();
        }

        public class SoundSetting
        {
            public bool IsMuted;

            public SoundSetting()
            {
                IsMuted = false;
            }
        }
    }
}