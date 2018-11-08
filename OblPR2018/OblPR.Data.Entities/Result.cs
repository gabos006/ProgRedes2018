using System;

namespace OblPR.Data.Entities
{
    [Serializable]
    public class Result
    {
        public Character Character { get; private set; }
        public int Points { get; private set; }

        public Result(Character character, int points)
        {
            this.Character = character;
            this.Points = points;
        }
    }
}