
Imports System.ComponentModel
Imports System.Threading
Imports WargamingDatabase


Public Class BattleLog

    Private _Accounts As List(Of Data.Common.User)
    Private _Loading As Boolean
    Private _log As Data.Stats.BattleLog

    Public Sub UpdateLog()

        For Each acc As Data.Common.User In Me._Accounts
            If acc.ToShortString = My.Settings.ActiveAccount Then
                _log = WargamingDatabase.WargamingSqliteDataAccess.GetBattleLog(acc, 100)
                Exit For
            End If
        Next

        Me.FillLog()

    End Sub

    Private Sub BattleLog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        _Loading = False

    End Sub

    Private Sub BattleLog_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        My.Settings.BattleLogLeft = Me.Left
        My.Settings.BattleLogTop = Me.Top

        My.Settings.Save()

    End Sub

    Private Sub BattleLog_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged

        If _Loading Then Return

        My.Settings.BattleLogLeft = Me.Left
        My.Settings.BattleLogTop = Me.Top

        My.Settings.Save()

    End Sub

    Private Sub GetUserAccounts()

        Me._Accounts = WargamingSqliteDataAccess.GetUserAccounts

        Me.mnu_accounts.Items.Clear()
        For Each acc As Data.Common.User In Me._Accounts
            Dim i As New MenuItem
            i.IsCheckable = True
            i.IsChecked = False
            i.Tag = acc.ToShortString
            i.Header = acc.ToShortString
            Me.mnu_accounts.Items.Add(i)
        Next

        Me.SetActiveAccount()

    End Sub

    Private Sub SetActiveAccount()

        If My.Settings.ActiveAccount = "" Then
            My.Settings.ActiveAccount = Me._Accounts(0).ToShortString
            CType(Me.mnu_accounts.Items(0), MenuItem).IsChecked = True
            My.Settings.Save()
        Else
            For Each i As MenuItem In Me.mnu_accounts.Items
                i.IsChecked = False
                If i.Header = My.Settings.ActiveAccount Then
                    i.IsChecked = True
                End If
            Next
        End If

    End Sub

    Private Sub BattleLog_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized

        _Loading = True

        Me.GetUserAccounts()

        Me.Left = My.Settings.BattleLogLeft
        Me.Top = My.Settings.BattleLogTop
        Me.Topmost = My.Settings.BattleLogAlwaysOnTop
        mnu_AlwaysOnTop.IsChecked = My.Settings.BattleLogAlwaysOnTop
        Me.lbl_title.Content = "Battle Log - (" & My.Settings.ActiveAccount & ")"

        If My.Settings.ViewInLog Is Nothing Then
            My.Settings.ViewInLog = New Specialized.StringCollection
            For Each i As MenuItem In mnu_filter.Items
                i.IsChecked = True
                My.Settings.ViewInLog.Add(i.Tag)
            Next
            My.Settings.Save()
        Else
            For Each s As String In My.Settings.ViewInLog
                For Each i As MenuItem In mnu_filter.Items
                    If CStr(i.Tag).ToUpper = s Then
                        i.IsChecked = True
                    End If
                Next
            Next
        End If

        Me.UpdateLog()

    End Sub

    Private Sub FillLog()

        Me.BattleLogList.Items.Clear()

        Dim Items(My.Settings.ViewInLog.Count - 1) As String
        My.Settings.ViewInLog.CopyTo(Items, 0)
        _log.UpdateSummery(Items)

        For Each i As Data.Stats.BattleLogItem In _log
            If i.ShowInLog Then
                Dim logItem As New BattleLogIListtem(i)
                Me.BattleLogList.Items.Add(logItem)
            End If
        Next

        Me.lbl_pvp_battles.Content = _log.PVP.Battles
        Me.lbl_pvp_wins.Content = _log.PVP.Wins
        Me.lbl_pvp_winrate.Content = _log.PVP.WinRate
        Me.lbl_pvp_frags.Content = _log.PVP.Frags
        Me.lbl_pvp_fr.Content = _log.PVP.FragRate
        Me.lbl_pvp_survived.Content = _log.PVP.Survived
        Me.lbl_pvp_lsr.Content = _log.PVP.SurvivalWinRate
        Me.lbl_pvp_dsr.Content = _log.PVP.SurvivedLossRate
        Me.lbl_pvp_dr.Content = _log.PVP.DistructionRate

        Me.lbl_pve_battles.Content = _log.PVE.Battles
        Me.lbl_pve_wins.Content = _log.PVE.Wins
        Me.lbl_pve_winrate.Content = _log.PVE.WinRate
        Me.lbl_pve_frags.Content = _log.PVE.Frags
        Me.lbl_pve_fr.Content = _log.PVE.FragRate
        Me.lbl_pve_survived.Content = _log.PVE.Survived
        Me.lbl_pve_lsr.Content = _log.PVE.SurvivalWinRate
        Me.lbl_pve_dsr.Content = _log.PVE.SurvivedLossRate
        Me.lbl_pve_dr.Content = _log.PVE.DistructionRate

        Me.grd_StatSummery.RowDefinitions.Item(0).Height = IIf(mnu_summery_pve.IsChecked OrElse mnu_summery_pvp.IsChecked, New GridLength(22), New GridLength(0))
        Me.grd_StatSummery.RowDefinitions.Item(1).Height = IIf(mnu_summery_pvp.IsChecked, New GridLength(22), New GridLength(0))
        Me.grd_StatSummery.RowDefinitions.Item(2).Height = IIf(mnu_summery_pve.IsChecked, New GridLength(22), New GridLength(0))

        If mnu_summery_pve.IsChecked OrElse mnu_summery_pvp.IsChecked Then
            If mnu_summery_pve.IsChecked AndAlso mnu_summery_pvp.IsChecked Then
                Me.grd_StatSummery.Height = 66
            Else
                Me.grd_StatSummery.Height = 44
            End If
        Else
            Me.grd_StatSummery.Height = 0
        End If

        Me.grd_StatSummery.InvalidateArrange()

        If _Loading Then Return

        My.Settings.Save()

    End Sub

    Private Sub mnu_AlwaysOnTop_Click(sender As Object, e As RoutedEventArgs)

        My.Settings.BattleLogAlwaysOnTop = mnu_AlwaysOnTop.IsChecked
        Me.Topmost = mnu_AlwaysOnTop.IsChecked
        My.Settings.Save()

    End Sub

    Private Sub TitleBar_MouseDown(sender As Object, e As MouseButtonEventArgs)

        Me.DragMove()

    End Sub

    Private Sub mnu_close_Click(sender As Object, e As RoutedEventArgs)

        Me.Hide()

    End Sub

    Private Sub mnu_summery_click(sender As Object, e As RoutedEventArgs)

        Dim Item As MenuItem = CType(sender, MenuItem)

        If Item.Name = "mnu_summery_pvp" AndAlso Not mnu_summery_pve.IsChecked Then
            mnu_summery_pve.IsChecked = True
        End If

        If Item.Name = "mnu_summery_pve" AndAlso Not mnu_summery_pvp.IsChecked Then
            mnu_summery_pvp.IsChecked = True
        End If

        My.Settings.ViewInLog.Clear()
        For Each i As MenuItem In mnu_filter.Items
            If i.IsChecked Then
                My.Settings.ViewInLog.Add(CStr(i.Tag).ToUpper)
            End If
        Next

        My.Settings.Save()

        Me.FillLog()

    End Sub

    Private Sub mnu_accounts_Click(sender As Object, e As RoutedEventArgs)

        Dim mnu As MenuItem = CType(e.Source, MenuItem)
        For Each i As MenuItem In Me.mnu_accounts.Items
            i.IsChecked = False
            If i.Header = mnu.Header Then
                i.IsChecked = True
                My.Settings.ActiveAccount = i.Header
                My.Settings.Save()
                Me.lbl_title.Content = "Battle Log - (" & My.Settings.ActiveAccount & ")"
            End If
        Next

        Me.UpdateLog()

    End Sub

End Class
