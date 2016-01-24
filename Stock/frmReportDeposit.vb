Imports Microsoft.Reporting.WinForms
Public Class frmReportDeposit

    Private Sub frmReportDeposit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim obj As New OledbObj
        Dim dataSource As New ReportDataSource("DataSet1", obj.Getdata("SELECT log_deposit.deid, supplies.spname, log_deposit.depriceperunit, log_deposit.decount, log_deposit.dedate FROM (log_deposit INNER JOIN supplies ON log_deposit.spid = supplies.spid) order by log_deposit.deid asc").Tables(0))

        ReportViewer1.LocalReport.ReportPath = Environment.CurrentDirectory & "\reports\repDeposit.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(dataSource)
        ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
        ReportViewer1.ZoomMode = ZoomMode.Percent
        ReportViewer1.ZoomPercent = 25
        ReportViewer1.RefreshReport()
        Me.ReportViewer1.RefreshReport()
        If Me.WindowState <> FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub
End Class