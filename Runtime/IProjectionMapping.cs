using System.Collections.Generic;
using UnityEngine.UI;

namespace Ellyality.Mapping
{
    interface IProjectionMapping
    {
        void UpdateCameraFeeds();
        List<RawImage> rawImages { set; get; }
    }
}
