using ClassesManagerReborn.Util;
using LuckedOut.Extensions;
using System.Collections;
using TMPro;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnityEngine;

namespace LuckedOut.Cards
{
    class RussianRoulette : CustomCard
    {
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = LeprechaunClass.name;
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
            if (LuckedOut.Debug) { UnityEngine.Debug.Log($"[{LuckedOut.ModInitials}][Card] {GetTitle()} has been setup."); }
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Edits values on player when card is selected
            player.data.GetAdditionalData().spins += 1;
            if (LuckedOut.Debug) { UnityEngine.Debug.Log($"[{LuckedOut.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}."); }
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
            if (LuckedOut.Debug) { UnityEngine.Debug.Log($"[{LuckedOut.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}."); }
        }

        internal static CardInfo card = null;

        protected override string GetTitle()
        {
            return "Russian Roulette";
        }
        protected override string GetDescription()
        {
            return "Spin the wheel and test your luck! (not affected by luck stat)";
        }
        protected override GameObject GetCardArt()
        {
            return LuckedOut.ArtAssets.LoadAsset<GameObject>("C_RussianRoulette");
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.PoisonGreen;
        }
        public override string GetModName()
        {
            return LuckedOut.ModInitials;
        }

        internal static IEnumerator WaitTillSpinDone()
        {
            bool done = true;
            GameObject gameObject = null;
            GameObject timer = null;
            float time = 7;
            PlayerManager.instance.players.ForEach(p =>
            {
                if (p.data.GetAdditionalData().spins > 0) 
                {
                    spinWheel(p);
                    done = false;
                }
            });

            if (!done)
            {
                gameObject = new GameObject();
                gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
                gameObject.AddComponent<TextMeshProUGUI>().text = "Waiting For Wheel Spins";
                Color c = Color.yellow;
                c.a = .85f;
                gameObject.GetComponent<TextMeshProUGUI>().color = c;
                gameObject.transform.localScale = new Vector3(.2f, .2f);
                gameObject.transform.localPosition = new Vector3(0, 5);
                timer = new GameObject();
                timer.AddComponent<Canvas>().sortingLayerName = "MostFront";
                timer.transform.localScale = new Vector3(.2f, .2f);
                timer.transform.localPosition = new Vector3(0, 16);
                timer.AddComponent<TextMeshProUGUI>().color = c;
                for (int i = 0; i < 5; i++)
                {
                    timer.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
                    yield return new WaitForSecondsRealtime(1f);
                    time -= 1;
                }
            }

            while (!done)
            {
                timer.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
                yield return new WaitForSecondsRealtime(0.2f);
                time -= 0.2f;
                if (time < 0) done = true;
            }
            Destroy(gameObject);
            Destroy(timer);
        }

        internal static void spinWheel(Player player)
        {
            player.gameObject.GetOrAddComponent<Wheel>();
        }
    }

    class Wheel : MonoBehaviour
    {
        private Player player;
        private GameObject wheel;
        private GameObject triangle;
        private int rand = 0;
        private float speed;
        private float startDelay = 0;
        private float endDelay = 1f;
        private float moveTime = 0.5f;
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        private void Start()
        {
            player = GetComponentInParent<Player>();
            triangle = Instantiate(LuckedOut.ArtAssets.LoadAsset<GameObject>("Triangle"));
            triangle.GetComponent<SpriteRenderer>().sortingLayerName = "MostFront";
            triangle.transform.SetPositionAndRotation(new Vector3(0, -24, -2), new Quaternion(0, 0, 0, 0));
            wheel = Instantiate(LuckedOut.ArtAssets.LoadAsset<GameObject>("Wheel"));
            wheel.GetComponent<SpriteRenderer>().sortingLayerName = "MostFront";
            wheel.transform.SetPositionAndRotation(new Vector3(0, -40, -1), new Quaternion(0, 0, 0, 0));
            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            rand = Random.Range(0, 54);
            if (rand == 0) // 0 genie
            {
                speed = 728.5f;
            }
            if (rand > 0) // 1-3 unluckier
            {
                speed = 733f;
            }
            if (rand > 3) // 4-11 foes -1 point
            {
                speed = 740f;
            }
            if (rand > 11) // 12-16 unlucky
            {
                speed = 750f;
            }
            if (rand > 16) // 17-24 Lucky
            {
                speed = 762f;
            }
            if (rand > 24) // 25-29 -1 point
            {
                speed = 773f;
            }
            if (rand > 29) // 30-37 Luckier
            {
                speed = 694f;
            }
            if (rand > 37) // 38-42 Unlucky
            {
                speed = 705f;
            }
            if (rand > 42) // 43-50 Luckiest
            {
                speed = 717f;
            }
            if (rand > 50) // 51-53 unluckier
            {
                speed = 724f;
            }
        }

        private void FixedUpdate()
        {
            if (startDelay < moveTime)
            {
                startDelay += TimeHandler.deltaTime;
                wheel.transform.SetYPosition(-40 * ((0.5f * Mathf.Cos((startDelay * Mathf.PI) / moveTime)) + 0.5f));
                triangle.transform.SetYPosition(wheel.transform.position.y + 16);
                return;
            }
            if (startDelay < 1f)
            {
                startDelay += TimeHandler.deltaTime;
                return;
            }
            if (speed > 0)
            {
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + (speed * TimeHandler.deltaTime));
                wheel.transform.rotation = rotation;
                speed -= TimeHandler.deltaTime * 185;
                return;
            }
            if (endDelay > moveTime)
            {
                endDelay -= TimeHandler.deltaTime;
                return;
            }
            if (endDelay > 0)
            {
                endDelay -= TimeHandler.deltaTime;
                wheel.transform.SetYPosition(-40 * ((0.5f * Mathf.Cos((endDelay * Mathf.PI) / moveTime)) + 0.5f));
                triangle.transform.SetYPosition(wheel.transform.position.y + 16);
                return;
            }
            if (rand == 0)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie"), false, "", 0, 0);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie"), false, "", 0, 0);
            }
            if (rand > 0 && rand < 4)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Unluckier.card, false, "", 0, 0);
            }
            if (rand > 5 && rand < 12)
            {
                PlayerManager.instance.players.ForEach(p =>
                {
                    if (p.playerID != player.playerID)
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, WeakPoison.card, false, "", 0, 0);
                    }
                });
            }
            if (rand > 11 && rand < 17)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Unlucky.card, false, "", 0, 0);
            }
            if (rand > 16 && rand < 25)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Lucky.card, false, "", 0, 0);
            }
            if (rand > 24 && rand < 30)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, WeakPoison.card, false, "", 0, 0);
            }
            if (rand > 29 && rand < 38)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Luckier.card, false, "", 0, 0);
            }
            if (rand > 37 && rand < 43)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Unlucky.card, false, "", 0, 0);
            }
            if (rand > 42 && rand < 51)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Luckiest.card, false, "", 0, 0);
            }
            if (rand > 50)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, Unluckier.card, false, "", 0, 0);
            }
            Destroy(this);
        }

        private void OnDestroy()
        {
            Destroy(wheel);
            Destroy(triangle);
        }
    }
}
