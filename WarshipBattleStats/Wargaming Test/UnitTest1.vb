Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports WargamingAPI

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub SearchWarshipUsers()

        Dim Srvs As New WorldOfWarshipsData(ServerRealm.NA)
        Dim Data As Data.Account.PlayerSearchResults = Srvs.SearchPlayers("wariad")

    End Sub

    <TestMethod()> Public Sub GetEncyclopediaShips()

        Dim Srvs As New WorldOfWarshipsData(ServerRealm.NA)
        Dim Data As Data.Encyclopedia.Ships = Srvs.GetEncyclopediaShips(True)

    End Sub

    <TestMethod()> Public Sub GetPLayerStats()

        Dim Srvs As New WorldOfWarshipsData(ServerRealm.NA)
        Dim Data As Data.Warships.PlayerShipStats = Srvs.GetPlayerStats("1006779797")

    End Sub


End Class