Public Class frmManageRecipient
    Private obj As New OledbObj
    Private Sub frmManageRecipient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        filldgv()
    End Sub
    Private Sub filldgv()
        Dim dt As New DataTable
        dt = obj.Getdata("select * from recipient order by rid asc").Tables(0)
        dgv.DataSource = dt

        Dim hd() As String
        hd = New String() {"รหัสผู้เบิก", "ชื่อ-สกุล", "ที่อยู่", "เบอร์โทร", "อีเมล์"}

        For i As Integer = 0 To dgv.ColumnCount - 1
            dgv.Columns(i).HeaderText = hd(i)
            If i <> 0 Then
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            End If
        Next




    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If dgv.SelectedRows.Count >= 1 Then
            txtrid.Text = dgv.SelectedRows(0).Cells(0).Value.ToString()
            txtname.Text = dgv.SelectedRows(0).Cells(1).Value.ToString()
            txtaddress.Text = dgv.SelectedRows(0).Cells(2).Value.ToString()
            txttel.Text = dgv.SelectedRows(0).Cells(3).Value.ToString()
            txtmail.Text = dgv.SelectedRows(0).Cells(4).Value.ToString()

            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = True
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim ds As New DataSet
        ds = obj.Getdata(String.Format("select * from recipient where rid={0}", Convert.ToInt32(txtrid.Text)))
        If validation() Then
            If ds.Tables(0).Rows.Count <= 0 Then
                obj.Query(String.Format("insert into recipient values({0},'{1}','{2}','{3}','{4}')", Convert.ToInt32(txtrid.Text), txtname.Text, txtaddress.Text, txttel.Text, txtmail.Text))
            Else
                obj.Query(String.Format("update recipient set rname='{0}',raddress='{1}',rtel='{2}',remail='{3}' where rid={4}", txtname.Text, txtaddress.Text, txttel.Text, txtmail.Text, Convert.ToInt32(txtrid.Text)))
            End If
            MessageBox.Show("บันทึกข้อมูลสำเร็จ")
            filldgv()
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = False
            clear()
        Else
            MessageBox.Show(Me, "กรุณากรอกข้อมูลให้ครบ", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If

    End Sub
    Private Sub CreateNewRid()
        Dim ds As New DataSet
        ds = obj.Getdata("select max(rid)+1 from recipient")
        If ds.Tables(0).Rows.Count > 0 Then
            txtrid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtrid.Text = "1"
        End If
    End Sub
    Private Sub clear()
        txtrid.Text = ""
        txtname.Text = ""
        txtaddress.Text = ""
        txtmail.Text = ""
        txttel.Text = ""
    End Sub
    Private Function validation() As Boolean
        If txtrid.Text <> "" And txtname.Text <> "" And txtaddress.Text <> "" And txttel.Text <> "" And txtmail.Text <> "" Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateNewRid()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If dgv.SelectedRows.Count <> 0 Then
            If MessageBox.Show(Me, "คุณต้องการลบ '" & dgv.SelectedRows(0).Cells(1).Value.ToString() & "' ออกจากฐานข้อมูลใช่หรือไม่?", "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                obj.Query(String.Format("delete from recipient where rid={0}", Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value)))
                MessageBox.Show("ลบสำเร็จ")
                filldgv()

            End If
        End If
    End Sub
End Class