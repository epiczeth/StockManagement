Public Class frmWithdraw
    Private ole As New OledbObj
    Private Sub frmWithdraw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillcb()
        filldgv()
    End Sub
    Private Sub fillcb()
        Dim dt, dt2 As New DataTable
        dt = ole.Getdata("SELECT spid, spname from supplies where (spremain > 0) order by spid asc").Tables(0)
        cbsupplie.DataSource = dt
        cbsupplie.DisplayMember = "spname"
        cbsupplie.ValueMember = "spid"
        cbsupplie.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cbsupplie.AutoCompleteSource = AutoCompleteSource.ListItems


        dt2 = ole.Getdata("SELECT rid, rname from recipient order by rid asc").Tables(0)
        cbrecipient.DataSource = dt2
        cbrecipient.DisplayMember = "rname"
        cbrecipient.ValueMember = "rid"
        cbrecipient.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cbrecipient.AutoCompleteSource = AutoCompleteSource.ListItems

    End Sub
    Private Sub CreateNewID()
        Dim ds As New DataSet
        ds = ole.Getdata("select max(wdid)+1 from log_pendingwithdraw")
        If ds.Tables(0).Rows.Count >= 1 Then
            txtid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtid.Text = "301"
        End If

    End Sub
    Private Sub  getMaxCount()
        If cbsupplie.SelectedIndex >= 0 And cbsupplie.SelectedValue.ToString() <> "System.Data.DataRowView" Then
            Dim ds As New DataSet
            ds = ole.Getdata(String.Format("select spremain from supplies where spid={0}", Convert.ToInt32(cbsupplie.SelectedValue)))
            If ds.Tables(0).Rows.Count >= 1 Then
                nudCount.Maximum = Convert.ToInt32(ds.Tables(0).Rows(0)(0))
            End If
        End If
    End Sub

    Private Sub cbsupplie_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbsupplie.SelectedValueChanged
        getMaxCount()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateNewID()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = True
    End Sub
    Private Sub filldgv()
        Dim ds As New DataSet
        ds = ole.Getdata("SELECT log_pendingwithdraw.wdid, supplies.spname, recipient.rname, log_pendingwithdraw.wdcount, log_pendingwithdraw.wddate, staff.sname,log_pendingwithdraw.wreason FROM (((log_pendingwithdraw INNER JOIN supplies ON log_pendingwithdraw.spid = supplies.spid) INNER JOIN recipient ON log_pendingwithdraw.rid = recipient.rid) INNER JOIN staff ON log_pendingwithdraw.sid = staff.sid) order by log_pendingwithdraw.wdid asc")
        dgv.DataSource = ds.Tables(0)
        Dim header() As String
        header = New String() {"รหัสการเบิก", "พัสดุ", "ผู้เบิก", "จำนวน", "วันที่เบิก", "ผ้จัดทำ", "หมายเหตุ"}
        For i As Integer = 0 To dgv.ColumnCount - 1
            dgv.Columns(i).HeaderText = header(i)
            If i <> 0 Then
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            End If


        Next

    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If validation() Then
            Dim ds As New DataSet
            ds = ole.Getdata(String.Format("select * from log_pendingwithdraw where wdid={0}", Convert.ToInt32(txtid.Text)))
            If ds.Tables(0).Rows.Count <= 0 Then
                If ole.Query(String.Format("insert into log_pendingwithdraw values({0},{1},{2},{3},'{4}',{5},{6})", Convert.ToInt32(txtid.Text), Convert.ToInt32(cbsupplie.SelectedValue), Convert.ToInt32(cbrecipient.SelectedValue), Convert.ToInt32(nudCount.Value), dtp.Value, Convert.ToInt32(OledbObj.currentSid), txtreason.Text)) = 1 Then
                    ole.Query(String.Format("update supplies set spremain={0} where spid={1}", Convert.ToInt32(nudCount.Maximum - nudCount.Value), Convert.ToInt32(cbsupplie.SelectedValue)))
                End If
            Else
                ole.Query(String.Format("update log_pendingwithdraw set spid={0},rid={1},wdcount={2},wddate='{3}',wreason='{4}' where wdid={5}", Convert.ToInt32(cbsupplie.SelectedValue), Convert.ToInt32(cbrecipient.SelectedValue), Convert.ToInt32(nudCount.Value), dtp.Value, txtreason.Text, Convert.ToInt32(txtid.Text)))
            End If
            MessageBox.Show(Me, "บันทีกข้อมูลสำเร็จ", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            fillcb()
            filldgv()
            clear()
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = False

        Else
            MessageBox.Show("กรุณากรอกข้อมูลให้ครบ")
        End If
    End Sub
    Private Function validation() As Boolean
        If txtid.Text <> "" And nudCount.Value >= 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If dgv.SelectedRows.Count <> 0 Then
            txtid.Text = dgv.SelectedRows(0).Cells(0).Value.ToString()
            cbsupplie.Text = dgv.SelectedRows(0).Cells(1).Value.ToString()
            cbrecipient.Text = dgv.SelectedRows(0).Cells(2).Value.ToString()
            nudCount.Value = Convert.ToDecimal(dgv.SelectedRows(0).Cells(3).Value)
            dtp.Value = Convert.ToDateTime(dgv.SelectedRows(0).Cells(4).Value)
            txtreason.Text = dgv.SelectedRows(0).Cells(6).Value.ToString()
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = True
        End If
    End Sub
    Private Sub clear()
        txtid.Text = ""
        nudCount.Value = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If dgv.SelectedRows.Count <> 0 Then
            If MessageBox.Show(Me, "คุณต้องการลบ '" & dgv.SelectedRows(0).Cells(1).Value.ToString() & "' ออกจากฐานข้อมูลใช่หรือไม่?", "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ole.Query(String.Format("delete from log_pendingwithdraw where wdid={0}", Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value)))
                filldgv()
                fillcb()
                clear()
                MessageBox.Show("ลบข้อมูลสำเร็จ")
            End If
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If dgv.SelectedRows(0).Cells(0).Value.ToString() <> "" Then
            frmPrintWith.rname = dgv.SelectedRows(0).Cells(2).Value.ToString()
            Dim ds As New DataSet
            ds = ole.Getdata("select rid from log_pendingwithdraw where wdid=" & Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value) & "")
            frmPrintWith.rid = ds.Tables(0).Rows(0)(0).ToString()
            Dim frm As New frmPrintWith
            frm.ShowDialog(Me)
        End If
    End Sub
End Class