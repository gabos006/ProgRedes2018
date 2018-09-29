using OblPR.Protocol;

namespace OblPR.Client
{
    public static class HandlerFactory
    {
        public static IHandler Handler(int? selectedCommand)
        {
            switch (selectedCommand)
            {
                case Command.LOGIN:
                    return new Login();

                case Command.ADD_PLAYER:
                    return new AddPlayer();

                case Command.LOGOUT:
                    return new Logout();

                default:
                    return null;
            }
        }
    }
}
