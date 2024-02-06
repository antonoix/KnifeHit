using Internal.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection
{
    public abstract class ServiceInjector<T> : ScriptableObject where T : class, IService
    {
        public T Service => _service ??= Create();

        protected T _service;

        public abstract T Create();
        public abstract void Initialize();
    }
}