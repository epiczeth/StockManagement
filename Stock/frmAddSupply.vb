Public Class frmAddSupply
    Private ole As New OledbObj
    Public Shared sid, sname As String
    Private Sub frmAddSupply_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CreateNewLogId()
        fill_cb()
    End Sub
    Private Sub sum()
        If nudcount.Value >= 1 And nudpriceperunit.Value >= 1 Then
            txtsum.Text = (nudpriceperunit.Value * nudcount.Value).ToString()
        End If
    End Sub

    Private Sub fill_cb()
        Dim dt As New DataTable
        dt = ole.Getdata("SELECT veid, vename from vendor order by veid asc").Tables(0)
        cbvendor.DataSource = dt


        cbvendor.DisplayMember = "vename"
        cbvendor.ValueMember = "veid"

        'cbvendor.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'cbvendor.AutoCompleteSource = AutoCompleteSource.ListItems

    End Sub
    Private Sub CreateNewLogId()
        ' ReSharper disable once RedundantAssignment
        Dim ds As New DataSet()
        ds = ole.Getdata("select MAX(deid)+1 from log_deposit")

        If ds.Tables(0).Rows(0)(0).ToString() <> "" Then
            txtlogid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtlogid.Text = "8001"
        End If
    End Sub
    Private Sub CreateNewSupplyId()
        ' ReSharper disable once RedundantAssignment
        Dim ds As New DataSet()
        ds = ole.Getdata("select MAX(spid)+1 from supplies")

        If ds.Tables(0).Rows(0)(0).ToString() <> "" Then
            txtsid.Text = ds.Tables(0).Rows(0)(0).ToString()
        Else
            txtsid.Text = "70002"
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        txtsid.Text = ""
        txtsdetail.Text = ""
        txtsname.Text = ""
        CreateNewSupplyId()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fss As New frmsubselect
        fss.ShowDialog()
        If sid <> "" And sname <> "" Then
            txtsid.Text = sid
            txtsname.Text = sname
        End If
    End Sub

    Private Sub nudpriceperunit_ValueChanged(sender As Object, e As EventArgs) Handles nudpriceperunit.ValueChanged
        sum()
    End Sub

    Private Sub nudcount_ValueChanged(sender As Object, e As EventArgs) Handles nudcount.ValueChanged
        sum()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If validation() Then
            Dim dt As DataTable
            ole.Query(String.Format("insert into log_deposit values({0},{1},{2},{3},'{4}',{5},{6})", Convert.ToInt32(txtlogid.Text), Convert.ToInt32(OledbObj.currentSid), Convert.ToInt32(txtsid.Text), Convert.ToInt32(nudpriceperunit.Value), DateTimePicker1.Value, Convert.ToInt32(nudcount.Value), Convert.ToInt32(cbvendor.SelectedValue)))
            dt = ole.Getdata(String.Format("select * from supplies where spid={0}", Convert.ToInt32(txtsid.Text))).Tables(0)
            If dt.Rows.Count >= 1 Then
                Dim ds As New DataSet
                ds = ole.Getdata("select spremain from supplies where spid=" & Convert.ToInt32(txtsid.Text) & "")
                Dim intnew As Integer = Convert.ToInt32(ds.Tables(0).Rows(0)(0).ToString()) + Convert.ToInt32(nudcount.Value)
                ole.Query(String.Format("update supplies set spremain={0} where spid={1}", intnew, Convert.ToInt32(txtsid.Text)))
            Else
                ole.Query(String.Format("insert into supplies values({0},'{1}','{2}',{3})", Convert.ToInt32(txtsid.Text), txtsname.Text, txtsdetail.Text, Convert.ToInt32(nudcount.Value)))
            End If
            MessageBox.Show("บันทึกข้อมูลแล้ว")
            txtsid.Text = ""
            txtsdetail.Text = ""
            txtsname.Text = ""
            nudcount.Value = 0
            nudpriceperunit.Value = 0
            txtsum.Text = ""
            CreateNewLogId()
        Else
            MessageBox.Show("กรุณากรอกข้อมูลให้ครบก่อนดำเนินการต่อ")
        End If
    End Sub
    ' ReSharper disable once InconsistentNaming
    Private Function validation() As Boolean
        If txtsid.Text <> "" And txtlogid.Text <> "" And txtsum.Text <> "" And nudcount.Value <> 0 And nudpriceperunit.Value <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub
End Class