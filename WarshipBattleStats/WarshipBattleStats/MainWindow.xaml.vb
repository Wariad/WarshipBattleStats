

Imports System.IO
Imports System.Windows.Forms
Imports WargamingAPI
Imports WargamingDatabase
Imports Microsoft.WindowsAPICodePack.Dialogs

Class MainWindow

    Dim WithEvents fw As IO.FileSystemWatcher
    Dim WithEvents ni As System.Windows.Forms.NotifyIcon
    Dim _Accounts As New List(Of Data.Common.User)
    Dim Log As BattleLog = Nothing

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Me.GetAccountInfo()

        fw = New IO.FileSystemWatcher("D:\World_of_Warships\replays", "tempArenaInfo.json")
        fw.EnableRaisingEvents = True

        ni = New System.Windows.Forms.NotifyIcon()
        ni.Text = "Battle Stats"
        ni.ContextMenu = Me.CreateContextMenu()

        Using iconStream As IO.Stream = Application.GetResourceStream(New Uri("pack://application:,,,/WarshipBattleStats;component/Resources/WorldOfWarships.ico")).Stream
            ni.Icon = New System.Drawing.Icon(iconStream)
            ni.Visible = True
        End Using

        If HaveAllSettings() Then
            Me.LoadStats()

        Else
            Me.GetFirstUseSettings()
        End If

    End Sub

    Private Sub LoadStats()

        Dim x As New Task(Sub() Me.OnLoadStats())
        x.Start()

    End Sub

    Private Function HaveAllSettings() As Boolean

        Return Me._Accounts.Count <> 0 AndAlso
                  My.Settings.WowsFolder <> ""

    End Function

    Private Sub GetFirstUseSettings()

        Dim x As New Task(Sub() Me.StartGetFirstUseSettings())
        x.Start()

    End Sub

    Private Sub StartGetFirstUseSettings()

        'Just flip the thread so the UI can keep painting the splash screen.
        Threading.Thread.Sleep(10)
        Dispatcher.Invoke(Sub() Me.OnGetFirstUseSettings())

    End Sub

    Private Sub OnGetFirstUseSettings()

        Dim FirstTry As Boolean = True
        Do
            If FirstTry Then MessageBox.Show("No Account Info!" & vbNewLine & vbNewLine & "Account info is missing.", "Account Info.", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Dim acc As New EditAccountInfo
            If acc.ShowDialog() Then Exit Do
            If MessageBox.Show("No Account Info!" & vbNewLine & vbNewLine & "Battle Log Needs Account info.", "Account Info.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) = Forms.DialogResult.Cancel Then
                My.Application.Shutdown()
            End If
            FirstTry = False
        Loop

        Me.GetAccountInfo()
        My.Settings.ActiveAccount = Me._Accounts(0).ToShortString
        My.Settings.Save()

        FirstTry = True
        Do
            If FirstTry Then MessageBox.Show("No Wows Folder!" & vbNewLine & vbNewLine & "Wows folder is missing.", "Wows Folder.", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Me.GetWowsFolder() Then Exit Do
            If MessageBox.Show("No Account Info!" & vbNewLine & vbNewLine & "Without the Wows folder stats can not be automaticly updated.", "Wows Folder.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) = Forms.DialogResult.Cancel Then
                Exit Do
            End If
            FirstTry = False
        Loop

        MessageBox.Show("Get inital data!" & vbNewLine & vbNewLine & "This could take a few moments.", "Initial Data.", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.GetInitialData()

    End Sub

    Private Function GetWowsFolder() As Boolean

        Dim dlg As New CommonOpenFileDialog()
        dlg.Title = "My Title"
        dlg.IsFolderPicker = True
        dlg.InitialDirectory = My.Settings.WowsFolder

        dlg.AddToMostRecentlyUsedList = False
        dlg.AllowNonFileSystemItems = False
        dlg.DefaultDirectory = My.Computer.FileSystem.SpecialDirectories.ProgramFiles
        dlg.EnsureFileExists = True
        dlg.EnsurePathExists = True
        dlg.EnsureReadOnly = False
        dlg.EnsureValidNames = True
        dlg.Multiselect = False
        dlg.ShowPlacesList = True

        If (dlg.ShowDialog() = CommonFileDialogResult.Ok) Then
            My.Settings.WowsFolder = dlg.FileName
            My.Settings.Save()
            Return True
        End If

        Return False

    End Function

    Private Function CreateContextMenu() As Menu

        Dim menu As New ContextMenu

        Dim sww As New MenuItem("Start With Windows", Sub() Me.StartsWithWindows_Click())
        sww.Name = "sww"
        sww.Checked = My.Settings.StartWithWindows

        Dim log As New MenuItem("Show Log", Sub() Me.ShowLog())
        Dim rst As New MenuItem("Rest Log Postion", Sub() Me.ResetLogPosition())
        Dim uds As New MenuItem("Update Stats", Sub() Me.OnUpdateStats())
        Dim acc As New MenuItem("Account Info", Sub() Me.EditAccountInfo_Click())
        Dim dlg As New MenuItem("Wows Folder", Sub() Me.EditWowsFolder_Click())

        Dim cls As New MenuItem("Exit", Sub() Me.NiClose())

        menu.MenuItems.Add(log)
        menu.MenuItems.Add(rst)
        ' menu.MenuItems.Add(sep)
        menu.MenuItems.Add(uds)
        menu.MenuItems.Add(acc)
        menu.MenuItems.Add(dlg)
        menu.MenuItems.Add(sww)
        menu.MenuItems.Add(cls)

        Return menu

    End Function

    Private Sub EditAccountInfo_Click()

        Dim info As New EditAccountInfo
        info.Owner = Me
        If info.ShowDialog() Then
            Me.GetAccountInfo()
            ShowNotification("Warship Stats", "Account info updated.")
            Me.LoadStats()
        End If

    End Sub

    Private Sub GetAccountInfo()

        Me._Accounts = WargamingSqliteDataAccess.GetUserAccounts

    End Sub

    Private Sub EditWowsFolder_Click()

        Me.GetWowsFolder()

    End Sub

    Private Sub SetWindowsSartup()

        Dim reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", True)
        If My.Settings.StartWithWindows Then
            reg.SetValue("WowsBattleStats", """" & My.Application.Info.DirectoryPath & "\WarshipBattleStats.exe" & """")
        Else
            reg.DeleteValue("WowsBattleStats")
        End If

    End Sub

    Private Sub StartsWithWindows_Click()

        ni.ContextMenu.MenuItems.Item("sww").Checked = Not ni.ContextMenu.MenuItems.Item("sww").Checked
        My.Settings.StartWithWindows = ni.ContextMenu.MenuItems.Item("sww").Checked
        My.Settings.Save()

    End Sub

    Private Sub NiClose()

        My.Application.Shutdown()

    End Sub

    Private Sub fw_Created(sender As Object, e As FileSystemEventArgs) Handles fw.Created, fw.Deleted

        Dispatcher.Invoke(Sub() Me.OnUpdateStats())

        If (e.ChangeType = WatcherChangeTypes.Created) AndAlso My.Settings.PlayBattleSound Then
            My.Computer.Audio.Play("", AudioPlayMode.Background)
        End If

    End Sub

    Private Sub OnUpdateStats()

        Me.UpdateStats()
        If Log IsNot Nothing Then
            Log.UpdateLog()
        End If

    End Sub

    Private Sub ni_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ni.MouseDoubleClick

        Me.ShowLog()

    End Sub

    Private Sub ShowLog()

        If Log Is Nothing Then
            Log = New BattleLog
            Log.Show()
        Else
            Log.Show()
        End If

    End Sub

    Private Sub ResetLogPosition()

        If Log IsNot Nothing Then
            Log.Close()
            Log = Nothing
        End If

        My.Settings.BattleLogLeft = 100
        My.Settings.BattleLogTop = 100
        My.Settings.Save()

        Me.ShowLog()

    End Sub

    Private Sub OnLoadStats()

        Me.UpdateStats()
        Dispatcher.Invoke(Sub() OnStatsLoaded())

    End Sub

    Private Sub OnStatsLoaded()

        ShowLog()
        Me.Hide()

    End Sub

    Private Sub GetInitialData()

        Me.prg_firstdata.Visibility = Visibility.Visible
        Me.UpdateLayout()

        Dim x As New Task(New Action(Sub() Me.OnGetInitialData()))
        x.Start()

    End Sub

    Private Sub GetInitialDataComplete()

        Me.prg_firstdata.Visibility = Visibility.Hidden
        Me.UpdateLayout()

        Me.OnStatsLoaded()

    End Sub

    Private Sub OnGetInitialData()

        Dim UpdatedAccounts As String = "Wows stats updated!" & vbCrLf
        For Each user As Data.Common.User In Me._Accounts
            Dispatcher.Invoke(Sub() Me.OnUpdateProgressHeading(String.Format("Getting {0} from Wargaming.", user.ToShortString)))
            Dim acc As Data.Warships.PlayerShipStats = New WorldOfWarshipsData(user.Server).GetPlayerStats(user)
            WargamingDatabase.WargamingSqliteDataAccess.AddShipStats(acc.Stats, AddressOf Me.UpdateInitialDataProgress)
            UpdatedAccounts &= [Enum].GetName(GetType(Data.Common.ServerRealm), user.Server) & "~" & user.Nickname & vbCrLf
        Next
        Me.ShowNotification("Warship Stats", UpdatedAccounts)

        Dispatcher.Invoke(Sub() Me.GetInitialDataComplete())

    End Sub

    Private Sub OnUpdateProgressHeading(msg As String)

        Me.prg_label.Content = msg

    End Sub

    Private Sub OnUpdateInitialDataProgress(msg As String, image As System.Drawing.Image, total As Integer, progress As Integer)

        Me.img_ship.Source = Common.ConvertImage(image)
        Me.prg_label.Content = msg
        Me.prg_progress.Value = (1.0 * progress / total) * 100
        Me.UpdateLayout()

    End Sub

    Private Sub UpdateInitialDataProgress(msg As String, image As System.Drawing.Image, total As Integer, progress As Integer)

        Dispatcher.Invoke(Sub() OnUpdateInitialDataProgress(msg, image, total, progress))

    End Sub

    Private Sub UpdateStats()

        Dim Ver As String = New WorldOfWarshipsData(Data.Common.ServerRealm.NA).LibraryNeedsUpdate
        If Ver IsNot Nothing Then
            Dim EncycUpdate As New Task(Sub() Me.UpdateEncyclopedia(Ver))
            EncycUpdate.Start()
            Me.ShowNotification("Warship Stats", "Encyclopedia updated Started  v" & Ver)
        End If

        Dim UpdatedAccounts As String = "Wows stats updated!" & vbCrLf
        For Each user As Data.Common.User In Me._Accounts
            Dim acc As Data.Warships.PlayerShipStats = New WorldOfWarshipsData(user.Server).GetPlayerStats(user)
            WargamingDatabase.WargamingSqliteDataAccess.AddShipStats(acc.Stats)
            UpdatedAccounts &= [Enum].GetName(GetType(Data.Common.ServerRealm), user.Server) & "~" & user.Nickname & vbCrLf
        Next

        Me.ShowNotification("Warship Stats", UpdatedAccounts)

    End Sub

    Private Sub UpdateEncyclopedia(ver As String)

        Dim Srvs As New WorldOfWarshipsData(Data.Common.ServerRealm.NA)
        Dim encyc As Data.Encyclopedia.Ships = Srvs.GetEncyclopediaShips()
        WargamingDatabase.WargamingSqliteDataAccess.AddEncyclopediaShips(encyc.data)

        Me.ShowNotification("Warship Stats", "Encyclopedia updated complete v" & ver)

    End Sub

    Private Sub ShowNotification(title As String, text As String)

        Me.ni.BalloonTipTitle = title
        Me.ni.BalloonTipText = text
        Me.ni.BalloonTipIcon = ToolTipIcon.Info
        Me.ni.ShowBalloonTip(1000)

    End Sub

End Class
