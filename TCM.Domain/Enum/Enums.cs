using System.ComponentModel;

namespace TCM.Domain.Enum
{
    public static class Enums
    {
        public enum Status
        {
            [Description("Active")]
            Active = 0,
            [Description("Deleted")]
            Deleted = 1
        }

        public enum UserRoles
        {
            [Description("Principal")]
            Principal = 1,
            [Description("Teacher")]
            Teacher = 2,
            [Description("Student")]
            Student = 3,
            [Description("Parent")]
            Parent = 4
        }
    }
}
