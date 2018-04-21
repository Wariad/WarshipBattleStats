
Namespace Data.Common

    Public Enum ServerRealm
        RU
        EU
        NA
        ASIA
    End Enum

    Public Enum Game
        WOT
        WOWS
        WOWP
    End Enum

    Public Class User
        Public Property Server As ServerRealm
        Public Property Nickname As String
        Public Property Account As String

        Public Sub New()

        End Sub

        Public Sub New(account As String, server As String, nickName As String)

            Me.ServerFromString(server)
            Me.Nickname = nickName
            Me.Account = account

        End Sub

        Public Overrides Function ToString() As String

            Return String.Format("{0}    ~    [{1}]    ~    {2}", Me.Account, Me._Server, Me.Nickname)

        End Function

        Public Function ToShortString() As String

            Return Me.ServerToString & "~" & Me.Nickname

        End Function

        Public Shared Function Parse(s As String) As User

            Dim tmp() As String = s.Replace("    ~    [", "~").Replace("]    ~    ", "~").Split("~")
            Dim svr As Data.Common.ServerRealm = [Enum].Parse(GetType(Data.Common.ServerRealm), tmp(1))
            Return New User(tmp(0), svr, tmp(2))

        End Function

        Public Sub ServerFromString(server As String)

            Me.Server = [Enum].Parse(GetType(Data.Common.ServerRealm), server)

        End Sub

        Public ReadOnly Property ServerToString As String
            Get
                Return [Enum].GetName(GetType(Data.Common.ServerRealm), Me.Server)
            End Get
        End Property

    End Class


    Public Class Meta
        Public Property count As Integer
        Public Property hidden As Object
    End Class

    Public Class [Error]
        Public Property field As String
        Public Property message As String
        Public Property code As Integer
        Public Property value As String
    End Class

End Namespace