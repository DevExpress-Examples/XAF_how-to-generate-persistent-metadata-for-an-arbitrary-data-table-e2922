using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Windows.Forms;

namespace MapArbitraryDatabaseExample {
    public partial class Form1 :Form {
        public Form1() {
            InitializeComponent();
            BindCategories();
            BindProducts();
        }

        void BindCategories() {
            gridControl1.DataSource = new XPCollection(session1, session1.GetClassInfo(string.Empty, "Categories"));
        }

        void BindProducts() {
            SimpleDataLayer dataLayer = (SimpleDataLayer)session1.DataLayer;
            ConnectionProviderSql provider = (ConnectionProviderSql)dataLayer.ConnectionProvider;
            DBTable[] tables = provider.GetStorageTables("Products");
            gridControl2.DataSource = new XPCollection(session1, Program.AddClass(session1.Dictionary, tables[0]));
        }
    }
}