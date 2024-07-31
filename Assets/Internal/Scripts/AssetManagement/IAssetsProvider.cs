using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Internal.Scripts.AssetManagement
{
    public interface IAssetsProvider : IService
    {
        UniTask<GameObject> InstantiateAsync(AssetReference reference, Vector3 at, Transform parent);
        UniTask<List<T>> LoadAllAsyncByLabel<T>(string path);
        UniTask<List<T>> LoadAssetsByLabelAsync<T>(AssetLabelReference label) where T : class;
        UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        UniTask<T> LoadAsync<T>(string address) where T : class;
        void CleanUp();
        void Release(AssetReference assetReference);
        void Release(string assetGuid);
        UniTask Initialize();
    }
}