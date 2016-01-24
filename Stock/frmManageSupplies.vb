Public Class frmManageSupplies
    Private ReadOnly _ole As New OledbObj

    Private Sub frmManageSupplies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Fill_dgv()
    End Sub

    Private Sub Fill_dgv()
        Dim dt As New DataTable
        dt = _ole.Getdata("SELECT * FROM supplies ORDER BY spid ASC").Tables(0)
        dgv.DataSource = dt
        Dim header() As String = New String() {"รหัสพัสดุ", "ชื่อพัสดุ", "รายละเอียด", "จำนวนคงคลัง"}
        For i As Integer = 0 To dgv.ColumnCount - 1
            dgv.Columns(i).HeaderText = header(i)
        Next
        For i As Integer = 0 To dgv.ColumnCount - 1
            If Not i = 0 And Not i = 3 Then
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                dgv.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            End If

        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If txtsupid.Text = "" Then
            btsave.Enabled = False

            btdel.Enabled = True
            btedit.Enabled = True
        Else
            btsave.Enabled = True

            btdel.Enabled = False
            btedit.Enabled = False
        End If
    End Sub

    Private Sub btedit_Click(sender As Object, e As EventArgs) Handles btedit.Click
        txtsupid.Text = dgv.SelectedRows(0).Cells(0).Value.ToString()
        txtname.Text = dgv.SelectedRows(0).Cells(1).Value.ToString()
        txtcaption.Text = dgv.SelectedRows(0).Cells(2).Value.ToString()
        nudcount.Value = Convert.ToDecimal(dgv.SelectedRows(0).Cells(3).Value)
    End Sub



    Private Sub btsave_Click(sender As Object, e As EventArgs) Handles btsave.Click
        If validation() Then
            Dim sql As String = ""
            If _
                _ole.Getdata(String.Format("SELECT * FROM supplies WHERE spid={0}", Convert.ToInt32(txtsupid.Text))).
                    Tables(0).Rows.Count <= 0 Then
                sql = String.Format("insert into supplies values({0},'{1}',{2})", Convert.ToInt32(txtsupid.Text),
                                    txtcaption.Text, Convert.ToInt32(nudcount.Value))
                _ole.Query(sql)
                clearcontrol()
                Fill_dgv()
            Else
                sql = String.Format("update supplies set spname='{0}',spdetail='{1}',spremain={2} where spid={3}", txtname.Text, txtcaption.Text,
                                    Convert.ToInt32(nudcount.Value), Convert.ToInt32(txtsupid.Text))
                _ole.Query(sql)
                clearcontrol()
                Fill_dgv()
            End If
        Else
            MessageBox.Show("กรุณากรอกข้อมูลให้ครบ")
            Return
        End If
    End Sub

    Private Function validation() As Boolean
        If txtsupid.Text <> "" And txtsupid.Text <> "" And nudcount.Value <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub clearcontrol()
        txtsupid.Text = ""
        txtcaption.Text = ""
        nudcount.Value = 0
    End Sub

    Private Sub btdel_Click(sender As Object, e As EventArgs) Handles btdel.Click
        If dgv.SelectedRows.Count >= 1 Then
            If _
                MessageBox.Show(Me,
                                "คุณต้องการลบ '" & dgv.SelectedRows(0).Cells(1).Value.ToString() &
                                "' ออกจากฐานข้อมูลใช่หรือไม่?", "ลบข้อมูล", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) <> DialogResult.No Then
                Dim sql As String
                sql = String.Format("delete from supplies  where spid={0}",
                                    Convert.ToInt32(dgv.SelectedRows(0).Cells(0).Value))
                _ole.Query(sql)
                Fill_dgv()
            End If

        End If
    End Sub
End Class