
namespace OblPR.Game
{
    public interface IGameController
    {
        void Attack(ICharacterHandler characterHandler);
        void PlayerExit(ICharacterHandler characterHandler);
        void MovePlayer(ICharacterHandler characterHandler, Point position);
    }
}