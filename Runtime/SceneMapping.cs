using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ellyality.Mapping
{

    public class SceneMapping : MonoBehaviour, ISceneMapping
    {
        #region Field
        [SerializeField] MappingType Type = MappingType.Ring;
        [SerializeField] int CameraCount = 1;
        [SerializeField] int DisplayStartAt = 1;
        [SerializeField] float FOV = 90;
        [SerializeField] Vector3Int Resolution = new Vector3Int(1920, 1080, 24);
        #endregion

        #region Public Variable
        public List<Camera> cameras { set; get; } = new List<Camera>();
        public List<RenderTexture> rts { set; get; } = new List<RenderTexture>();
        public Vector2Int res => new Vector2Int(Resolution.x, Resolution.y);
        #endregion

        #region Private Property
        int c => cameras.Count;
        float ratio => (float)res.x / (float)res.y;
        float radFOV => FOV * Mathf.Deg2Rad;
        double radHFOV => 2.0f * Math.Atan(Mathf.Tan(radFOV / 2) * ratio);
        float rot => (float)(radHFOV * Mathf.Rad2Deg);
        float start => ((CameraCount - 1) * rot) / -2;
        #endregion

        [Button]
        public void DebugInfo()
        {
            Debug.Log($"Camera Count: {c}");
            Debug.Log($"Display Res: {res}");
            Debug.Log($"Ratio: {ratio}");
            Debug.Log($"Rotation: {rot}");
            Debug.Log($"Start From: {start}");
        }

        [Button]
        public void DestroyChild()
        {
            while(transform.childCount > 0)
            {
                if (Application.isEditor) DestroyImmediate(transform.GetChild(0).gameObject);
                else Destroy(transform.GetChild(0).gameObject);
            }
            foreach(var i in rts)
            {
                i.Release();
            }
            cameras.Clear();
            rts.Clear();
        }

        [Button]
        public void UpdateSetup()
        {
            for (int i = 0; i < c; i++)
            {
                if (Application.isEditor) DestroyImmediate(cameras[i].gameObject);
                else Destroy(cameras[i].gameObject);
            }
            cameras.Clear();

            if (CameraCount <= 1) CameraCount = 1;
            for (int i = 0; i < CameraCount; i++)
            {
                GameObject camera_object = new GameObject($"camera {i}");
                Camera camera_buffer = camera_object.AddComponent<Camera>();
                RenderTexture rt = new RenderTexture(Resolution.x, Resolution.y, Resolution.z);
                rt.name = $"rt {i}";
                camera_buffer.fieldOfView = FOV;
                camera_buffer.targetDisplay = DisplayStartAt + i;
                camera_buffer.targetTexture = rt;
                cameras.Add(camera_buffer);
                rts.Add(rt);
                camera_object.transform.SetParent(transform, false);
            }
            PositionReset();
        }

        [Button]
        public void PositionReset()
        {
            for (int i = 0; i < CameraCount; i++)
            {
                GameObject camera_object = cameras[i].gameObject;
                camera_object.transform.localPosition = Vector3.zero;
                camera_object.transform.localEulerAngles = new Vector3(0, start + rot * i, 0);
            }
                
        }
    }
}
