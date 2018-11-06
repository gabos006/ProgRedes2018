using OblPR.Data.Entities;

namespace OblPR.WebService
{
    public class MatchResultModel
    {
        public string Nick;
        public string CharacterRole;
        public string Result;

        public MatchResultModel(string nick, string role, string result)
        {
            Nick = nick;
            CharacterRole = role;
            Result = result;
        }
    }
}