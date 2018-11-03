using System;

namespace OblPR.Data.Entities
{
    [Serializable]
    public class Character
    {
        public Player CurentPlayer { get; private set; }
        public Role CharacterRole { get; private set; }

        private int _health;

        public Character(Player player, Role role)
        {
            CurentPlayer = player;
            CharacterRole = role;
            _health = GetHealthFromrole();
        }

        private int GetHealthFromrole()
        {
            if (CharacterRole == Role.Monster)
                return 100;
            if (CharacterRole == Role.Survivor)
                return 20;
            return 0;
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Ap
        {
            get { return GetApFromRole(); }
        }

        private int GetApFromRole()
        {
            if (CharacterRole == Role.Monster)
                return 10;
            if (CharacterRole == Role.Survivor)
                return 5;
            return 0;
        }

        public void Attack(Character defender)
        {
            if (CharacterRole == Role.Monster)
                defender.Health -= this.Ap;
            if (CharacterRole == Role.Survivor)
            {
                if (defender.CharacterRole == Role.Monster)
                    defender.Health -= this.Ap;
            }
        }
    }
}
