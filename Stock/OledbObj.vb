Imports System.Data.OleDb

Public Class OledbObj
    Public _
        Con As _
            New OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\Databases\StockDB.accdb",
                                              Environment.CurrentDirectory))
    Public Shared CurrentSid As String
    Public Function Getdata(ByVal sql As String) As DataSet
        Dim ds As New DataSet()
        ' Try
        If Con.State <> ConnectionState.Closed Then
            Con.Close()
        End If
        Con.Open()

        Dim da As New OleDbDataAdapter(sql, Con)
        ds.Clear()
        da.Fill(ds)
        ' Catch ex As Exception
        '  Debug.WriteLine(ex.Message)
        '  End Try
        Return ds
    End Function

    Public Function Query(ByVal sql As String) As Integer
        Dim i As Integer
        ' Try
        If Con.State <> ConnectionState.Closed Then
            Con.Close()
        End If
        Con.Open()
        Dim com As New OleDbCommand()
        com.CommandType = CommandType.Text
        com.CommandText = sql
        com.Connection = Con
        i = com.ExecuteNonQuery()
        ' Catch ex As Exception
        'Debug.WriteLine(ex.Message)
        'End Try
        Return i
    End Function
End Class
