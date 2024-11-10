using Telerik.SvgIcons;

namespace RSCS.FinancingStatements.Web.Models
{
    public class AccesResquestParameter
    {
        public int NameID { get; set; }
        public int ResourceSecurityTypeID { get; set; }
        public string ResourceSecurityValue { get; set; }
        public string? LastModifiedBy { get; set; } = null;
        public string? Contact { get; set; }=null;
    }
}
