Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports System.Windows.Forms
Imports DevExpress.Xpo.Metadata
Imports DXSample
Imports System.Data.OleDb

Namespace MapArbitraryDatabaseExample
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread> _
		Shared Sub Main()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Using connection As IDbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\nwind.mdb")
				InitializeDataLayer(connection)
				Application.Run(New Form1())
			End Using
		End Sub

		Private Shared Sub InitializeDataLayer(ByVal connection As IDbConnection)
			Dim prov As ConnectionProviderSql = CType(XpoDefault.GetConnectionProvider(connection, AutoCreateOption.None), ConnectionProviderSql)
			Dim dict As XPDictionary = New ReflectionDictionary()
			Dim tables() As DBTable = prov.GetStorageTables("Categories")
			AddClass(dict, tables(0))
			XpoDefault.DataLayer = New SimpleDataLayer(dict, prov)
		End Sub

	Public Shared Function AddClass(ByVal dict As XPDictionary, ByVal table As DBTable) As XPClassInfo
		If table.PrimaryKey.Columns.Count > 1 Then
			Throw New NotSupportedException("Compound primary keys are not supported")
		End If
		Dim classInfo As XPClassInfo = dict.CreateClass(dict.GetClassInfo(GetType(BasePersistentClass)), table.Name.Replace("."c, "_"c))
		classInfo.AddAttribute(New PersistentAttribute(table.Name))
		Dim primaryColumnType As DBColumnType = table.GetColumn(table.PrimaryKey.Columns(0)).ColumnType
		classInfo.CreateMember(table.PrimaryKey.Columns(0), DBColumn.GetType(primaryColumnType), New KeyAttribute(IsAutoGenerationSupported(primaryColumnType)))
		For Each col As DBColumn In table.Columns
			If (Not col.IsKey) Then
				classInfo.CreateMember(col.Name, DBColumn.GetType(col.ColumnType))
			End If
		Next col
		Return classInfo
	End Function

		Private Shared Function IsAutoGenerationSupported(ByVal type As DBColumnType) As Boolean
			Return type = DBColumnType.Guid OrElse type = DBColumnType.Int16 OrElse type = DBColumnType.Int32
		End Function
	End Class
End Namespace