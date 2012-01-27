using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class CAudio
    {
        Song backgroundMusic;
        SoundEffect effect;

        public void init(ContentManager content, String filename)
        {
            backgroundMusic = content.Load<Song>(filename);
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        public void playEffect(String soundname, ContentManager content, bool sound)
        {
            if (sound == true)
            {
                effect = content.Load<SoundEffect>(soundname);
                effect.Play();
            }
        }

        public void backgroundMusicToggle()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Resume();
            }
        }
    }
}
