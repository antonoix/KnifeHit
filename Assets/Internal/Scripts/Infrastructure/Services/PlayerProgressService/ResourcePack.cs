using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Internal.Scripts.Infrastructure.ResourceService;
using UnityEngine;
using UnityEngine.Rendering;

namespace InternalAssets.Scripts.Data.PlayerData.PlayerResources
{
    [Serializable]
    public class ResourcePack : IEnumerable<Resource>
    {
        [SerializeField] private SerializedDictionary<ResourceType, Resource> items = new SerializedDictionary<ResourceType, Resource>();
        
        public Dictionary<ResourceType, Resource> Items => items;

        /// <summary>
        /// Количество объектов с положительным значением
        /// </summary>
        public int Count => 
            items.Values.Count(r => r.IsNotEmpty);

        public event System.Action<ResourceType, long> OnChanged;


        // Пустой конструтор необходим для сереиализации/десериализации класса
        public ResourcePack() { }


        public ResourcePack(Resource resource)
        {
            Add(resource);
        }

        public ResourcePack(ResourcePack original)
        {
            Merge(original);
        }

        /// <summary>
        /// Упрощенная запись для получения ресурса
        /// </summary>
        public Resource this[ResourceType key] => 
            Get(key);

        /// <summary>
        /// Получить указанный ресурс
        /// </summary>
        public Resource Get(ResourceType key)
        {
            if (items.ContainsKey(key)) 
                return items[key];
            
            Resource emptyResource = new Resource(key, 0);
            items.Add(key, emptyResource);
            items[key].OnChanged += (long value) => OnChanged?.Invoke(key, value);
            return items[key];
        }

        /// <summary>
        /// Установить значение указанного ресурса
        /// </summary>
        public void Set(Resource item)
        {
            Get(item.Key).Value = item.Value;
        }

        /// <summary>
        /// Проверка есть ли ресурс в наличие
        /// </summary>
        public bool Contains(ResourceType key)
        {
            return Get(key).IsNotEmpty;
        }

        /// <summary>
        /// Проверить наличие данного ресурса.
        /// </summary>
        public bool CheckEnoughPrice(Resource price) =>
            CheckEnoughPrice(price, out long lacks);

        /// <summary>
        /// Проверить наличие номинала данного ресурса. lacks - сколько не хватает
        /// </summary>
        public bool CheckEnoughPrice(Resource price, out long lacks)
        {
            lacks = 0;
            
            if (price.Value == 0)
                return true;
            
            if (price.Value < 0)
                Debug.LogWarning($"Price is lower than 0: {price.Key}");
            
            // Если в паке ресурса меньше чем в цене
            if (Get(price.Key).Value < price.Value)
            {
                lacks = price.Value - Get(price.Key).Value;
            }
            return lacks == 0;
        }
        
        /// <summary>
        /// Проверить, что всех ресурсов в паке достаточно
        /// </summary>
        public bool CheckEnoughPrice(ResourcePack pricePack)
        {
            foreach (var price in pricePack)
            {
                if (price.Value < 0)
                    Debug.LogWarning($"Price is lower than 0: {price.Key}");

                if (Get(price.Key).Value < price.Value)
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// Вычесть из пака перечисленные ресурсы
        /// </summary>
        public void Subtract(params Resource[] resources)
        {
            SubtractInternal(resources);
        }
        
        /// <summary>
        /// Вычесть из пака элементы другого пака
        /// </summary>
        public void Subtract(ResourcePack pack)
        {
            SubtractInternal(pack.items.Values);
        }
        
        /// <summary>
        /// Вычесть из пака конкретный ресурс
        /// </summary>
        public void Subtract(Resource item) =>
            Get(item.Key).Value -= item.Value;

        /// <summary>
        /// Добавить в пак перечисленные итемы
        /// </summary>
        public void Add(params Resource[] items)
        {
            foreach (var c in items)
            {
                Get(c.Key).Value += c.Value;
            }
        }

        /// <summary>
        /// Добавить в пак конкретный ресурс
        /// </summary>
        public void Add(Resource item)
        {
            if (items.ContainsKey(item.Key))
            {
                items[item.Key].Value += item.Value;
                return;
            }
            
            items.Add(item.Key, item);
            items[item.Key].OnChanged += (long value) => OnChanged?.Invoke(item.Key, value);
        }

        /// <summary>
        /// Перезаписать текущий новым
        /// </summary>
        public void RewriteAll(ResourcePack newPack)
        {
            foreach (var item in this)
            {
                if (!newPack.Contains(item.Key))
                {
                    item.Value = 0;
                }
            }
            foreach (var item in newPack)
            {
                Get(item.Key).Value = item.Value;
            }
        }

        /// <summary>
        /// Очистить пак с ресурсами
        /// </summary>
        public void Clear()
        {
            foreach (var item in this)
            {
                item.Clear();
            }
        }

        /// <summary>
        /// Добавить все ресурсы из другого пака в этот
        /// </summary>
        public ResourcePack Merge(ResourcePack newPack)
        {
            if (newPack == null) return this;
            
            foreach (var resource in newPack)
            {
                Get(resource.Key).Value += resource.Value;
            }
            return this;
        }

        public IEnumerator<Resource> GetEnumerator()
        {
            foreach (Resource item in items.Values)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Вычесть из пака любую коллекцию ресурсов
        /// </summary>
        private void SubtractInternal(IEnumerable<Resource> resources)
        {
            foreach (var resource in resources)
            {
                Get(resource.Key).Value -= resource.Value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (Resource item in this)
            {
                builder.AppendFormat($"[{item.Key}] = {item.Value}, ");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public static ResourcePack operator *(ResourcePack pack, float multiplier)
        {
            foreach (var item in pack.Items.Values)
            {
                var result = item.Value * multiplier;
                item.Value = (long)result;
            }
            return pack;
        }
    }
}