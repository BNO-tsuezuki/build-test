namespace evogmtool
{
    public static class Enums
    {
        public enum AuthResult
        {
            Success = 0,
            InvalidAccount = 1,
            InvalidPassword = 2,
            Forbidden = 3,
            Lockout = 4,
        }
    }
}
