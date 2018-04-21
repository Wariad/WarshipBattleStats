
Imports WargamingDatabase

Public Class SaveAccountChanges

    Private _saveAccounts As List(Of Data.Common.User)
    Private _removeAccounts As List(Of Data.Common.User)

    Public Sub New(saveAccounts As List(Of Data.Common.User), removeAccounts As List(Of Data.Common.User))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _removeAccounts = removeAccounts
        _saveAccounts = saveAccounts

        For Each save As Data.Common.User In _saveAccounts
            Me.lst_add_accounts.Items.Add(save.ToString)
        Next

        For Each remove As Data.Common.User In _removeAccounts
            Me.lst_remove_accounts.Items.Add(remove.ToString)
        Next

    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As RoutedEventArgs)

        Me.DialogResult = False
        Me.Close()

    End Sub

    Private Sub btn_continue_Click(sender As Object, e As RoutedEventArgs)

        Me.DialogResult = True
        Me.Close()

    End Sub

End Class
