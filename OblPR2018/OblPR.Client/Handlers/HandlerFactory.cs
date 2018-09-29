using OblPR.Protocol;

namespace OblPR.Client
{
    public static class HandlerFactory
    {
        public static IHandler Handler(int? selectedCommand)
        {
            switch (selectedCommand)
            {
                case ClientCommand.LOGIN:
                    return new Login();

                case ClientCommand.ADD_PLAYER:
                    return new AddPlayer();

                default:
                    return null;
            }
        }
    }
}
