﻿Imports System.IO
Imports System.Windows
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Net

Class MainWindow
    Dim themeColor As String
    Dim themeFont As String
    Dim colorR As String
    Dim colorG As String
    Dim colorB As String
    Dim appVersion As String = "2.0.0"
    Dim lastUsedColor = System.Drawing.Color.FromArgb(69, 181, 197)

    Private Sub init()
        'Populate fonts list
        For Each font As System.Drawing.FontFamily In System.Drawing.FontFamily.Families
            fontList.Items.Add(font.Name)
        Next
        Try
            Dim currentFile As New readFile("custom.styles")

            Dim colorPattern As New Regex("\d{1,3}\s\d{1,3}\s\d{1,3}")
            Dim fileColorVal As Match = colorPattern.Match(currentFile.contents)

            Dim fontStartTim = currentFile.contents.IndexOf("basefont=""") + 10
            Dim fontEndTrim = currentFile.contents.LastIndexOf(""" semibold") - fontStartTim
            Dim fileFontVal = currentFile.contents.Substring(fontStartTim, fontEndTrim)

            'Colors are in order as they appear in the UI
            Select Case fileColorVal.Value
                Case "220 79 173"
                    color0.IsChecked = True
                Case "172 25 61"
                    color1.IsChecked = True
                Case "210 71 38"
                    color2.IsChecked = True
                Case "225 143 50"
                    color3.IsChecked = True
                Case "130 186 0"
                    color4.IsChecked = True
                Case "0 138 23"
                    color5.IsChecked = True
                Case "3 179 178"
                    color6.IsChecked = True
                Case "0 130 153"
                    color7.IsChecked = True
                Case "93 178 255"
                    color8.IsChecked = True
                Case "0 114 198"
                    color9.IsChecked = True
                Case "70 23 180"
                    color10.IsChecked = True
                Case "140 0 149"
                    color11.IsChecked = True
                Case Else
                    customColorButton.IsChecked = True
            End Select

            'Restore decal style

            If currentFile.contents.Contains("graphics/decal_steam_btmr") Then
                decal_default.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/1") Then
                decal0.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/2") Then
                decal1.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/3") Then
                decal2.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/4") Then
                decal3.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/5") Then
                decal4.IsChecked = True
            ElseIf currentFile.contents.Contains("graphics/backgrounds/6") Then
                decal5.IsChecked = True
            Else
                decal6.IsChecked = True
            End If

            'Restore friends list style
            Dim friendsListStyle As New readFile("Friends/FriendsDialog.res")

            If friendsListStyle.contents.Contains("//fat") Then
                friendslist0.IsChecked = True
            ElseIf friendsListStyle.contents.Contains("//chubby") Then
                friendslist1.IsChecked = True
            Else
                friendslist2.IsChecked = True
            End If

            'Select the font read from file
            Try
                fontList.SelectedItem = fileFontVal
            Catch
                fontList.SelectedItem = "Arial"
            End Try
        Catch ex As Exception
            color9.IsChecked = True
            resetFont()

        End Try

        appVersionLabel.Content = "Version " + appVersion

    End Sub

    Private Sub uninit()

        If color0.IsChecked Then
            themeColor = "220 79 173"
        ElseIf color1.IsChecked Then
            themeColor = "172 25 61"
        ElseIf color2.IsChecked Then
            themeColor = "210 71 38"
        ElseIf color3.IsChecked Then
            themeColor = "225 143 50"
        ElseIf color4.IsChecked Then
            themeColor = "130 186 0"
        ElseIf color5.IsChecked Then
            themeColor = "0 138 23"
        ElseIf color6.IsChecked Then
            themeColor = "3 179 178"
        ElseIf color7.IsChecked Then
            themeColor = "0 130 153"
        ElseIf color8.IsChecked Then
            themeColor = "93 178 255"
        ElseIf color9.IsChecked Then
            themeColor = "0 114 198"
        ElseIf color10.IsChecked Then
            themeColor = "70 23 180"
        ElseIf color11.IsChecked Then
            themeColor = "140 0 149"
        ElseIf customColorButton.IsChecked Then
            themeColor = colorR + " " + colorG + " " + colorB
        End If

        Dim outFont As String
        Dim selectedFont = fontList.SelectedItem.ToString()

        If fontList.SelectedItem.ToString = "Segoe UI" Then
            outFont = """Segoe UI"" semibold=""Segoe UI Semibold"" semilight=""Segoe UI Semilight"" light=""Segoe UI Light"""
        Else
            outFont = """" + selectedFont + """ semibold=""" + selectedFont + """ semilight=""" + selectedFont + """ light=""" + selectedFont + """"
        End If

        Dim decalStyle As String

        If decal_default.IsChecked Then
            decalStyle = " 8=""image( x1 - 821, y1 - 375, x1, y1, graphics/decal_steam_btmr)"""
        ElseIf decal0.IsChecked Then
            decalStyle = " 1=""image( x1 - 186, y0+40, x1, y0-176, graphics/backgrounds/1)"""
        ElseIf decal1.IsChecked Then
            decalStyle = " 2=""image( x1 - 900, y1 - 194, x1, y1, graphics/backgrounds/2)"""
        ElseIf decal2.IsChecked Then
            decalStyle = " 3=""image( x1 - 603, y1 - 231, x1, y1, graphics/backgrounds/3)"""
        ElseIf decal3.IsChecked Then
            decalStyle = " 4=""image( x1 - 506, y0+43, x1, y0-298, graphics/backgrounds/4)"""
        ElseIf decal4.IsChecked Then
            decalStyle = " 5=""image( x1 - 684, y0+40, x1, y0-194, graphics/backgrounds/5_top)"" 6=""image( x1 - 508, y1 - 103, x1, y1, graphics/backgrounds/5_bot_sm)"""
        ElseIf decal5.IsChecked Then
            decalStyle = " 7=""image( x1 - 688, y0+81, x1, y0-259, graphics/backgrounds/6)"""
        Else
            decalStyle = ""
        End If

        Dim friendsListStyle As String

        If friendslist0.IsChecked Then
            friendsListStyle = "Fat"
        ElseIf friendslist1.IsChecked Then
            friendsListStyle = "Chubby"
        Else
            friendsListStyle = "Slim"
        End If

        Dim fileContents As String = """custom.styles""{colors{Focus=""" + themeColor + " 255"" basefont=" + outFont + "}" + "styles{""CSteamRootDialog""{bgcolor=ClientBG render_bg{ 0=""fill( x0, y0, x1, y1, ClientBG )""" + decalStyle + " 98=""fill( x0, y0, x1, y0+40, FrameBorder)"" 99=""fill( x0, y0, x1, y0+39, Header_Dark )""}}}}"

        Try
            If File.Exists("custom.styles") = True Then
                Dim file As New StreamWriter("custom.styles")
                file.Write(fileContents)
                file.Flush()
                file.Close()
            Else
                Dim file As TextWriter = System.IO.File.CreateText("custom.styles")
                file.Write(fileContents)
                file.Flush()
                file.Close()
            End If

            File.Copy("Friends/Options/" + friendsListStyle + "/FriendsDialog.res", "Friends/FriendsDialog.res", True)

            Me.Close()

        Catch ex As Exception
            'do error handling here
            openPopup()
        End Try

    End Sub

    Private Sub openColorPicker()
        Dim colorPicker As New ColorDialog()
        colorPicker.FullOpen = True
        colorPicker.Color = lastUsedColor

        If (colorPicker.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            colorR = colorPicker.Color.R.ToString
            colorG = colorPicker.Color.G.ToString
            colorB = colorPicker.Color.B.ToString

            lastUsedColor = System.Drawing.Color.FromArgb(colorR, colorG, colorB)
        End If
    End Sub

    Private Sub closeDlg()
        Me.Close()
    End Sub

    Private Sub openPopup()
        warningPopup.Visibility = Windows.Visibility.Visible
        popupOKbutton.Focus()
    End Sub

    Private Sub closePopup()
        warningPopup.Visibility = Windows.Visibility.Collapsed
        dlgOKbutton.Focus()
    End Sub

    Private Sub resetFont()
        Try
            fontList.SelectedItem = "Segoe UI"
        Catch
            fontList.SelectedItem = "Arial"
        End Try
    End Sub

    Class readFile
        Public contents As String
        Public Sub New(_file As String)
            If File.Exists(_file) Then
                Dim file As New StreamReader(_file)
                contents = file.ReadToEnd()
                file.Close()
            End If
        End Sub
    End Class

    Private Sub openLink(sender As Object, e As RoutedEventArgs)
        System.Diagnostics.Process.Start(e.Source.NavigateUri.ToString())
    End Sub

    Private Sub openDonateLink()
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=8Z74B76F79LTJ&lc=US&item_name=MetroSteamSettings&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted")
    End Sub

    Private Sub checkUpdates()
        checkUpdatesButton.IsEnabled = False
        Dim webClient As New WebClient
        AddHandler webClient.DownloadStringCompleted, AddressOf subUIUpdate

        webClient.DownloadStringAsync(New Uri("https://raw.github.com/SoapyHamHocks/MetroSteamSettings/master/version.txt"))
    End Sub

    Private Sub subUIUpdate(ByVal sender As Object, ByVal e As DownloadStringCompletedEventArgs)
        checkUpdatesButton.Visibility = Windows.Visibility.Collapsed
        Try
            Dim updateVersion As String = e.Result
            If updateVersion > appVersion Then
                updatesLabel.Visibility = Windows.Visibility.Visible
            Else
                noUpdatesLabel.Visibility = Windows.Visibility.Visible
            End If
        Catch
            noConnection.Visibility = Windows.Visibility.Visible
        End Try
    End Sub

End Class
