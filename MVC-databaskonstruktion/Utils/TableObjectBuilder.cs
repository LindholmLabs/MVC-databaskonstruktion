using System.Data;

namespace MVC_databaskonstruktion.Utils
{
    public class TableObjectBuilder
    {
        private DataTable _dataTable;
        private string _controllerName;
        private string? _deleteTable;
        private List<string>? _primaryKeys;
        private string? _redirect;

        public TableObjectBuilder(DataTable dataTable)
        {
            _dataTable = dataTable;
        }
        public TableObjectBuilder()
        {
            //empty constructor if you want to set the DataTable later
        }

        public TableObjectBuilder SetControllerName(string controllerName)
        {
            _controllerName = controllerName;
            return this;
        }

        public TableObjectBuilder SetPrimaryKeys(List<string> primaryKeys)
        {
            _primaryKeys = primaryKeys;
            return this;
        }

        public TableObjectBuilder SetDeleteTable(string tableName)
        {
            _deleteTable = tableName;
            return this;
        }

        public TableObjectBuilder SetDataTable(DataTable dataTable)
        {
            _dataTable = dataTable;
            return this;
        }

        public TableObjectBuilder SetRedirect(string redirect)
        {
            _redirect = redirect;
            return this;
        }

        public TableObject Build()
        {
            if (_dataTable == null)
                throw new NullReferenceException("DataTable is null.");

            return new TableObject
            {
                DataSet = this._dataTable,
                ControllerName = this._controllerName,
                DeleteTable = this._deleteTable,
                PrimaryKeys = this._primaryKeys,
                Redirect = this._redirect
            };
        }
    }
}
