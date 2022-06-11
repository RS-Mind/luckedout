using UnboundLib.GameModes;
using UnityEngine;
using System.Collections;
using LuckedOut.Cards;
using LuckedOut.Extensions;
using ModdingUtils.MonoBehaviours;

namespace LuckedOut.MonoBehaviors
{
	class LeprechaunMono : MonoBehaviour
	{
		private void Start()
        {
			player = GetComponentInParent<Player>();
			GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);
			GameModeManager.AddHook(GameModeHooks.HookBattleStart, BattleStart);
		}

		private void OnDestroy()
		{
			GameModeManager.RemoveHook(GameModeHooks.HookPointStart, PointStart);
			GameModeManager.RemoveHook(GameModeHooks.HookBattleStart, BattleStart);
		}

		IEnumerator PointStart(IGameModeHandler gm)
		{
			player.data.GetAdditionalData().luck = 0;
			foreach (CardInfo card in player.data.currentCards)
            {
				if (card.rarity == CardInfo.Rarity.Rare || card == Lucky.card)
                {
					player.data.GetAdditionalData().luck++;
                }
				if (card == Leprechaun.card || card == Luckier.card)
                {
					player.data.GetAdditionalData().luck += 2;
				}
				if (card == PotOfGold.card || card == Luckiest.card)
				{
					player.data.GetAdditionalData().luck += 3;
				}
				if (card == FourLeafClover.card)
				{
					player.data.GetAdditionalData().luck += 4;
				}
				if (card == Unlucky.card)
				{
					player.data.GetAdditionalData().luck -= 1;
				}
				if (card == Unluckier.card)
				{
					player.data.GetAdditionalData().luck -= 2;
				}
			}
			if (player.data.GetAdditionalData().luck >= 15)
            {
				bool cap = false;
				foreach (CardInfo card in player.data.currentCards)
                {
					if (card == LeprechaunsCap.card)
                    {
						cap = true;
						break;
                    }
                }
				if (!cap)
                {
					ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, LeprechaunsCap.card, false, "", 0, 0);
				}
				player.data.GetAdditionalData().luck = 15;
            }

			yield break;
		}

		IEnumerator BattleStart(IGameModeHandler gm)
		{
			ReversibleEffect reversibleEffect = player.gameObject.AddComponent<ReversibleEffect>();
			reversibleEffect.characterStatModifiersModifier.movementSpeed_mult = 1 + (player.data.GetAdditionalData().luck * 0.1f);
			reversibleEffect.gunStatModifier.projectileSpeed_add = (player.data.GetAdditionalData().luck * 0.15f);
			reversibleEffect.characterStatModifiersModifier.sizeMultiplier_mult = 1 - (player.data.GetAdditionalData().luck * 0.025f);
			reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
			yield break;
		}

		private Player player;
	}
}