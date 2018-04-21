
Imports System.Drawing

Namespace Data.Stats

    Public Class Tier
        Public Property Value As Integer

        Public ReadOnly Property Symbol As String
            Get
                Select Case Me.Value
                    Case 1 : Return "I"
                    Case 2 : Return "II"
                    Case 3 : Return "III"
                    Case 4 : Return "IV"
                    Case 5 : Return "V"
                    Case 6 : Return "VI"
                    Case 7 : Return "VII"
                    Case 8 : Return "VIII"
                    Case 9 : Return "IX"
                    Case 10 : Return "X"
                End Select
                Return ""
            End Get
        End Property
    End Class

    Public Class Ship
        Public Property ShipID As Int64
        Public Property ShipIDString As String
        Public Property LastBattleTime As DateTime
        Public Property Name As String
        Public Property Nation As String
        Public Property Type As String
        Public Property Tier As New Tier
        Public Property BattleType As String
        Public Property Image As Image
    End Class

    Public Class BattleLog
        Inherits List(Of BattleLogItem)

        Public Property PVP As New BattleLogSummery
        Public Property PVE As New BattleLogSummery
        Public Property Cruiser_PVE As New BattleLogSummery
        Public Property Destroyer_PVE As New BattleLogSummery
        Public Property Carrier_PVE As New BattleLogSummery
        Public Property BattleShip_PVE As New BattleLogSummery
        Public Property Cruiser_PVP As New BattleLogSummery
        Public Property Destroyer_PVP As New BattleLogSummery
        Public Property Carrier_PVP As New BattleLogSummery
        Public Property BattleShip_PVP As New BattleLogSummery

        Public Overloads Sub Add(item As BattleLogItem)
            MyBase.Add(item)

            Me.CreateSummery(item)
            MyBase.Sort(New BattleLogItemSorter)

        End Sub

        Private Sub CreateSummery(item As BattleLogItem)

            Select Case item.Ship.BattleType
                Case "PVP"
                    UpdateSummery(PVP, item)
                    Select Case item.Ship.Type
                        Case "Cruiser"
                            UpdateSummery(Cruiser_PVP, item)
                        Case "Destroyer"
                            UpdateSummery(Destroyer_PVP, item)
                        Case "AirCarrier"
                            UpdateSummery(Carrier_PVP, item)
                        Case "Battleship"
                            UpdateSummery(BattleShip_PVP, item)
                    End Select

                Case "PVE"
                    UpdateSummery(PVE, item)
                    Select Case item.Ship.Type
                        Case "Cruiser"
                            UpdateSummery(Cruiser_PVE, item)
                        Case "Destroyer"
                            UpdateSummery(Destroyer_PVE, item)
                        Case "AirCarrier"
                            UpdateSummery(Carrier_PVE, item)
                        Case "Battleship"
                            UpdateSummery(BattleShip_PVE, item)
                    End Select

            End Select


        End Sub

        Private Sub UpdateSummery(summery As BattleLogSummery, item As BattleLogItem)

            summery.Battles += 1
            summery.Wins += IIf(item.Win, 1, 0)
            summery.Draws += IIf(item.Draw, 1, 0)
            summery.Survived += IIf(item.Survived, 1, 0)
            summery.Frags += item.Frags
            summery.PlanesKilled += item.PlanesKilled
            summery.DamageDelt += item.DamageDelt
            summery.SurvivedWins += IIf((item.Win AndAlso item.Survived), 1, 0)
            summery.SurvivedLosses += IIf(((Not item.Win) AndAlso item.Survived), 1, 0)

        End Sub

        Public Sub UpdateSummery(include As IEnumerable(Of String))

            PVP = New BattleLogSummery
            PVE = New BattleLogSummery
            Cruiser_PVE = New BattleLogSummery
            Destroyer_PVE = New BattleLogSummery
            Carrier_PVE = New BattleLogSummery
            BattleShip_PVE = New BattleLogSummery
            Cruiser_PVP = New BattleLogSummery
            Destroyer_PVP = New BattleLogSummery
            Carrier_PVP = New BattleLogSummery
            BattleShip_PVP = New BattleLogSummery

            For Each item As BattleLogItem In Me
                item.ShowInLog = False
                If include.Contains(item.Ship.BattleType.ToUpper) AndAlso
                        include.Contains(item.Ship.Type.ToUpper) Then
                    item.ShowInLog = True
                    Me.CreateSummery(item)
                End If
            Next

            MyBase.Sort(New BattleLogItemSorter)

        End Sub

        Private Class BattleLogItemSorter
            Implements IComparer(Of BattleLogItem)

            Public Function Compare(x As BattleLogItem, y As BattleLogItem) As Integer Implements IComparer(Of BattleLogItem).Compare

                Return 0 - x.Ship.LastBattleTime.CompareTo(y.Ship.LastBattleTime)

            End Function

        End Class

    End Class

    Public Enum BattleResult
        Win
        Draw
        Loss
    End Enum

    Public Class BattleLogItem
        Public Property ShowInLog As Boolean
        Public Property Ship As New Ship
        Public Property Battles As Integer
        Public Property Win As Boolean
        Public Property Draw As Boolean
        Public Property Survived As Boolean
        Public Property Frags As Integer
        Public Property PlanesKilled As Integer
        Public Property DamageDelt As Integer
        Public Property CapturePoints As Integer
        Public Property TopedoShots As Integer
        Public Property TorpedoHits As Integer
        Public Property TorpedoFrags As Integer
        Public Property MainBatteryShots As Integer
        Public Property MainBatteryHits As Integer
        Public Property MainBatteryFrags As Integer
        Public Property AircraftFrags As Integer
        Public Property WR As Double
        Public Property FR As Double
        Public Property SR As Double
        Public Property DR As Double
        Public Property DMG As Double

        Public ReadOnly Property result As BattleResult
            Get
                If Me.Win Then Return BattleResult.Win
                If Me.Draw Then Return BattleResult.Draw
                Return BattleResult.Loss
            End Get
        End Property
    End Class

    Public Class BattleLogSummery

        Public Property Battles As Integer
        Public Property Wins As Integer
        Public Property SurvivedWins As Integer
        Public Property SurvivedLosses As Integer
        Public Property Draws As Integer
        Public Property Survived As Integer
        Public Property Frags As Integer
        Public Property PlanesKilled As Integer
        Public Property DamageDelt As Integer

        Public ReadOnly Property Losses As Integer
            Get
                Return Battles - Wins - Draws
            End Get
        End Property

        Public ReadOnly Property AvgDamageDelt As Integer
            Get
                Return DamageDelt / Battles
            End Get
        End Property

        Public ReadOnly Property WinRate As Double
            Get
                Return Math.Round(CDbl(Wins) / CDbl(Battles), 2)
            End Get
        End Property

        Public ReadOnly Property SurvivalRate As Double
            Get
                Return Math.Round(CDbl(Survived) / CDbl(Battles), 2)
            End Get
        End Property

        Public ReadOnly Property SurvivalWinRate As Double
            Get
                Return Math.Round(CDbl(SurvivedWins) / CDbl(Wins), 2)
            End Get
        End Property

        Public ReadOnly Property SurvivedLossRate As Double
            Get
                Return Math.Round(CDbl(SurvivedLosses) / CDbl(Losses), 2)
            End Get
        End Property

        Public ReadOnly Property FragRate As Double
            Get
                Return Math.Round(CDbl(Frags) / CDbl(Battles), 2)
            End Get
        End Property

        Public ReadOnly Property DistructionRate As Double
            Get
                Return Math.Round(CDbl(Frags) / CDbl(Survived), 2)
            End Get
        End Property

    End Class

End Namespace



