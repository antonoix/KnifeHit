using System;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource
{
    [Serializable]
    public class Resource
    {
        [SerializeField] private long _value;

        public ResourceType Key;

        public long Value
        {
            get => _value;
            set
            {
                if (_value == value) return;

                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public bool IsNotEmpty => Value > 0;

        public event Action<long> OnChanged;

        public Resource()
        { }

        public Resource(ResourceType key, long value = 0)
        {
            Key = key;
            _value = value;
        }

        public void Clear()
        {
            Value = 0;
        }

        public override string ToString()
        {
            return Value.ToString("N0");
        }
    }
}