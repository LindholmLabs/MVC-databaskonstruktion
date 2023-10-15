using System.Data;

namespace MVC_databaskonstruktion.Utils
{
    public struct TableObject
    {
        public DataTable DataSet;
        public string ControllerName;
        public string? DeleteTable;
        public string? DeletionController;
        public List<string>? PrimaryKeys;
        public string? Redirect;
    }
}
