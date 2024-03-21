using System.Collections.Generic;
using UnityEngine;

namespace Ellyality.Mapping
{
    public interface ISceneMapping
    {
        List<Camera> cameras { set; get; }
        List<RenderTexture> rts { set; get; }
        Vector2Int res { get; }
        void DebugInfo();
        void DestroyChild();
        void UpdateSetup();
        void PositionReset();
    }
}
