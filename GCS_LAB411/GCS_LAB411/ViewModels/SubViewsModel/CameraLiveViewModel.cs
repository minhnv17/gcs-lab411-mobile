using LibVLCSharp.Shared;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class CameraLiveViewModel : BaseViewModel
    {
        readonly LibVLC _libvlc;

        private MediaPlayer _livestream;
        public MediaPlayer Livestream
        {
            get => _livestream;
            set => SetProperty(ref _livestream, value);
        }

        private bool _isShow;
        public bool IsShow
        {
            get => _isShow;
            set
            {
                SetProperty(ref _isShow, value);
            }
        }

        private bool _isSelectFlyTab = true;
        public bool IsSelectFlyTab
        {
            get => _isSelectFlyTab;
            set
            {
                SetProperty(ref _isSelectFlyTab, value);
                IsShow = _isEnable && value;
            }
        }

        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                SetProperty(ref _isEnable, value);
                IsShow = _isSelectFlyTab && value;
            }
        }

        public CameraLiveViewModel()
        {
            _isShow = _isEnable && _isSelectFlyTab;
            Core.Initialize();

            // instanciate the main libvlc object
            _libvlc = new LibVLC();
            Livestream = new MediaPlayer(_libvlc);
        }

        public void ConnectCamera(string url)
        {
            if(Livestream == null)
            {
                Livestream = new MediaPlayer(_libvlc);
            }
            using (var media = new Media(_libvlc, new Uri(url)))
                Livestream.Play(media);
        }

        public void DisConnectCamera()
        {
            Livestream.Pause();
        }
    }
}
