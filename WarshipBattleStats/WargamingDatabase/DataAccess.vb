
Imports System.Data.Common
Imports System.Threading
Imports EntLib = Microsoft.Practices.EnterpriseLibrary.Data

Public MustInherit Class DataAccess

#Region "Declares..."

#End Region

#Region "New..."

#End Region

#Region "Methods..."


    Protected Shared Function GetValue(Of t)(reader As IDataReader, field As Int32, defaultValue As Object) As t

        If reader.IsDBNull(field) Then
            Return CType(defaultValue, t)
        Else
            Return CType(reader.GetValue(field), t)
        End If

    End Function

    Protected Shared Function GetEnum(ByVal reader As IDataReader, ByVal field As Int32) As Int32

        If reader.IsDBNull(field) Then
            Return 0
        Else
            Return CInt(reader.GetValue(field))
        End If

    End Function

    Protected Shared Function GetDate(ByVal reader As IDataReader, ByVal field As Int32) As DateTime

        If reader.IsDBNull(field) Then
            Return Nothing
        Else
            Return reader.GetDateTime(field)
        End If

    End Function

    Protected Shared Function GetDouble(ByVal reader As IDataReader, ByVal field As Int32) As Double

        If reader.IsDBNull(field) Then
            Return Double.MinValue
        Else
            Return reader.GetDouble(field)
        End If

    End Function

    Protected Shared Function GetDecimal(ByVal reader As IDataReader, ByVal field As Int32) As Decimal

        If reader.IsDBNull(field) Then
            Return Decimal.MinValue
        Else
            Return reader.GetDecimal(field)
        End If

    End Function

    Protected Shared Function GetByte(ByVal reader As IDataReader, ByVal field As Int32) As Byte

        If reader.IsDBNull(field) Then
            Return Byte.MinValue
        Else
            Return reader.GetByte(field)
        End If

    End Function

    Protected Shared Function GetBytes(reader As IDataReader, field As Int32) As Byte()

        Dim Bytes(249) As Byte
        Dim Read As Int32 = 0
        Dim Total As Int32 = 0

        Do
            Read = reader.GetBytes(field, Bytes.Length - 250, Bytes, Bytes.Length - 250, 250)
            If Read = 0 Then
                Exit Do
            End If
            Total += Read

            ReDim Preserve Bytes(Bytes.Length + 249)

        Loop

        ReDim Preserve Bytes(Total - 1)

        Return Bytes

    End Function

    Protected Shared Function GetBool(ByVal reader As IDataReader, ByVal field As Int32) As Boolean

        If reader.IsDBNull(field) Then
            Return False
        Else
            Return CBool(reader.GetValue(field))
        End If

    End Function

    Protected Shared Function GetString(ByVal reader As IDataReader, ByVal field As Int32) As String

        If reader.IsDBNull(field) Then
            Return ""
        Else
            Return reader.GetString(field)
        End If

    End Function

    Protected Shared Function GetInt64(ByVal reader As IDataReader, ByVal field As Int32) As Int64

        If reader.IsDBNull(field) Then
            Return Int64.MinValue
        Else
            Return reader.GetInt64(field)
        End If

    End Function

    Protected Shared Function GetInt32(ByVal reader As IDataReader, ByVal field As Int32) As Int32

        If reader.IsDBNull(field) Then
            Return Int32.MinValue
        Else
            Return reader.GetInt32(field)
        End If

    End Function

    Protected Shared Function GetInt16(ByVal reader As IDataReader, ByVal field As Int32) As Int16

        If reader.IsDBNull(field) Then
            Return Int16.MinValue
        Else
            Return reader.GetInt16(field)
        End If

    End Function

    Protected Shared Function GetIntByte(ByVal reader As IDataReader, ByVal field As Int32) As Byte

        If reader.IsDBNull(field) Then
            Return Byte.MinValue
        Else
            Return reader.GetByte(field)
        End If

    End Function

    Protected Shared Function DateToSqlDate(ByVal rDate As DateTime) As String

        Return String.Format("{0:0000}-{1:00}-{2:00}", rDate.Year, rDate.Month, rDate.Day)

    End Function

    Protected Shared Function TimeToSqlTime(ByVal time As DateTime) As String

        Return TimeToSqlTime(time.TimeOfDay)

    End Function

    Protected Shared Function TimeToSqlTime(ByVal time As TimeSpan) As String

        Dim tmp As String = String.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds)

        If time < TimeSpan.Zero Then
            Return "-" & tmp.Replace("-", "")
        Else
            Return tmp
        End If

    End Function

    Protected Shared Function StringToSqlString(ByVal s As String) As String

        Return s.Replace("'", "''")

    End Function

    Protected Shared Function CreateDatabase(ByVal dataBase As String) As Microsoft.Practices.EnterpriseLibrary.Data.Database

        Return EntLib.DatabaseFactory.CreateDatabase(dataBase)

    End Function

#End Region

#Region "Properties..."


#End Region

End Class
