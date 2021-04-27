using System.ComponentModel.DataAnnotations;

namespace evogmtool.Attributes
{
    public class PasswordAttribute : RegularExpressionAttribute
    {
        // todo: mask 処理の都合上 " は許可しない方がいいかも
        // todo: error message
        // todo: test escape
        private const string pattern = @"^[a-zA-Z0-9 !""#$%&'()*+,\-./:;<=>?@[\\\]\^_`{|}~]+$";

        public PasswordAttribute() :
            base(pattern)
        { }
    }
}
