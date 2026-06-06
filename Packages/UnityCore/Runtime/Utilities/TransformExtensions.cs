using System.Collections.Generic;
using UnityEngine;

namespace SecretPlan.Core
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform? transform)
        {
            if (transform == null)
            {
                return;
            }

            void DestroyChild(Transform child)
            {
                if (!Application.isPlaying || Application.exitCancellationToken.IsCancellationRequested)
                {
                    Object.DestroyImmediate(child.gameObject);
                }
                else
                {
                    Object.Destroy(child.gameObject);
                }
            }

            var childrenToDestroy = new List<Transform>();

            foreach (Transform child in transform)
            {
                childrenToDestroy.Add(child);
            }

            foreach (var child in childrenToDestroy)
            {
                DestroyChild(child);
            }
        }
    }
}