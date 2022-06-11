using UnboundLib.GameModes;
using UnityEngine;
using System.Collections;
using LuckedOut.Extensions;
using ModdingUtils.MonoBehaviours;

namespace LuckedOut.MonoBehaviors
{
	class LuckyCharmsMono : MonoBehaviour
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
			reversibleEffect.gunAmmoStatModifier.maxAmmo_add = player.data.GetAdditionalData().luck;
			reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
			yield break;
		}

		private Player player;
		private ReversibleEffect reversibleEffect;
	}
}