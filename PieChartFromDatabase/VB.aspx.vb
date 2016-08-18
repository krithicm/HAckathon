Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting

Partial Class VB
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim query As String = "select distinct shipcountry from orders"
            Dim dt As DataTable = GetData(query)
            ddlCountries.DataSource = dt
            ddlCountries.DataTextField = "shipcountry"
            ddlCountries.DataValueField = "shipcountry"
            ddlCountries.DataBind()
            ddlCountries.Items.Insert(0, New ListItem("Select", ""))
        End If
    End Sub


    Protected Sub ddlCountries_SelectedIndexChanged(sender As Object, e As EventArgs)
        Chart1.Visible = ddlCountries.SelectedValue <> ""
        Dim query As String = String.Format("select shipcity, count(orderid) from orders where shipcountry = '{0}' group by shipcity", ddlCountries.SelectedValue)
        Dim dt As DataTable = GetData(query)
        Dim x As String() = New String(dt.Rows.Count - 1) {}
        Dim y As Integer() = New Integer(dt.Rows.Count - 1) {}
        For i As Integer = 0 To dt.Rows.Count - 1
            x(i) = dt.Rows(i)(0).ToString()
            y(i) = Convert.ToInt32(dt.Rows(i)(1))
        Next
        Chart1.Series(0).Points.DataBindXY(x, y)
        Chart1.Series(0).ChartType = SeriesChartType.Pie
        Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        Chart1.Legends(0).Enabled = True
    End Sub

    Private Shared Function GetData(query As String) As DataTable
        Dim dt As New DataTable()
        Dim cmd As New SqlCommand(query)
        Dim constr As [String] = ConfigurationManager.ConnectionStrings("ConString").ConnectionString
        Dim con As New SqlConnection(constr)
        Dim sda As New SqlDataAdapter()
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        sda.SelectCommand = cmd
        sda.Fill(dt)
        Return dt
    End Function

End Class
