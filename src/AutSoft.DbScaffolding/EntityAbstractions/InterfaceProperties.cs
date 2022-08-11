namespace AutSoft.DbScaffolding.EntityAbstractions
{
    public class InterfaceProperties
    {
        public string InterfaceNameSpace { get; set; } = typeof(InterfaceProperties).Namespace;

        public string SoftDeletableInterfaceName { get; set; } = "ISoftDeletableEntity";
        public string AuditableInterfaceName { get; set; } = "IAuditableEntity";

        public string IsDeleted { get; set; } = nameof(IsDeleted);
        public string CreatedAt { get; set; } = nameof(CreatedAt);
        public string CreatedBy { get; set; } = nameof(CreatedBy);
        public string LastModAt { get; set; } = nameof(LastModAt);
        public string LastModBy { get; set; } = nameof(LastModBy);
    }
}
