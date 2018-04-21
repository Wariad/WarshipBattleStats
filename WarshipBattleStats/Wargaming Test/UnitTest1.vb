Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports WargamingAPI
Imports WargamingDatabase
Imports WarshipBattleStats


<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub SearchWarshipUsers()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        Dim Data As Data.Account.PlayerSearchResults = Srvs.SearchPlayers("wariad")

    End Sub

    <TestMethod()> Public Sub GetEncyclopediaShips()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        Dim Data As Data.Encyclopedia.Ships = Srvs.GetEncyclopediaShips()
        'WargamingDatabase.WargamingDataAccess.AddEncyclopediaShips(Data.data)

    End Sub

    <TestMethod()> Public Sub GetPlayerStats()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        'Dim Data As Data.Warships.PlayerShipStats = Srvs.GetPlayerStats("1006779797")
        '   WargamingDatabase.WargamingSqliteDataAccess.AddShipStats(Data.Stats)

    End Sub

    <TestMethod()> Public Sub GetEncyclopediaAchievments()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        Dim Data As Data.Encyclopedia.Achievments = Srvs.GetEncyclopediaAchievments()
        '  WargamingDatabase.WargamingDataAccess.AddAchievments(Data.data)

    End Sub

    <TestMethod()> Public Sub GetPlayerAchievments()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        ' Dim Data As Data.Account.PlayerAchievements = Srvs.GetPlayerAchievments("1006779797")
        '  WargamingDatabase.WargamingDataAccess.AddUserAchievments("1006779797", Data.earned)

    End Sub

    <TestMethod()> Public Sub GetAPIVersion()

        Dim Srvs As New WorldOfWarshipsData(WargamingDatabase.Data.Common.ServerRealm.NA)
        Dim s As String = Srvs.GetAPIVersion()
        ' WargamingDatabase.WargamingDataAccess.AddUserAchievments("1006779797", Data.earned)

    End Sub

    <TestMethod()> Public Sub GetBattleLog()

        ',WargamingDatabase.WargamingSqliteDataAccess.GetBattleLog(100)

    End Sub

    <TestMethod()> Public Sub GetAccountInfo()

        Dim info As New EditAccountInfo
        info.ShowDialog()

    End Sub

    <TestMethod()> Public Sub ShowLog()

        Dim log As New BattleLog
        log.ShowDialog()

    End Sub


End Class