
Imports System.Drawing


Public Class Common

    Public Shared Function ConvertImage(image As Image) As BitmapImage

        Dim bi As New BitmapImage()

        Using ms As New IO.MemoryStream()
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            ms.Position = 0

            bi.BeginInit()
            bi.CacheOption = BitmapCacheOption.OnLoad
            bi.StreamSource = ms
            bi.EndInit()

        End Using

        Return bi

    End Function

    Public Shared Function ConvertResourceImage(name As String) As BitmapImage

        Return New BitmapImage(New Uri("pack://application:,,,/WarshipBattleStats;component/Resources/" & name & ".png"))

    End Function


End Class
