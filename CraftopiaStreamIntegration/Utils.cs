using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Humanizer;
using Newtonsoft.Json;
using Oc;
using Oc.Item;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CraftopiaStreamIntegration
{
    public class Utils
    {
        public static bool InGame => SingletonMonoBehaviour<OcGameMng>.Inst && SingletonMonoBehaviour<OcGameMng>.Inst.IsInGame;
        
        public static bool InvertControls { get; set; }
        
        public static bool InvertMouse { get; set; }
        
        public static Stack InvertControlsStack { get; } = new Stack();
        
        public static Stack InvertMouseStack { get; } = new Stack();
        
        public static Stack<float> ChangeSensitivityStack { get; } = new Stack<float>();
        
        public static Vector3 RandomVector3(float range) => new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        
        public static Vector3 GetBoundedRandVector(float min, float max)
        {
            var x = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            var y = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            var z = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            return new Vector3(x, y, z);
        }

        public static Vector3 GetRandomVector3()
        {
            return GetBoundedRandVector(0, 1).normalized;
        }

        public static void DumpItems()
        {
            var items = new JsonArray();
            var itemDataMng = SingletonMonoBehaviour<OcItemDataMng>.Inst;
            var itemData = (ItemData[]) AccessTools.Field(typeof(OcItemDataMng), "validItemDataList").GetValue(itemDataMng);
            items.AddRange(itemData.Select(item =>
                {
                    var cat = item.ItemCategory.Humanize(LetterCasing.Title);
                    return new JsonItem
                    {
                        ID = item.Id,
                        DisplayName = item.DisplayName,
                        Description = item.Description,
                        Rarity = item.DisplayRarityType.Humanize(LetterCasing.Title),
                        Category = cat.Length > 4 ? cat.Substring(4): cat,
                        Durability = item.DurabilityValue,
                        Image = Convert.ToBase64String(DuplicateTexture(item.ItemImage_Icon.texture).EncodeToPNG())
                    };
                })
                .Cast<object>());
            

            var itemString = JsonConvert.SerializeObject(items, Formatting.None);
            File.WriteAllText(Application.dataPath + "/items.json", itemString);
            CSIPlugin.Instance.Log.LogDebug(Application.dataPath + "/items.json");
            
            var enchantments = new JsonArray();
            enchantments.AddRange(OcResidentData.EnchantDataList.GetAll().Where(e => e.IsEnabled).Select(enchantment => new JsonEnchantment
                {
                    ID = enchantment.ID,
                    DisplayName = enchantment.DisplayName,
                    Description = enchantment.Description,
                    Rarity = enchantment.Rarity.Humanize(LetterCasing.Title)
                })
                .Cast<object>());
            var enchantmentString = JsonConvert.SerializeObject(enchantments, Formatting.None);
            File.WriteAllText(Application.dataPath + "/enchantments.json", enchantmentString);
            CSIPlugin.Instance.Log.LogDebug(Application.dataPath + "/enchantments.json");
        }


        private static Texture2D DuplicateTexture(Texture source)
        {
            var renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(source, renderTexture);
            Graphics.Blit(source, renderTexture);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTexture;
            var readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);
            return readableText;
        }
        
        private struct JsonItem
        {
            [JsonProperty("id")]
            public int ID;
            
            [JsonProperty("display_name")]
            public string DisplayName;
            
            [JsonProperty("description")]
            public string Description;

            [JsonProperty("rarity")]
            public string Rarity;
            
            [JsonProperty("category")]
            public string Category;
            
            [JsonProperty("durability")]
            public float Durability;

            [JsonProperty("image")]
            public string Image;
        }
        
        private struct JsonEnchantment
        {
            [JsonProperty("id")]
            public int ID;
            
            [JsonProperty("display_name")]
            public string DisplayName;
            
            [JsonProperty("description")]
            public string Description;

            [JsonProperty("rarity")]
            public string Rarity;
        }
    }
}