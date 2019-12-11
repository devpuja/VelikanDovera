namespace SmearAdmin.ViewModels
{
    public class ChemistStockistViewModel
    {
        public string ID { get; set; }
        public ContactResourseViewModel Contact { get; set; }
        public CommonResourceViewModel Common { get; set; }
        public AuditableEntityViewModel AuditableEntity { get; set; }
    }
}