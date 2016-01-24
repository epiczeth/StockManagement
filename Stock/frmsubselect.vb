Public Class Frmsubselect
    Private ReadOnly _objole As New OledbObj
    Private Sub frmsubselect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dt As DataTable = _objole.Getdata("SELECT spid, spname from supplies order by spid asc").Tables(0)
        ComboBox1.DataSource = dt
        ComboBox2.DataSource = dt

        ComboBox1.DisplayMember = "spid"
        ComboBox1.ValueMember = "spid"
        ComboBox2.DisplayMember = "spname"
        ComboBox2.ValueMember = "spid"

        ComboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        ComboBox1.AutoCompleteSource = AutoCompleteSource.ListItems

        ComboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        ComboBox2.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmAddSupply.sid = ComboBox1.SelectedValue.ToString()
        frmAddSupply.sname = ComboBox2.Text
        Close()
    End Sub
End Class