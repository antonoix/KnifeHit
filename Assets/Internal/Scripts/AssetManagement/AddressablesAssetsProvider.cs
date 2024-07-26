using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Internal.Scripts.Infrastructure.AssetManagement
{
    public class AddressablesAssetsProvider : IAssetsProvider, IInitializable
    {
		private readonly Dictionary<string, AsyncOperationHandle> _competedCache = new();
		private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();
		private bool _isLogging;

		public void Initialize() =>
			Addressables.InitializeAsync();
		
		public async UniTask<GameObject> InstantiateAsync(AssetReference reference, Vector3 at, Transform parent)
		{
			var handle = Addressables.InstantiateAsync(reference, at, Quaternion.identity, parent);
			return await RunWithCacheOnComplete(handle, reference.AssetGUID);
		}

		public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class
		{
			if (_competedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(assetReference),
				cacheKey: assetReference.AssetGUID);
		}
		
		public async UniTask<T> LoadAsync<T>(string address) where T : class
		{
			if (_competedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;
			

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(address),
				cacheKey: address);
		}
		
		public async UniTask<List<T>> LoadAssetsByLabelAsync<T>(AssetLabelReference label) where T : class
		{
			var downloadHandle = Addressables.LoadAssetsAsync<T>(label, (addressable) => { });
			
			var a = await RunWithCacheOnComplete(
				downloadHandle,
				cacheKey: label.labelString);
			
			return a.ToList();
		}

		public async UniTask<List<T>> LoadAllAsyncByLabel<T>(string path)
		{
			var startTimeResourcesLoading = new Stopwatch();
			startTimeResourcesLoading.Start();
			List<string> keys = new List<string>();

			int prevInd = -1;
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i].Equals('/'))
				{
					var key = path.Substring(prevInd + 1, i - prevInd - 1);
					keys.Add(key);
					prevInd = i;
				}
			}

			if (prevInd > 0)
			{
				var key = path.Substring(prevInd + 1, path.Length - prevInd - 1);
				keys.Add(key);
			}
			else
			{
				keys.Add(path);
			}

			List<T> list = new List<T>();
			var taskResult = await Addressables
				.LoadResourceLocationsAsync(keys, Addressables.MergeMode.Intersection, null).ToUniTask();
			int handledCount = 0;
			bool getComponent = typeof(T).IsSubclassOf(typeof(MonoBehaviour));

			foreach (IResourceLocation resourceLocation in taskResult)
			{
				if (resourceLocation.ResourceType == typeof(GameObject))
				{
					//Debug.LogError("[Resources] load all " + path + " " + resourceLocation.ResourceType + " " + resourceLocation.InternalId + " " + resourceLocation.PrimaryKey + " " + resourceLocation.ProviderId);
					var result = await Addressables.LoadAssetAsync<GameObject>(resourceLocation).ToUniTask();
					//Debug.LogError("[Resources] load all " + path + " " + handle.Result + " " + (typeof(T).IsSubclassOf(typeof(MonoBehaviour))), handle.Result);
					if (getComponent) list.Add(result.GetComponent<T>());
					handledCount++;
					if (handledCount != taskResult.Count)
						continue;

					if (_isLogging)
						Debug.Log($"[Resources] [{path}] - loading time = {startTimeResourcesLoading.Elapsed.Seconds.ToString()}");

					return list;
				}
				else
				{
					var result = await Addressables.LoadAssetAsync<T>(resourceLocation).ToUniTask();
					list.Add(result);
					handledCount++;
					if (handledCount != taskResult.Count)
						continue;

					if (_isLogging)
						Debug.Log($"[Resources] [{path}] - loading time = {startTimeResourcesLoading.Elapsed.Seconds.ToString()}");

					return list;
				}
			}

			return null;
		}

		public void Release(AssetReference assetReference)
		{
			var assetGuid = assetReference.AssetGUID;
			ReleaseAsset(assetGuid);
		}
		
		public void Release(string assetGuid)
        {
            ReleaseAsset(assetGuid);
        }

		public void CleanUp()
		{
			foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
				foreach (AsyncOperationHandle handle in resourceHandles)
					Addressables.Release(handle);

			_competedCache.Clear();
			_handles.Clear();
		}


		private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey)
			where T : class
		{
			handle.Completed += completedHandle =>
			{
				_competedCache[cacheKey] = completedHandle;
			};

			AddHandle(cacheKey, handle);
			
			return await handle.ToUniTask();
		}


		private void ReleaseAsset(string assetGuid)
		{
			if (!_handles.ContainsKey(assetGuid))
				return;

			var handles = _handles[assetGuid];
			
			foreach (var handle in handles)
			{
				Addressables.Release(handle);
			}
			
			_competedCache.Remove(assetGuid);
			_handles.Remove(assetGuid);
		}

		private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
		{
			if (!_handles.TryGetValue(key,
				out List<AsyncOperationHandle> resourcesHandles))
			{
				resourcesHandles = new List<AsyncOperationHandle>();
				_handles[key] = resourcesHandles;
			}
			resourcesHandles.Add(handle);
		}
	}
}