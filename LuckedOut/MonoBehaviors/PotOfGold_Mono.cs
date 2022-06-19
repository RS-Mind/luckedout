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
			Destroy(reversibleEffect);
			GameModeManager.RemoveHook(GameModeHooks.HookBattleStart, BattleStart);
		}

		IEnumerator BattleStart(IGameModeHandler gm)
		{
			Destroy(reversibleEffect);
			reversibleEffect = player.gameObject.AddComponent<ReversibleEffect>();
			reversibleEffect.characterDataModifier.health_mult = 1 + (player.data.GetAdditionalData().luck * 0.05f);
			reversibleEffect.characterDataModifier.maxHealth_mult = 1 + (player.data.GetAdditionalData().luck * 0.05f);
			reversibleEffect.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1 - (player.data.GetAdditionalData().luck * 0.05f);
			reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
			yield break;
		}

		private Player player;
		private ReversibleEffect reversibleEffect;
	}
}