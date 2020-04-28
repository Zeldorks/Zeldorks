using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace NetGameClient
{
    public class Sound
    {
        public enum Background
        {
            Dungeon
        }

        public enum Effect
        {
            SwordSlash,
            ArrowBoomerang,
            LinkHurt,
            Secret,
            Fanfare,
            Recorder
        }

        // Depedencies
        private ContentManager contentManager;

        public Sound(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;

            SoundEffect.MasterVolume = 0.5f;
        }

        public void Play(Background background)
        {
            Song song = contentManager.Load<Song>(
                "Sound/Background/" + background.ToString()
            );
            MediaPlayer.Play(song);
        }

        public void Play(Effect effect)
        {
            SoundEffect soundEffect = contentManager.Load<SoundEffect>(
                "Sound/Effects/" + effect.ToString()
            );
            soundEffect.Play();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
