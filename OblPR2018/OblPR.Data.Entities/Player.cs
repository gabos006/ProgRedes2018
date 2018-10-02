namespace OblPR.Data.Entities
{
    public class Player
    {


        public Player(string nickname, string image)
        {
            Nick = nickname;

            Image = image;
        }

        public string Nick { get; }
        public string Image { get; } = "";


        public override string ToString()
        {
            return Nick;
        }
    }
}
