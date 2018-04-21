

Imports WargamingDatabase

Public Class EditAccountInfo

    Private _loading As Boolean
    Private _Server As Data.Common.ServerRealm
    Private _Accounts As List(Of Data.Common.User)
    Private _RemoveAccounts As New List(Of Data.Common.User)

    Private Sub btn_search_Click(sender As Object, e As RoutedEventArgs)

        Me.btn_search.IsEnabled = False
        Me.btn_Save.IsEnabled = False

        Dim tmp As Cursor = Me.Cursor
        Me.Cursor = Cursors.Wait

        Dim Search As String = txt_username.Text
        Dim Srvs As New WargamingAPI.WorldOfWarshipsData(Me._Server)
        Dim Results As Data.Account.PlayerSearchResults = Srvs.SearchPlayers(Search)

        Me.lst_accounts.Items.Clear()

        For Each acc As Data.Account.FoundPlayer In Results.data
            Me.lst_accounts.Items.Add(acc.ToString(Me._Server))
        Next

        Me.Cursor = tmp
        Me.btn_search.IsEnabled = True

    End Sub

    Private Sub EditAccountInfo_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized

        _loading = True

        Me.btn_search.IsEnabled = False
        Me.btn_Save.IsEnabled = False
        Me.btn_add_account.IsEnabled = False
        Me.btn_remove_account.IsEnabled = False

        Me.cmbo_server.Items.Clear()

        For Each s As String In [Enum].GetNames(GetType(Data.Common.ServerRealm))
            Me.cmbo_server.Items.Add(s)
        Next

        Me.lst_use_accounts.Items.Clear()
        Me._Accounts = WargamingSqliteDataAccess.GetUserAccounts
        For Each acc As Data.Common.User In Me._Accounts
            Me.lst_use_accounts.Items.Add(acc.ToString)
        Next

        Me.txt_username.Focus()

        _loading = False

    End Sub

    Private Sub cmbo_server_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbo_server.SelectionChanged

        If _loading Then Return

        Me._Server = [Enum].Parse(GetType(Data.Common.ServerRealm), Me.cmbo_server.SelectedItem)

        Me.EnableSearchButton()

    End Sub

    Private Sub txt_username_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txt_username.TextChanged

        Me.EnableSearchButton()

    End Sub

    Private Sub TitleBar_MouseDown(sender As Object, e As MouseButtonEventArgs)

        Me.DragMove()

    End Sub

    Private Sub EnableSearchButton()

        If txt_username.Text.Length < 4 OrElse (cmbo_server.SelectedItem = Nothing) Then
            btn_search.IsEnabled = False
        Else
            btn_search.IsEnabled = True
        End If

    End Sub

    Private Sub btn_Cancel_Click(sender As Object, e As RoutedEventArgs)

        Me.DialogResult = False
        Me.Close()

    End Sub

    Private Sub lst_accounts_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        If lst_accounts.SelectedValue = "" Then
            Me.btn_add_account.IsEnabled = False
        Else
            Me.btn_add_account.IsEnabled = True
        End If

    End Sub

    Private Sub btn_add_account_Click(sender As Object, e As RoutedEventArgs)

        If lst_accounts.SelectedValue = "" Then Return

        Dim Acc As Data.Common.User = Data.Common.User.Parse(lst_accounts.SelectedValue)
        Me.RemoveFromRemoveAccounts(Acc)

        Me.lst_use_accounts.Items.Add(lst_accounts.SelectedValue)
        Me.lst_accounts.Items.Remove(lst_accounts.SelectedValue)

        Me.btn_Save.IsEnabled = ActivateSaveButton()

    End Sub

    Private Sub btn_remove_account_Click(sender As Object, e As RoutedEventArgs)

        If lst_use_accounts.SelectedValue = "" Then Return

        Dim Acc As Data.Common.User = Data.Common.User.Parse(lst_use_accounts.SelectedValue)
        If IsActiveAccount(Acc) Then
            If MessageBox.Show("This is an active account." & vbCrLf & "If you remove it the data will be deleted!" & vbCrLf & "Are you sure you want to remove it?", "CAUTION", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) = MessageBoxResult.No Then
                Return
            Else
                Me._RemoveAccounts.Add(Acc)
            End If
        End If

        Me.lst_accounts.Items.Add(lst_use_accounts.SelectedValue)
        Me.lst_use_accounts.Items.Remove(lst_use_accounts.SelectedValue)

        Me.btn_Save.IsEnabled = ActivateSaveButton()

    End Sub

    Private Sub lst_use_accounts_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst_use_accounts.SelectionChanged

        If lst_use_accounts.SelectedValue = "" Then
            Me.btn_remove_account.IsEnabled = False
        Else
            Me.btn_remove_account.IsEnabled = True
        End If

    End Sub

    Private Sub RemoveFromRemoveAccounts(acc As Data.Common.User)

        Dim index As Integer = 0
        Dim Found As Boolean = False
        For Each user As Data.Common.User In Me._RemoveAccounts
            If user.Account = acc.Account AndAlso
                    user.Server = acc.Server Then
                Found = True
                Exit For
            End If
            index += 1
        Next

        If Found Then Me._RemoveAccounts.RemoveAt(index)

    End Sub

    Private Function IsActiveAccount(acc As Data.Common.User) As Boolean

        For Each user As Data.Common.User In Me._Accounts
            If user.Account = acc.Account AndAlso
                    user.Server = acc.Server Then
                Return True
            End If
        Next
        Return False

    End Function

    Private Function ActivateSaveButton() As Boolean

        If Me._Accounts.Count <> Me.lst_use_accounts.Items.Count Then Return True

        For Each user As Data.Common.User In Me._Accounts
            For Each i As String In Me.lst_use_accounts.Items
                Dim acc As Data.Common.User = Data.Common.User.Parse(i)
                If user.Account <> acc.Account OrElse
                    user.Server <> acc.Server Then
                    Return True
                End If
            Next
        Next

        Return False

    End Function

    Private Sub btn_Save_Click(sender As Object, e As RoutedEventArgs)

        Me._Accounts.Clear()
        For Each i As String In Me.lst_use_accounts.Items
            Dim acc As Data.Common.User = Data.Common.User.Parse(i)
            Me._Accounts.Add(acc)
        Next

        Dim Confirm As New SaveAccountChanges(Me._Accounts, Me._RemoveAccounts)
        Confirm.Owner = Me
        If Confirm.ShowDialog() Then
            Me.SaveAccountChanges()
            Me.DialogResult = True
            Me.Close()
        End If

    End Sub

    Private Sub SaveAccountChanges()

        WargamingSqliteDataAccess.AddUserAccounts(Me._Accounts)
        WargamingSqliteDataAccess.DeleteUserAccounts(Me._RemoveAccounts)

    End Sub

End Class
