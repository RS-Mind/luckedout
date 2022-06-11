using ClassesManagerReborn;
using System.Collections;

namespace LuckedOut.Cards
{
    class LeprechaunClass : ClassHandler
    {
        internal static string name = "Leprechaun";

        public override IEnumerator Init()
        {
            CardInfo classCard = null;
            while (!(CoinGun.card && FourLeafClover.card && Leprechaun.card && LuckyCharms.card && PotOfGold.card)) yield return null;
            ClassesRegistry.Register(Leprechaun.card, CardType.Entry);
            ClassesRegistry.Register(CoinGun.card, CardType.Card, Leprechaun.card);
            ClassesRegistry.Register(FourLeafClover.card, CardType.Card, Leprechaun.card);
            ClassesRegistry.Register(LuckyCharms.card, CardType.Card, Leprechaun.card);
            ClassesRegistry.Register(PotOfGold.card, CardType.Card, Leprechaun.card);
            ClassesRegistry.Register(RussianRoulette.card, CardType.Card, Leprechaun.card);
        }
    }
}
