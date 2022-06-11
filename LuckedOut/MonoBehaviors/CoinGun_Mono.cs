using UnboundLib.GameModes;
using UnityEngine;
using System.Collections;
using LuckedOut.Extensions;
using ModdingUtils.MonoBehaviours;

namespace LuckedOut.MonoBehaviors
{
	class CoinGunMono : MonoBehaviour
	{
		private void Start()
        {
			player = GetComponentInParent<Player>();
			GameModeManager.AddHook(GameModeHooks.HookBattleStart, BattleStart);
		}

		private void OnDestroy()
		{
			GameModeManager.RemoveHook(GameModeHooks.HookBattleStart, BattleStart);
		}

		IEnumerator BattleStart(IGameModeHandler gm)
		{
			ReversibleEffect reversibleEffect = player.gameObject.AddComponent<ReversibleEffect>();
			reversibleEffect.gunStatModifier.damage_add = (player.data.GetAdditionalData().luck * 0.25f);
			reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
			yield break;
		}

		private Player player;
	}
}