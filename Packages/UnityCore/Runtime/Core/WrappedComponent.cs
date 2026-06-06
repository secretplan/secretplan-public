using UnityEngine;

namespace SecretPlan.Core
{
    public class WrappedComponent<T> where T : Component
    {
        private Vector3 _savedPosition;

        public WrappedComponent(T? component)
        {
            Component = component;
        }

        public WrappedComponent()
        {
            Component = null;
        }

        public T? Component { get; }

        public Vector2 Position
        {
            get
            {
                if (Component && Component.transform != null)
                {
                    _savedPosition = Component.transform.position;
                    return Component.transform.position;
                }

                return _savedPosition;
            }
            set
            {
                if (Component && Component.transform != null)
                {
                    Component.transform.position = value;
                    _savedPosition = value;
                }
            }
        }

        public bool IsDestroyed()
        {
            return Component == null;
        }

        public void DestroyGameObject()
        {
            if (Component)
            {
                Object.Destroy(Component.gameObject);
            }
        }
    }
}