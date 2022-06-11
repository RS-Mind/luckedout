using System;
using System.Linq;
using HarmonyLib;
using LuckedOut.Extensions;
using UnboundLib;

namespace LuckedOut.Patches // This is Root's Code
{
	[HarmonyPatch]
	[Serializable]
	public class EfficientGetRandomCard
	{
		public static void PrefixMthod(CardChoice cardChoice)
		{
			Player player = EfficientGetRandomCard.PickingPlayer(cardChoice);
			if (player != null)
			{
				LuckedOut.luck = player.data.GetAdditionalData().luck;
			}
		}

		public static void PostfixMthod()
		{
			LuckedOut.luck = 0;
		}

		internal static Player PickingPlayer(CardChoice cardChoice)
		{
			Player result = null;
			try
			{
				if ((PickerType)ExtensionMethods.GetFieldValue(cardChoice, "pickerType") == PickerType.Team)
				{
					result = PlayerManager.instance.GetPlayersInTeam(cardChoice.pickrID).FirstOrDefault<Player>();
				}
				else
				{
					result = ((cardChoice.pickrID < PlayerManager.instance.players.Count<Player>() && cardChoice.pickrID >= 0) ? PlayerManager.instance.players[cardChoice.pickrID] : null);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
