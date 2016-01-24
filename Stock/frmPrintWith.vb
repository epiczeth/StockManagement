Imports Microsoft.Reporting.WinForms

Public Class frmPrintWith
    Dim obj As New OledbObj
    Public Shared rname, rid As String
    Private Sub frmPrintWith_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim dataSource As New ReportDataSource("DataSet1", obj.Getdata("SELECT log_pendingwithdraw.wdid, supplies.spname, log_pendingwithdraw.wdcount, log_pendingwithdraw.wreason FROM (log_pendingwithdraw INNER JOIN supplies ON log_pendingwithdraw.spid = supplies.spid) where log_pendingwithdraw.rid=" & Convert.ToInt32(rid) & "").Tables(0))

            Dim param As ReportParameter
            param = New ReportParameter("pName", rname)
            ReportViewer1.LocalReport.ReportPath = Environment.CurrentDirectory & "\reports\repNewWithdraw.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.LocalReport.DataSources.Add(dataSource)
            ReportViewer1.LocalReport.SetParameters(param)
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 25
            ReportViewer1.RefreshReport()
            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
     

    End Sub
End Class