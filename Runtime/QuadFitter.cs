using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ellyality.Mapping
{
    [AddComponentMenu("Ellyality/Mapping/Quad Fitter")]
    [ExecuteAlways]
    public class QuadFitter : MonoBehaviour
    {
        Camera target => transform.GetComponentInParent<Camera>();

        private void Update()
        {
            Vector3 p1 = target.ViewportToWorldPoint(new Vector3(0, 0, target.nearClipPlane));
            Vector3 p2 = target.ViewportToWorldPoint(new Vector3(0, 1, target.nearClipPlane));
            Vector3 p3 = target.ViewportToWorldPoint(new Vector3(1, 0, target.nearClipPlane));

            float height = Mathf.Abs(p1.y - p2.y);
            float width = Mathf.Abs(p1.x - p3.x);
            transform.localScale = new Vector3(width, height, 1);
            transform.localPosition = new Vector3(0, 0, target.nearClipPlane);
        }
    }
}
