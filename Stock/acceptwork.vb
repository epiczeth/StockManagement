Public Class acceptwork
    Private obj As New OledbObj
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
    Private Sub FillLB()
        Dim ds As New DataSet
        ds = obj.Getdata("select wdid from log_pendingwithdraw order by wdid asc")
        ListBox1.DataSource = ds.Tables(0)
        ListBox1.DisplayMember = "wdid"
        ListBox1.ValueMember = "wdid"
    End Sub

    Private Sub acceptwork_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillLB()
    End Sub

    Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged
        Try
            If ListBox1.SelectedValue.ToString() <> "System.Data.DataRowView" And ListBox1.SelectedValue.ToString() <> "" Then
                Dim ds As New DataSet
                ds = obj.Getdata("SELECT log_pendingwithdraw.wdid, supplies.spname, recipient.rname, log_pendingwithdraw.wdcount, log_pendingwithdraw.wddate, staff.sname, log_pendingwithdraw.wreason FROM (((log_pendingwithdraw INNER JOIN recipient ON log_pendingwithdraw.rid = recipient.rid) INNER JOIN staff ON log_pendingwithdraw.sid = staff.sid) INNER JOIN supplies ON log_pendingwithdraw.spid = supplies.spid) WHERE log_pendingwithdraw.wdid=" & Convert.ToInt32(ListBox1.SelectedValue) & "")
                txtid.Text = ds.Tables(0).Rows(0)(0).ToString()
                txtspname.Text = ds.Tables(0).Rows(0)(1).ToString()
                txtrid.Text = ds.Tables(0).Rows(0)(2).ToString()
                txtcuount.Text = ds.Tables(0).Rows(0)(3).ToString()
                dtp.Value = Convert.ToDateTime(ds.Tables(0).Rows(0)(4))
                txtsid.Text = ds.Tables(0).Rows(0)(5).ToString()
                txtreason.Text = ds.Tables(0).Rows(0)(6).ToString()

            End If
        Catch ex As Exception

        End Try


        

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txtid.Text <> "" Then
            Dim ds As New DataSet
            ds = obj.Getdata(String.Format("select * from log_pendingwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
            If ds.Tables(0).Rows.Count = 1 Then
                If MessageBox.Show("คุณต้องการยกเลิกคำร้องข้อที่ '" & txtid.Text & "' ใช่หรือไม่?", "ลบคำขอ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    obj.Query(String.Format("delete from log_pendingwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
                    FillLB()
                    clear()
                End If
            End If

        End If
    End Sub
    Private Sub clear()
        txtcuount.Text = ""
        txtid.Text = ""
        txtreason.Text = ""
        txtrid.Text = ""
        txtsid.Text = ""
        txtspname.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtid.Text <> "" Then
            Dim ds, ds2 As New DataSet
            ds = obj.Getdata(String.Format("select * from log_pendingwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
            ds2 = obj.Getdata(String.Format("select * from log_acceptedwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
            If ds.Tables(0).Rows.Count = 1 Then
                If MessageBox.Show("คุณต้องการอนุมัติคำร้องข้อที่ '" & txtid.Text & "' ใช่หรือไม่?", "อนุมัติคำขอ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    If ds2.Tables(0).Rows.Count <= 0 Then

                        Dim str As String
                        str = "insert into log_acceptedwithdraw values("
                        str &= "" & Convert.ToInt32(ds.Tables(0).Rows(0)(0)) & ","
                        str &= "" & Convert.ToInt32(ds.Tables(0).Rows(0)(1)) & ","
                        str &= "" & Convert.ToInt32(ds.Tables(0).Rows(0)(2)) & ","
                        str &= "" & Convert.ToInt32(ds.Tables(0).Rows(0)(3)) & ","
                        str &= "'" & Convert.ToDateTime(ds.Tables(0).Rows(0)(4)) & "',"
                        str &= "" & Convert.ToInt32(ds.Tables(0).Rows(0)(5)) & ","
                        str &= "'" & ds.Tables(0).Rows(0)(6).ToString() & "')"
                        obj.Query(str)
                        obj.Query(String.Format("delete from log_pendingwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
                        FillLB()
                        clear()
                    End If

                End If
            End If
        End If
       

    End Sub
End Class