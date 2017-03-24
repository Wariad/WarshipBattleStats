
Imports WargamingAPI.Data
Imports Entlib = Microsoft.Practices.EnterpriseLibrary.Data

Public Class WargamingDataAccess
    Inherits DataAccess

#Region "Declares..."

#End Region

#Region "New..."

#End Region

#Region "Methods..."

    Public Shared Sub AddUser(ByVal user As Account.FoundPlayer)

        Dim Db As Entlib.Database = CreateDatabase("MarketData")

        Db.ExecuteNonQuery("dbo.AddUser", user.account_id,
                                        user.nickname)

    End Sub

    Public Shared Sub AddShipStats(stats As List(Of Warships.ShipStats))

        Dim Db As Entlib.Database = CreateDatabase("MarketData")

        For Each Stat As Warships.ShipStats In stats

            Db.ExecuteScalar("dbo.AddUser", user.account_id,
                                                   user.nickname)

        Next



    End Sub




#End Region

#Region "Properties..."

#End Region

End Class
