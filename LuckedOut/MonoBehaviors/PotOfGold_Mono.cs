using UnboundLib.GameModes;
using UnityEngine;
using System.Collections;
using LuckedOut.Extensions;
using ModdingUtils.MonoBehaviours;

namespace LuckedOut.MonoBehaviors
{
	class PotOfGoldMono : MonoBehaviour
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
			reversibleEffect.characterStatModifiersModifier.health_add = (player.data.GetAdditionalData().luck * 0.05f);
			reversibleEffect.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1 - (player.data.GetAdditionalData().luck * 0.05f);
			reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
			yield break;
		}

		private Player player;
	}
}