Public Class frmManagVendor
    Dim obj As New OledbObj
    Private Sub frmManagVendor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        filldgv()
    End Sub
    Private Sub filldgv()
        Dim ds As New DataSet
        ds = obj.Getdata("SELECT * FROM vendor order by veid asc")
        dgv.DataSource = ds.Tables(0)
        Dim header() As String
        header = New String() {"รหัสร้านค้า", "ชื่อ", "ที่อยู่", "เบอร์โทร", "อีเมล์"}
        For i As Integer = 0 To dgv.ColumnCount - 1
            dgv.Columns(i).HeaderText = header(i)
            If i <> 0 Then
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If dgv.SelectedRows.Count <> 0 Then
            txtid.Text = dgv.SelectedRows(0).Cells(0).Value.ToString()
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
    Private Sub clear()
        txtaddress.Text = ""
        txtid.Text = ""
        txtmail.Text = ""
        txtname.Text = ""
        txttel.Text = ""
    End Sub
    Private Sub CreateNewID()
        Dim ds As New DataSet
        ds = obj.Getdata("select max(veid)+1 from vendor")
        If ds.Tables(0).Rows.Count >= 1 Then
            txtid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtid.Text = "201"
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        CreateNewID()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If dgv.SelectedRows.Count <> 0 Then
            If MessageBox.Show(Me, String.Format("คุณต้องการลบ '{0}' ออกจากฐานข้อมูลใช่หรือไม่?", dgv.SelectedRows(0).Cells(1).Value.ToString()), "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                obj.Query(String.Format("delete from vendor where veid={0}", Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value)))
                filldgv()
                MessageBox.Show("ลบข้อมูลสำเร็จ")
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If validation() Then
            Dim ds As New DataSet
            ds = obj.Getdata(String.Format("select * from vendor where veid={0}", Convert.ToInt32(txtid.Text)))
            If ds.Tables(0).Rows.Count <= 0 Then
                obj.Query(String.Format("insert into vendor values({0},'{1}','{2}','{3}',{4})", Convert.ToInt32(txtid.Text), txtname.Text, txtaddress.Text, txttel.Text, txtmail.Text))
            Else
                obj.Query(String.Format("update vendor set vename='{0}',veaddress='{1}',vetel='{2}',veemail='{3}' where veid={4}", txtname.Text, txtaddress.Text, txttel.Text, txtmail.Text, Convert.ToInt32(txtid.Text)))
            End If
            clear()
            filldgv()
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = False
            MessageBox.Show("บันทึกเรียบร้อย")
        Else
            MessageBox.Show(Me, "กรุณากรอกข้อมูลให้ครบ", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If
    End Sub
    Private Function validation() As Boolean
        If txtaddress.Text <> "" And txtid.Text <> "" And txtmail.Text <> "" And txtname.Text <> "" And txttel.Text <> "" Then
            Return True
        Else
            Return False
        End If

    End Function
End Class