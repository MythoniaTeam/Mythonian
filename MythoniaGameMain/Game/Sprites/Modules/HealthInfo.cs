



namespace Mythonia.Game.Sprites.Modules
{
    public class HealthInfo
    {
        public int HealthPoint { get; set; }
        public int HealthMax { get; private set; }
        public int Defence { get; set; }


        public delegate bool DeathEvent(DamageInfo damage);
        public event DeathEvent OnDeath;
        public delegate void DealDamageEvent(DamageInfo damage);
        public event DealDamageEvent OnDealDamage;


        public HealthInfo(int health, int defence)
        {
            HealthMax = HealthPoint = health;
            Defence = defence;

            OnDealDamage += EntityHealthInfo_OnDealDamage;
        }

        private void EntityHealthInfo_OnDealDamage(DamageInfo damage)
        {
            damage.DamageValue -= Defence;
            if (damage.DamageValue <= 0) damage.DamageValue = 1;
            //string t = FrameCounter.Ins.FrameCount.ToString();
            //string d = damage.DamageValue.ToString();
            //TextManager.Ins.WriteLine(() => $"{t}: OnDealDamage, Damage {d}", 2600);
        }

        public bool DealDamage(DamageInfo damage)
        {
            damage = damage.Clone();
            //string t = FrameCounter.Ins.FrameCount.ToString();
            //string hp = HealthPoint.ToString();
            //TextManager.Ins.WriteLine(() => $"{t}: Before DealDamage, HP {hp}", 2600);

            OnDealDamage(damage);
            HealthPoint -= damage.DamageValue;

            //string hp2 = HealthPoint.ToString();
            //TextManager.Ins.WriteLine(() => $"{t}: After DealDamgae, HP {hp2}", 2600);

            if (HealthPoint <= 0) return OnDeath(damage);
            return false;
        }
    }
}
