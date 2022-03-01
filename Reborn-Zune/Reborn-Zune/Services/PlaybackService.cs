using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace Reborn_Zune.Services
{
    class PlaybackService
    {
        static PlaybackService instance;

        public static PlaybackService Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlaybackService();
                return instance;
            }
        }

        public MediaPlayer Player { get; private set; }


        public PlaybackService()
        {
            Player = new MediaPlayer();
            Player.AutoPlay = true ;
        }
    }
}
