
Imports System.IO
Imports System.Net
Imports System.Drawing
Imports Newtonsoft.Json

Public MustInherit Class WargamingDataServiceBase

#Region "declares..."

    Protected _ServerRealm As ServerRealm
    Protected _Game As Game
#End Region

    Friend Sub New(serverRealm As ServerRealm, game As Game)

        Me._ServerRealm = serverRealm
        Me._Game = game

    End Sub

#Region "Methods..."

    Friend Function SearchPlayerName(search As String) As Data.Account.PlayerSearchResults

        Dim Url As String = String.Format("{0}/account/list/?application_id={1}&search={2}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID, search)
        Dim Response As String = Me.GetData(Url)

        Dim Results As Data.Account.PlayerSearchResults = Me.DeserialiseJson(Of Data.Account.PlayerSearchResults)(Response)

        Return Results

    End Function

    Protected Function DeserialiseJson(Of T)(s As String) As T

        Return CType(JsonConvert.DeserializeObject(Of T)(s), T)

    End Function

    Protected Function GetData(ByVal url As String) As String

        Dim Request As HttpWebRequest = Nothing
        Dim Response As WebResponse = Nothing
        Dim Stream As Stream = Nothing
        Dim Reader As StreamReader = Nothing

        Dim Tries As Int32 = 0
        Dim Success As Boolean = False
        Dim Result As String = ""
        Dim Ex As Exception = Nothing

        Try

            Request = CType(WebRequest.Create(url), HttpWebRequest)
            Request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)"
            Request.AllowAutoRedirect = True
            Request.KeepAlive = False
            Request.ProtocolVersion = HttpVersion.Version10
            Response = Request.GetResponse
            Stream = Response.GetResponseStream
            Reader = New StreamReader(Stream)

            Result = Reader.ReadToEnd

        Catch Ex : Finally
            If Response IsNot Nothing Then
                Response.Close()
                Reader.Close()
            End If
        End Try

        If Ex IsNot Nothing Then
            Throw Ex
        End If

        Return Result

    End Function

    Public Function GetImage(url As String) As Image

        Dim Request As HttpWebRequest = Nothing
        Dim Response As WebResponse = Nothing
        Dim Stream As Stream = Nothing
        Dim Reader As StreamReader = Nothing

        Dim Tries As Int32 = 0
        Dim Success As Boolean = False
        Dim Result As String = ""
        Dim Ex As Exception = Nothing

        Try

            Request = CType(WebRequest.Create(url), HttpWebRequest)
            Request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)"
            Request.AllowAutoRedirect = True
            Request.KeepAlive = False
            Request.ProtocolVersion = HttpVersion.Version10
            Response = Request.GetResponse
            Stream = Response.GetResponseStream
            Reader = New StreamReader(Stream)

            Result = Reader.ReadToEnd

        Catch Ex : Finally
            If Response IsNot Nothing Then
                Response.Close()
                Reader.Close()
            End If
        End Try

        If Ex IsNot Nothing Then
            Throw Ex
        End If

        '  Return Result

    End Function

#End Region

#Region "Properties..."

    Protected ReadOnly Property RealmURL(serverRealm As ServerRealm, game As Game) As String
        Get

            Dim url As String = ""

            Select Case serverRealm
                Case ServerRealm.RU
                    url = "https://api.{0}.ru/{1}"
                Case ServerRealm.EU
                    url = "https://api.{0}.eu/{1}"
                Case ServerRealm.NA
                    url = "https://api.{0}.com/{1}"
                Case ServerRealm.ASIA
                    url = "https://api.{0}.asia/{1}"
            End Select

            Select Case game
                Case Game.WOT
                    url = String.Format(url, "worldoftanks", "wot")
                Case Game.WOWS
                    url = String.Format(url, "worldofwarships", "wows")
                Case Game.WOWP
                    url = String.Format(url, "worldofwarplanes", "wowp")
            End Select

            Return url

        End Get
    End Property
#End Region

End Class
