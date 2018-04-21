
Imports WargamingDatabase

Public Class BattleLogIListtem

    Public Sub New(item As Data.Stats.BattleLogItem)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.lbl_ShipName.Content = item.Ship.Name
        Me.lbl_ShipTier.Content = item.Ship.Tier.Symbol
        Me.img_Flag.Source = Common.ConvertResourceImage(item.Ship.Nation)
        Me.img_BattleType.Source = Common.ConvertResourceImage(item.Ship.BattleType)
        Me.img_ShipType.Source = Common.ConvertResourceImage(item.Ship.Type)
        Me.img_Ship.Source = Common.ConvertImage(item.Ship.Image)
        Me.lbl_Frags.Content = item.Frags
        Me.lbl_Planes.Content = item.PlanesKilled
        Me.lbl_Damage.Content = item.DamageDelt
        Me.lbl_Date.Content = item.Ship.LastBattleTime.ToShortDateString
        Me.lbl_Time.Content = item.Ship.LastBattleTime.ToShortTimeString
        Me.Background = GetBgColour(item.result)
        If item.Survived Then
            Me.img_survived.Visibility = Visibility.Visible
            Me.img_died.Visibility = Visibility.Hidden
        Else
            Me.img_survived.Visibility = Visibility.Hidden
            Me.img_died.Visibility = Visibility.Visible
        End If

        Me.lbl_battleType.Content = item.Ship.BattleType
        Me.lbl_Battles.Content = item.Battles
        Me.lbl_WR.Content = item.WR
        Me.lbl_FR.Content = item.FR
        Me.lbl_SR.Content = item.SR
        Me.lbl_DR.Content = item.DR
        Me.lbl_DMG.Content = item.DMG



    End Sub

    Private Function GetBgColour(result As Data.Stats.BattleResult) As LinearGradientBrush

        Dim brush As New LinearGradientBrush()
        brush.StartPoint = New Point(0.6, 0)
        brush.EndPoint = New Point(0.6, 1)

        Select Case result
            Case Data.Stats.BattleResult.Win
                brush.GradientStops.Add(New GradientStop(Media.Color.FromRgb(3, 38, 5), 0.4))

            Case Data.Stats.BattleResult.Draw
                brush.GradientStops.Add(New GradientStop(Media.Color.FromRgb(189, 99, 23), 0.4))

            Case Data.Stats.BattleResult.Loss
                brush.GradientStops.Add(New GradientStop(Media.Color.FromRgb(125, 0, 0), 0.4))

        End Select
        brush.GradientStops.Add(New GradientStop(Media.Color.FromRgb(185, 185, 185), 1))

        Return brush

    End Function



End Class
