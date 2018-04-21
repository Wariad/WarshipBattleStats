
Imports Newtonsoft.Json.Linq
Imports System.Drawing
Imports WargamingDatabase

Public Class WorldOfWarshipsData
    Inherits WargamingDataServiceBase

#Region "Declares..."



#End Region

#Region "New..."

    Public Sub New(serverRealm As Data.Common.ServerRealm)
        MyBase.New(serverRealm, Data.Common.Game.WOWS)

    End Sub

#End Region

#Region "Methods..."

    Public Function SearchPlayers(search As String) As Object

        Return MyBase.SearchPlayerName(search)

    End Function

    Public Function LibraryNeedsUpdate() As String

        Dim ver As String = Me.GetAPIVersion
        If My.Settings.APIversion <> ver Then
            My.Settings.APIversion = ver
            My.Settings.Save()
            Return ver
        Else
            Return Nothing
        End If

    End Function

    Public Function GetAPIVersion() As String

        Dim Url As String = String.Format("{0}/encyclopedia/info/?application_id={1}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID)
        Dim response As JObject = JObject.Parse(Me.GetData(Url))

        Return response.Item("data").Item("game_version").ToString

    End Function

    Public Function GetPlayerAchievments(accountID As String) As Data.Account.PlayerAchievements

        Dim Url As String = String.Format("{0}/account/achievements/?application_id={1}&account_id={2}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID, accountID)
        Dim response As JObject = JObject.Parse(Me.GetData(Url))

        Dim achievs As New Data.Account.PlayerAchievements
        achievs.status = response.Item("status").ToString

        For Each a As JToken In response.Item("data").Item(accountID).Item("battle")
            Dim x As New Data.Account.AchievementData(a.ToString)
            achievs.earned.Add(x)
        Next

        For Each a As JToken In response.Item("data").Item(accountID).Item("progress")
            Dim x As New Data.Account.AchievementData(a.ToString)
            achievs.earning.Add(x)
        Next

        Return achievs

    End Function

    Public Function GetEncyclopediaShips() As Data.Encyclopedia.Ships

        Dim response As JObject = GetEncyclopediaShips(1)

        Dim Ships As New Data.Encyclopedia.Ships
        Ships.status = response.Item("status").ToString

        If Ships.status = "error" Then
            Ships.Error = Me.DeserialiseJson(Of Data.Common.Error)(response.Item("error").ToString)
            Return Ships
        End If

        Ships.data.AddRange(DesierialiseShipPage(response.Item("data")))

        Dim pages As Integer = response.Item("meta").Item("page_total").ToString
        If pages > 1 Then
            For i = 2 To pages
                response = Me.GetEncyclopediaShips(i)
                Ships.data.AddRange(DesierialiseShipPage(response.Item("data")))
            Next
        End If

        Return Ships

    End Function

    Private Function DesierialiseShipPage(page As JToken) As List(Of Data.Encyclopedia.Ship)

        Dim ships As New List(Of Data.Encyclopedia.Ship)

        For Each jSonShip As JToken In page.Children

            Dim ship As New Data.Encyclopedia.Ship
            ship.images.small = jSonShip.First.Item("images").Item("small")
            ship.images.medium = jSonShip.First.Item("images").Item("medium")
            ship.images.large = jSonShip.First.Item("images").Item("large")
            ship.images.contour = jSonShip.First.Item("images").Item("contour")

            Dim small As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(ship.images.small))
            'Dim medium As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(ship.images.medium))
            'Dim large As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(ship.images.large))
            'Dim contour As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(ship.images.contour))

            ship.images.smallImage = small.Result
            'ship.images.mediumImage = medium.Result
            'ship.images.largeImage = large.Result
            'ship.images.contourImage = contour.Result

            ship.tier = jSonShip.First.Item("tier")
            ship.description = jSonShip.First.Item("description")
            ship.price_gold = jSonShip.First.Item("price_gold")
            ship.ship_id = jSonShip.First.Item("ship_id")
            ship.name = jSonShip.First.Item("name")
            ship.nation = jSonShip.First.Item("nation")
            ship.is_premium = jSonShip.First.Item("is_premium")
            ship.ship_id_str = jSonShip.First.Item("ship_id_str")
            ship.price_credit = jSonShip.First.Item("price_credit")
            ship.type = jSonShip.First.Item("type")
            ship.mod_slots = jSonShip.First.Item("mod_slots")
            ship.default_profile = Me.DeserialiseJson(Of Data.Encyclopedia.Default_Profile)(jSonShip.First.Item("default_profile").ToString)
            ship.modules = Me.DeserialiseJson(Of Data.Encyclopedia.Modules)(jSonShip.First.Item("modules").ToString)

            For Each upgrade As JToken In jSonShip.First.Item("upgrades")
                ship.upgrades.Add(upgrade)
            Next

            For Each [module] As JToken In jSonShip.First.Item("modules_tree")
                ship.modules_tree.Add(Me.DeserialiseJson(Of Data.Encyclopedia.TreeModule)([module].First.ToString))
            Next

            Task.WaitAll(small)

            ships.Add(ship)

        Next

        Return ships

    End Function

    Private Function GetEncyclopediaShips(pageNo As Integer) As JObject

        Dim Url As String = String.Format("{0}/encyclopedia/ships/?application_id={1}&page_no={2}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID, pageNo)
        Return JObject.Parse(Me.GetData(Url))

    End Function

    Public Function GetPlayerStats(user As Data.Common.User) As Data.Warships.PlayerShipStats

        Dim Url As String = String.Format("{0}/ships/stats/?application_id={1}&extra=pve,oper_solo,rank_solo,club&account_id={2}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID, user.Account)
        Dim Response As JObject = JObject.Parse(Me.GetData(Url))

        Dim PlayerStats As New Data.Warships.PlayerShipStats
        PlayerStats.status = Response.Item("status").ToString
        PlayerStats.User = user

        If PlayerStats.status = "error" Then
            PlayerStats.Error = Me.DeserialiseJson(Of Data.Common.Error)(Response.Item("error").ToString)
            Return PlayerStats
        End If

        PlayerStats.meta = Me.DeserialiseJson(Of Data.Common.Meta)(Response.Item("meta").ToString)

        For Each jSonStats As JToken In Response.Item("data").Item(user.Account)
            Dim Ship As Data.Warships.ShipStats = Me.DeserialiseJson(Of Data.Warships.ShipStats)(jSonStats.ToString)
            Ship.User = user
            PlayerStats.Stats.Add(Ship)
        Next

        Return PlayerStats

    End Function

    Public Function GetEncyclopediaAchievments() As Data.Encyclopedia.Achievments

        Dim Url As String = String.Format("{0}/encyclopedia/achievements/?application_id={1}", Me.RealmURL(Me._ServerRealm, Me._Game), My.Settings.ApplicationID)
        Dim Response As JObject = JObject.Parse(Me.GetData(Url))

        Dim Achievments As New Data.Encyclopedia.Achievments

        For Each jSonStats As JToken In Response.Item("data").Item("battle")
            Dim Achievment As Data.Encyclopedia.Achievment = Me.DeserialiseJson(Of Data.Encyclopedia.Achievment)(jSonStats.First.ToString)

            Dim active As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(Achievment.image))
            Dim inActive As Task(Of Image) = Task(Of Image).Run(Function() Me.GetImage(Achievment.image_inactive))

            Task.WaitAll(active, inActive)

            Achievment.activeImage = active.Result
            Achievment.inactiveImage = inActive.Result

            Achievments.data.Add(Achievment)
        Next

        Return Achievments

    End Function



#End Region
End Class
