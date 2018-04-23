Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports System.Windows.Forms

Namespace MapArbitraryDatabaseExample
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			BindCategories()
			BindProducts()
		End Sub

		Private Sub BindCategories()
			gridControl1.DataSource = New XPCollection(session1, session1.GetClassInfo(String.Empty, "Categories"))
		End Sub

		Private Sub BindProducts()
			Dim dataLayer As SimpleDataLayer = CType(session1.DataLayer, SimpleDataLayer)
			Dim provider As ConnectionProviderSql = CType(dataLayer.ConnectionProvider, ConnectionProviderSql)
			Dim tables() As DBTable = provider.GetStorageTables("Products")
			gridControl2.DataSource = New XPCollection(session1, Program.AddClass(session1.Dictionary, tables(0)))
		End Sub
	End Class
End Namespace