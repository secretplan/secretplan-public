using System;
using UnityEngine;

namespace SecretPlan.Core
{
    public class GameObjectProvider : ValueProvider<GameObject?>
    {
        public GameObjectProvider(GameObject? data) : base(data)
        {
        }

        public GameObjectProvider(Func<GameObject?> providerFunction) : base(providerFunction)
        {
        }

        public GameObjectProvider() : base((GameObject?)null)
        {
        }
    }
}