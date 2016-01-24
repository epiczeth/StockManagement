Public Class frmLogin
    Dim objConn As New OledbObj

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dt, dt2 As New DataTable
        dt =
            objConn.Getdata(
                "SELECT sid FROM staff WHERE user_id='" & TextBox1.Text.Trim() & "' AND user_pwd='" & TextBox2.Text.Trim() &
                "'").Tables(0)
        If dt.Rows.Count >= 1 Then
            OledbObj.currentSid = dt.Rows(0)(0).ToString()
            Dim fm As New frmMain
            fm.Show()
            Me.Hide()
        Else
            MessageBox.Show(Me, "ชื่อผู้ใช้หรือรหัสผ่านผิด", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            TextBox2.Text = ""
            TextBox2.Focus()
        End If
    End Sub
End Class
