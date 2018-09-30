namespace OblPR.Data.Entities
{
    public class Player
    {
        public Player(string nickname)
        {
            Nick = nickname;
        }
        public string Nick { get; }

        public override string ToString()
        {
            return Nick;
        }
    }
}
