using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ellyality.Mapping
{
    [AddComponentMenu("Ellyality/Mapping/Projection")]
    public class ProjectionMapping : MonoBehaviour, IProjectionMapping
    {
        #region Field
        [SerializeField] SceneMapping sceneManager;
        [SerializeField] GameObject cameraPrefab;
        [SerializeField] RectTransform cameraFeeds;
        [SerializeField] public RenderTexture[] UseTextures = new RenderTexture[0];
        #endregion

        #region Public Property
        public List<RawImage> rawImages { set; get; } = new List<RawImage>();
        public List<Camera> cameras { set; get; } = new List<Camera>();
        #endregion

        [Button]
        public void UpdateCameraFeeds()
        {
            while(cameraFeeds.childCount > 0)
            {
                if (Application.isEditor) DestroyImmediate(cameraFeeds.GetChild(0).gameObject);
                else Destroy(cameraFeeds.GetChild(0).gameObject);
            }

            int c = sceneManager.rts.Count;
            for(int i = 0; i < c; i++)
            {
                GameObject g = new GameObject($"rt {i}");
                RawImage ri = g.AddComponent<RawImage>();
                AspectRatioFitter arf = g.AddComponent<AspectRatioFitter>();
                arf.aspectRatio = (float)sceneManager.res.x / (float)sceneManager.res.y;
                ri.texture = sceneManager.rts[i];
                g.transform.SetParent(cameraFeeds);

                g = GameObject.Instantiate(cameraPrefab, transform);
                Camera cam = g.transform.GetComponent<Camera>();
                cameras.Add(cam);
                rawImages.Add(ri);
            }

            cameraFeeds.ForceUpdateRectTransforms();
        }
    }
}
