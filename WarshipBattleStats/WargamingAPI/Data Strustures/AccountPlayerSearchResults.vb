
Namespace Data.Account

    Public Class PlayerSearchResults
        Public Property status As String
        Public Property meta As Data.Common.Meta
        Public Property [Error] As Data.Common.Error
        Public Property data As List(Of FoundPlayer)
    End Class

    Public Class FoundPlayer
        Public Property nickname As String
        Public Property account_id As Integer
    End Class

End Namespace
