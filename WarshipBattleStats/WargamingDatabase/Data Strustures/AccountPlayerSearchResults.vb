
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
        Public Shadows Function ToString(server As Data.Common.ServerRealm) As String

            Return String.Format("{0}    ~    [{1}]    ~    {2}", Me.account_id, server, Me.nickname)

        End Function

    End Class

    Public Class PlayerAchievements
        Public Property status As String
        Public Property meta As Meta
        Public Property earned As New List(Of AchievementData)
        Public Property earning As New List(Of AchievementData)
    End Class

    Public Class Meta
        Public Property count As Integer
        Public Property hidden As Object
    End Class

    Public Class AchievementData

        Public Sub New(s As String)
            Dim x() As String = s.Replace("""", "").Split(":")
            Me.AchievmentID = x(0)
            Me.Count = CInt(x(1))
        End Sub

        Public Property AchievmentID As String
        Public Property Count As Integer

    End Class

End Namespace
