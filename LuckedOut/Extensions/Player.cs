using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LuckedOut.Extensions
{
    [Serializable]
    public class PlayerAdditionalData
    {
        public int luck;
        public int spins;

        public PlayerAdditionalData()
        {
            luck = 0;
        }
    }
    public static class PlayerExtension
    {
        public static readonly ConditionalWeakTable<CharacterData, PlayerAdditionalData> data =
            new ConditionalWeakTable<CharacterData, PlayerAdditionalData>();

        public static PlayerAdditionalData GetAdditionalData(this CharacterData chara)
        {
            return data.GetOrCreateValue(chara);
        }

        public static void AddData(this CharacterData chara, PlayerAdditionalData value)
        {
            try
            {
                data.Add(chara, value);
            }
            catch (Exception) { }
        }
    }
}