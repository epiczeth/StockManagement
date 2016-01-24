Imports Microsoft.Reporting.WinForms

Public Class frmReportwithdraw
    Dim obj As New OledbObj
    Private Sub frmReportwithdraw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillCb()
        If Me.WindowState <> FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub
    Private Sub FillCb()
        Dim dt2 As New DataTable
        dt2 = obj.Getdata("SELECT rid, rname from recipient order by rid asc").Tables(0)
        cbrid.DataSource = dt2
        cbrid.DisplayMember = "rname"
        cbrid.ValueMember = "rid"
        cbrid.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cbrid.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub cbrid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbrid.SelectedIndexChanged
        If cbrid.SelectedValue.ToString() <> "System.Data.DataRowView" And cbrid.SelectedIndex <> -1 Then
            Dim dataSource As New ReportDataSource("DataSet1", obj.Getdata(String.Format("SELECT log_acceptedwithdraw.wdid, supplies.spname, log_acceptedwithdraw.wdcount, log_acceptedwithdraw.wreason FROM (log_acceptedwithdraw INNER JOIN supplies ON log_acceptedwithdraw.spid = supplies.spid) where log_acceptedwithdraw.rid={0}", Convert.ToInt32(cbrid.SelectedValue))).Tables(0))

            ReportViewer1.LocalReport.ReportPath = Environment.CurrentDirectory & "\reports\reportWithdraw.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.LocalReport.DataSources.Add(dataSource)
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 25
            ReportViewer1.RefreshReport()
            Me.ReportViewer1.RefreshReport()

        End If
        
    End Sub
End Class