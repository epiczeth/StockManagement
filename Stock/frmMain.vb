Public Class frmMain
    ReadOnly _objConn As New OledbObj
    Private Sub fToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles fToolStripMenuItem.Click

    End Sub

    Private Sub CloseSub()
        For Each fm As Form In MdiChildren
            fm.Close()
        Next fm
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        CloseSub()
        Dim fa As New frmAddSupply
        fa.MdiParent = Me

        fa.Show()
    End Sub

    Private Sub แกไขToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles แกไขToolStripMenuItem.Click
        CloseSub()
        Dim fms As New frmManageSupplies
        fms.MdiParent = Me
        fms.Show()
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem6.Click
        CloseSub()
        Dim frm As New frmManageRecipient
        frm.MdiParent = Me

        frm.Show()
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        CloseSub()
        Dim frm As New frmManagestaff
        frm.MdiParent = Me

        frm.Show()
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        CloseSub()
        Dim frm As New frmManagVendor
        frm.MdiParent = Me

        frm.Show()
    End Sub
    Dim _ctrl As MdiClient
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dt As New DataTable
        dt = _objConn.Getdata(String.Format("SELECT roles.role_name FROM (staff INNER JOIN roles ON staff.role_id = roles.role_id) WHERE (staff.sid ={0})", Convert.ToInt32(OledbObj.currentSid))).Tables(0)
        If dt.Rows.Count <> 0 Then
            lblstatus.Text = dt.Rows(0)(0).ToString()
        End If

        If lblstatus.Text = "หัวหน้าด่านตรวจสัตว์น้ำ" Then
            lblcacwait.Visible = True
            lblacwait.Visible = True
            lblcwait.Visible = True
            lblwait.Visible = True
            AcceptToolStripMenuItem.Visible = True
        Else
            lblcacwait.Visible = False
            lblacwait.Visible = False
            lblcwait.Visible = False
            lblwait.Visible = False
            AcceptToolStripMenuItem.Visible = False
        End If


        For Each o As Control In Controls
            Try
                _ctrl = DirectCast(o, MdiClient)
                _ctrl.BackColor = BackColor
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub WithdrawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithdrawToolStripMenuItem.Click
        CloseSub()
        Dim frm As New frmWithdraw
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub DepositToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DepositToolStripMenuItem.Click
        CloseSub()
        Dim frm As New frmReportDeposit
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub WithdrawToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles WithdrawToolStripMenuItem1.Click
        CloseSub()
        Dim frm As New frmReportwithdraw
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub bkData_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bkData.DoWork
        Dim ds, ds2 As New DataSet
        ds = _objConn.Getdata("select count(wdid) from log_pendingwithdraw")
        ds2 = _objConn.Getdata("select count(wdid) from log_acceptedwithdraw")
        lblwait.Text = ds.Tables(0).Rows(0)(0).ToString()
        lblacwait.Text = ds2.Tables(0).Rows(0)(0).ToString()
    End Sub

    Private Sub AcceptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcceptToolStripMenuItem.Click
        CloseSub()
        Dim frm As New acceptwork
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Application.Restart()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If bkData.IsBusy = False Then
            bkData.RunWorkerAsync()
        End If

    End Sub
End Class