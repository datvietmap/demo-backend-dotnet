namespace InventoryManagement.Helpers
{
    public class Constants
    {
        public class Columns
        {
            public const string ID = "Id";
            public const string CODE = "Code";
            public const string IS_DELETED = "IsDeleted";
        }

        public class CustomClaimTypes
        {
            public const string ID = "id";
            public const string FULLNAME = "fullname";
            public const string EMAIL = "email";
            public const string PERMISSION = "permission";
        }

        public class ErrorMessage
        {
            public const string IsNullOrWhiteSpace = "Value cannot be empty or whitespace only string.";
            public const string IsDefaultValue = "Value cannot be default value.";
            public const string InvalidPaswordHash = "Invalid length of password hash (64 bytes expected).";
            public const string InvalidPasswordSalt = "Invalid length of password salt (128 bytes expected).";
        }
    }
}
