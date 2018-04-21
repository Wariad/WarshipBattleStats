
Namespace Data.Battle

    Public Class TeamLineups
        Public Property clientVersionFromXml As String
        Public Property gameMode As Integer
        Public Property clientVersionFromExe As String
        Public Property scenarioUiCategoryId As Integer
        Public Property mapDisplayName As String
        Public Property mapId As Integer
        Public Property matchGroup As String
        Public Property weather As String
        Public Property duration As Integer
        Public Property gameLogic As String
        Public Property name As String
        Public Property scenario As String
        Public Property playerID As Integer
        Public Property vehicles() As Vehicle
        Public Property playersPerTeam As Integer
        Public Property dateTime As String
        Public Property mapName As String
        Public Property playerName As String
        Public Property scenarioConfigId As Integer
        Public Property teamsCount As Integer
        Public Property logic As String
        Public Property playerVehicle As String
    End Class

    Public Class Vehicle
        Public Property shipId As Long
        Public Property relation As Integer
        Public Property id As Integer
        Public Property name As String
    End Class

End Namespace
