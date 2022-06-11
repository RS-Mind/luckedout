using BepInEx;
using HarmonyLib;
using LuckedOut.Cards;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using Jotunn.Utils;
using UnityEngine;

namespace LuckedOut
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.Root.Cards", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.temporarystatspatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class LuckedOut : BaseUnityPlugin
    {
        private const string ModId = "com.rsmind.rounds.commission.LuckedOut";
        private const string ModName = "Lucked Out";
        public const string Version = "1.1.2";
        public const string ModInitials = "LO";
        public static LuckedOut instance { get; private set; }

        void Awake()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        void Start()
        {
            instance = this;

            LuckedOut.ArtAssets = AssetUtils.LoadAssetBundleFromResources("rsluckedout", typeof(LuckedOut).Assembly);

            if (LuckedOut.ArtAssets == null)
            {
                UnityEngine.Debug.Log("Lucked Out art asset bundle either doesn't exist or failed to load.");
            }

            CustomCard.BuildCard<CoinGun>((card) => CoinGun.card = card);
            CustomCard.BuildCard<FourLeafClover>((card) => FourLeafClover.card = card);
            CustomCard.BuildCard<Leprechaun>((card) => Leprechaun.card = card);
            CustomCard.BuildCard<LuckyCharms>((card) => LuckyCharms.card = card);
            CustomCard.BuildCard<PotOfGold>((card) => PotOfGold.card = card);
            CustomCard.BuildCard<RussianRoulette>((card) => RussianRoulette.card = card);

            CustomCard.BuildCard<LeprechaunsCap>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); LeprechaunsCap.card = card; });
            CustomCard.BuildCard<Lucky>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); Lucky.card = card; });
            CustomCard.BuildCard<Luckier>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); Luckier.card = card; });
            CustomCard.BuildCard<Luckiest>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); Luckiest.card = card; });
            CustomCard.BuildCard<Unluckier>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); Unluckier.card = card; });
            CustomCard.BuildCard<Unlucky>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); Unlucky.card = card; });
            CustomCard.BuildCard<WeakPoison>((card) => { ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); WeakPoison.card = card; });

            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => RussianRoulette.WaitTillSpinDone());
        }

        public static bool Debug = false;
        internal static AssetBundle ArtAssets;
        internal static int luck;
    }
}