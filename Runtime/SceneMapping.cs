using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Ellyality.Mapping
{
    public class SceneMapping : MonoBehaviour
    {
        [SerializeField] private MappingType Type = MappingType.Ring;
        [SerializeField] private int CameraCount = 1;
        [SerializeField] private int DisplayStartAt = 1;
        [SerializeField] private float FOV = 90;

        List<Camera> cameras = new List<Camera>();

        int c => cameras.Count;
        Vector2Int res => new Vector2Int(Display.displays[DisplayStartAt].renderingWidth, Display.displays[DisplayStartAt].renderingHeight);
        float ratio => (float)res.x / (float)res.y;
        float radFOV => FOV * Mathf.Deg2Rad;
        double radHFOV => 2.0f * Math.Atan(Mathf.Tan(radFOV / 2) * ratio);
        float rot => (float)(radHFOV * Mathf.Rad2Deg);
        float start => ((CameraCount - 1) * rot) / -2;

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
            cameras.Clear();
        }

        [Button]
        public void UpdateSetup()
        {
            StartCoroutine(_UpdateSetup());
        }

        IEnumerator _UpdateSetup()
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
                camera_buffer.fieldOfView = FOV;
                camera_buffer.targetDisplay = DisplayStartAt + i;
                cameras.Add(camera_buffer);
                camera_object.transform.SetParent(transform, false);
                yield return new WaitForEndOfFrame();
                camera_object.transform.localPosition = Vector3.zero;
                camera_object.transform.localEulerAngles = new Vector3(0, start + rot * i, 0);
            }
        }
    }
}
