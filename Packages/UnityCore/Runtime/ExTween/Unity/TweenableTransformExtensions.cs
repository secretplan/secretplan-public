using UnityEngine;

namespace ExTween.Unity
{
    public static class TweenableTransformExtensions
    {
        public static TweenableVector3 GetTweenableLocalScale(this Transform transform)
        {
            var lastKnownValue = transform.localScale;
            return new TweenableVector3(() =>
            {
                if (transform)
                {
                    lastKnownValue = transform.localScale;
                }

                return lastKnownValue;
            }, value =>
            {
                if (transform)
                {
                    transform.localScale = value;
                }
            });
        }

        public static TweenableVector3 GetTweenableLocalPosition(this Transform transform)
        {
            var lastKnownValue = transform.localPosition;

            return new TweenableVector3(() =>
            {
                if (transform)
                {
                    lastKnownValue = transform.localPosition;
                }

                return lastKnownValue;
            }, value =>
            {
                if (transform)
                {
                    transform.localPosition = value;
                }
            });
        }

        public static TweenableVector3 GetTweenablePosition(this Transform transform)
        {
            var lastKnownValue = transform.position;

            return new TweenableVector3(() =>
            {
                if (transform)
                {
                    lastKnownValue = transform.position;
                }

                return lastKnownValue;
            }, value =>
            {
                if (transform)
                {
                    transform.position = value;
                }
            });
        }

        public static TweenableQuaternion GetTweenableRotation(this Transform transform)
        {
            var lastKnownValue = transform.rotation;

            return new TweenableQuaternion(() =>
            {
                if (transform)
                {
                    lastKnownValue = transform.rotation;
                }

                return lastKnownValue;
            }, value =>
            {
                if (transform)
                {
                    transform.rotation = value;
                }
            });
        }

        public static TweenableQuaternion GetTweenableLocalRotation(this Transform transform)
        {
            var lastKnownValue = transform.rotation;

            return new TweenableQuaternion(() =>
            {
                if (transform)
                {
                    lastKnownValue = transform.localRotation;
                }

                return lastKnownValue;
            }, value =>
            {
                if (transform)
                {
                    transform.localRotation = value;
                }
            });
        }

        public static TweenableFloat GetTweenablePositionX(this Transform transform)
        {
            return new TweenableFloat(() => transform.position.x,
                value => transform.position = new Vector3(value, transform.position.y, transform.position.z));
        }

        public static TweenableFloat GetTweenablePositionY(this Transform transform)
        {
            return new TweenableFloat(() => transform.position.y,
                value => transform.position = new Vector3(transform.position.x, value, transform.position.z));
        }

        public static TweenableFloat GetTweenablePositionZ(this Transform transform)
        {
            return new TweenableFloat(() => transform.position.z,
                value => transform.position = new Vector3(transform.position.x, transform.position.y, value));
        }

        public static TweenableFloat GetTweenableLocalPositionX(this Transform transform)
        {
            return new TweenableFloat(() => transform.localPosition.x,
                value => transform.localPosition =
                    new Vector3(value, transform.localPosition.y, transform.localPosition.z));
        }

        public static TweenableFloat GetTweenableLocalPositionY(this Transform transform)
        {
            return new TweenableFloat(() => transform.localPosition.y,
                value => transform.localPosition =
                    new Vector3(transform.localPosition.x, value, transform.localPosition.z));
        }

        public static TweenableFloat GetTweenableLocalPositionZ(this Transform transform)
        {
            return new TweenableFloat(() => transform.localPosition.z,
                value => transform.localPosition =
                    new Vector3(transform.localPosition.x, transform.localPosition.y, value));
        }
    }
}