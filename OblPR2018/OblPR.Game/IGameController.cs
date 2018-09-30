
namespace OblPR.Game
{
    public interface IGameController
    {
        void Attack(ICharacterHandler characterHandler);
        void PlayerExit(ICharacterHandler characterHandler, string result);
        void MovePlayer(ICharacterHandler characterHandler, Point position);
    }
}