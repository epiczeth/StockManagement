Public Class frmManagestaff
    Dim obj As New OledbObj

    Private Sub frmManagestaff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        filldgv()
        fillCb()
    End Sub
    Private Sub filldgv()
        Dim ds As New DataSet
        ds = obj.Getdata("SELECT staff.sid, staff.sname, staff.stel, staff.smail, roles.role_name, staff.saddress,staff.user_id,staff.user_pwd FROM (staff INNER JOIN roles ON staff.role_id = roles.role_id) ORDER BY staff.sid")
        dgv.DataSource = ds.Tables(0)
        Dim header() As String
        header = New String() {"รหัสบุคลากร", "ชื่อ-สกุล", "เบอร์โทร", "อีเมล์", "ตำแหน่ง", "ที่อยู่", "ชื่อผู้ใช้", "รหัสผ่าน"}
        For i As Integer = 0 To dgv.ColumnCount - 1
            dgv.Columns(i).HeaderText = header(i)
            If i <> 0 Then
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            End If
        Next
    End Sub
    Private Sub  fillCb()
        Dim ds As New DataSet
        ds = obj.Getdata("select * from roles order by role_id asc")
        cbrole.DataSource = ds.Tables(0)
        cbrole.DisplayMember = "role_name"
        cbrole.ValueMember = "role_id"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If dgv.SelectedRows.Count <> 0 Then
            txtsid.Text = dgv.SelectedRows(0).Cells(0).Value.ToString()
            txtsname.Text = dgv.SelectedRows(0).Cells(1).Value.ToString()
            txttel.Text = dgv.SelectedRows(0).Cells(2).Value.ToString()
            txtmail.Text = dgv.SelectedRows(0).Cells(3).Value.ToString()
            cbrole.Text = dgv.SelectedRows(0).Cells(4).Value.ToString()
            txtaddress.Text = dgv.SelectedRows(0).Cells(5).Value.ToString()
            txtid.Text = dgv.SelectedRows(0).Cells(6).Value.ToString()
            txtpass.Text = dgv.SelectedRows(0).Cells(7).Value.ToString()

            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = True
        End If
    End Sub
    Private Function validation() As Boolean
        If txtsid.Text <> "" And txtaddress.Text <> "" And txtmail.Text <> "" And txtsname.Text <> "" And txttel.Text <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub clear()
        txtsid.Text = ""
        txtaddress.Text = ""
        txtmail.Text = ""
        txtsname.Text = ""
        txttel.Text = ""
        txtaddress.Text = ""
    End Sub
    Private Sub CreateNewID()
        Dim ds As New DataSet
        ds = obj.Getdata("select max(sid)+1 from staff")
        If ds.Tables(0).Rows.Count >= 1 Then
            txtsid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtsid.Text = "50001"
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If validation() Then
            Dim ds As New DataSet
            ds = obj.Getdata(String.Format("select * from staff where sid={0}", Convert.ToInt32(txtsid.Text)))
            If ds.Tables(0).Rows.Count <= 0 Then
                obj.Query(String.Format("insert into staff values({0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}')", Convert.ToInt32(txtsid.Text), txtsname.Text, txttel.Text, txtmail.Text, Convert.ToInt32(cbrole.SelectedValue), txtaddress.Text, txtid.Text, txtpass.Text))
            Else
                obj.Query(String.Format("update staff set sname='{0}',stel='{1}',smail='{2}',role_id={3},saddress='{4}',user_id='{6}',user_pwd='{7}' where sid={5}", txtsname.Text, txttel.Text, txtmail.Text, Convert.ToInt32(cbrole.SelectedValue), txtaddress.Text, Convert.ToInt32(txtsid.Text), txtid.Text, txtpass.Text))
            End If
            clear()
            fillCb()
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
                obj.Query(String.Format("delete from staff where sid={0}", Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value)))
                MessageBox.Show("ลบข้อมูลสำเร็จ")
                fillCb()
                filldgv()
            End If
        End If
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
End Class