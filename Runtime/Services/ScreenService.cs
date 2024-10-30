using SensenToolkit;
using UnityEngine;

namespace Bumashuta
{
    public class ScreenService : APermanentSingleton<ScreenService>
    {
        [SerializeField] private FullScreenMode _mode;
        [SerializeField] private int _width = 640;
        [SerializeField] private int _height = 480;

        protected override void AwakeSingleton()
        {
            base.AwakeSingleton();
            Screen.SetResolution(_width, _height, _mode);
        }
    }
}
