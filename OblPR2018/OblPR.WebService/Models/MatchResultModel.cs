using OblPR.Data.Entities;

namespace OblPR.WebService
{
    public class MatchResultModel
    {
        public string Nick { get; private set; }
        public string CharacterRole { get; private set; }
        public int  Result { get; private set; }

        public MatchResultModel(string nick, string role, int result)
        {
            Nick = nick;
            CharacterRole = role;
            Result = result;
        }
    }
}