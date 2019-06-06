﻿using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class DetailViewModel : ViewModelBase
    {
        private BitmapImage _thumbnail;
        private string _title;
        private string _artist;
        private ObservableCollection<LocalMusicModel> _musics;

        public DetailViewModel(BitmapImage image, string title, string artist, ObservableCollection<LocalMusicModel> musics)
        {
            Thumbnail = image;
            Title = title;
            Artist = artist;
            Musics = musics;
        }

        public BitmapImage Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if(_thumbnail != value)
                {
                    _thumbnail = value;
                    RaisePropertyChanged(() => Thumbnail);
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                if(_artist != value)
                {
                    _artist = value;
                    RaisePropertyChanged(() => Artist);
                }
            }
        }

        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                if(_musics != value)
                {
                    _musics = value;
                    RaisePropertyChanged(() => Musics);
                }
            }
        }
    }
}
