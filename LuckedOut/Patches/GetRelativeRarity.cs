using System;
using HarmonyLib;

namespace LuckedOut.Patches // This is Root's Code
{
	[HarmonyPatch]
	[Serializable]
	public class GetRelativeRarity
	{
		public static void PostfixMthod(ref float __result, CardInfo card)
		{
			if (card.rarity == CardInfo.Rarity.Rare)
			{
				__result *= 1f + (LuckedOut.luck * 0.2f);
			}
		}
	}
}
