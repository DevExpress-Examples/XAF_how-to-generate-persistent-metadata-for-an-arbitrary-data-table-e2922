using System;
using System.Data;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Windows.Forms;
using DevExpress.Xpo.Metadata;
using DXSample;
using System.Data.OleDb;

namespace MapArbitraryDatabaseExample {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (IDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\nwind.mdb")) {
                InitializeDataLayer(connection);
                Application.Run(new Form1());
            }
        }

        static void InitializeDataLayer(IDbConnection connection) {
            ConnectionProviderSql prov = (ConnectionProviderSql)XpoDefault.GetConnectionProvider(connection, AutoCreateOption.None);
            XPDictionary dict = new ReflectionDictionary();
            DBTable[] tables = prov.GetStorageTables("Categories");
            AddClass(dict, tables[0]);
            XpoDefault.DataLayer = new SimpleDataLayer(dict, prov);
        }

	public static XPClassInfo AddClass(XPDictionary dict, DBTable table) {
	   if (table.PrimaryKey.Columns.Count > 1) 
		   throw new NotSupportedException("Compound primary keys are not supported");
	   XPClassInfo classInfo = dict.CreateClass(dict.GetClassInfo(typeof(BasePersistentClass)), table.Name.Replace('.', '_'));
	   classInfo.AddAttribute(new PersistentAttribute(table.Name));
	   DBColumnType primaryColumnType = table.GetColumn(table.PrimaryKey.Columns[0]).ColumnType;
	   classInfo.CreateMember(table.PrimaryKey.Columns[0], DBColumn.GetType(primaryColumnType), 
		   new KeyAttribute(IsAutoGenerationSupported(primaryColumnType)));
	   foreach (DBColumn col in table.Columns)
		   if (!col.IsKey)
			   classInfo.CreateMember(col.Name, DBColumn.GetType(col.ColumnType));
	   return classInfo;
	}

        static bool IsAutoGenerationSupported(DBColumnType type) {
            return type == DBColumnType.Guid || type == DBColumnType.Int16 || type == DBColumnType.Int32;
        }
    }        
}