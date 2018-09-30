
namespace OblPR.Game
{
    public interface IGameLogic
    {
        void Attack(ICharacterHandler characterHandler);
        void PlayerExit(ICharacterHandler characterHandler, string result);
        void MovePlayer(ICharacterHandler characterHandler, Point position);
    }
}