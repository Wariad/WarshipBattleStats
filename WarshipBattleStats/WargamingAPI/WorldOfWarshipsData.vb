
Imports Newtonsoft.Json.Linq

Public Class WorldOfWarshipsData
    Inherits WargamingDataServiceBase

#Region "Declares..."



#End Region

#Region "New..."

    Public Sub New(serverRealm As ServerRealm)
        MyBase.New(serverRealm, Game.WOWS)

    End Sub

#End Region
#Region "Methods..."

    Public Function SearchPlayers(search As String) As Object

        Return MyBase.SearchPlayerName(search)

    End Function

    Public Function GetEncyclopediaShips(withImages As Boolean) As Data.Encyclopedia.Ships

        Dim Url As String = String.Format("{0}/encyclopedia/ships/?application_id={1}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID)
        Dim Response As JObject = JObject.Parse(Me.GetData(Url))

        Dim Ships As New Data.Encyclopedia.Ships
        Ships.status = Response.Item("status").ToString

        If Ships.status = "error" Then
            Ships.Error = Me.DeserialiseJson(Of Data.Common.Error)(Response.Item("error").ToString)
            Return Ships
        End If

        Ships.meta.count = Response.Item("meta").Item("count").ToString

        Dim jSonData As JObject = Response.Item("data")
        For Each jSonShip As JToken In jSonData.Children

            Dim Ship As Data.Encyclopedia.Ship = Me.DeserialiseJson(Of Data.Encyclopedia.Ship)(jSonShip.First.ToString)

            'jSonShip.Item("modules")
            Stop
        Next

        If withImages Then

        End If

        Dim Results As Data.Encyclopedia.Ships = Me.DeserialiseJson(Of Data.Encyclopedia.Ships)(Response)

        Return Results

    End Function

    Public Function GetPlayerStats(accountID As String) As Data.Warships.PlayerShipStats

        Dim Url As String = String.Format("{0}/ships/stats/?application_id={1}&account_id={2}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID, accountID)
        Dim Response As JObject = JObject.Parse(Me.GetData(Url))

        Dim PlayerStats As New Data.Warships.PlayerShipStats
        PlayerStats.status = Response.Item("status").ToString

        If PlayerStats.status = "error" Then
            PlayerStats.Error = Me.DeserialiseJson(Of Data.Common.Error)(Response.Item("error").ToString)
            Return PlayerStats
        End If

        PlayerStats.meta = Me.DeserialiseJson(Of Data.Common.Meta)(Response.Item("meta").ToString)

        For Each jSonStats As JToken In Response.Item("data").Item(accountID)
            Dim Ship As Data.Warships.ShipStats = Me.DeserialiseJson(Of Data.Warships.ShipStats)(jSonStats.ToString)
            PlayerStats.Stats.Add(Ship)
        Next

        Return PlayerStats

    End Function





#End Region
End Class
