Imports System
Imports System.Configuration
'https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=dotnet-plat-ext-7.0
Module Program

    Sub Main(args As String())
        ReadAllSettings()
        ReadSetting("Key0")
        ReadSetting("NotValid")
        AddUpdateAppSettings("Key0", Date.Now.ToString)
        ReadAllSettings()
        Console.ReadKey()
    End Sub
    Private Sub ReadAllSettings()
        Try
            Dim appSettings = ConfigurationManager.AppSettings

            If appSettings.Count = 0 Then
                Console.WriteLine("AppSettings is empty.")
            Else

                For Each key In appSettings.AllKeys
                    Console.WriteLine("Key: {0} Value: {1}", key, appSettings(key))
                Next
            End If

        Catch ex As ConfigurationErrorsException
            Console.WriteLine("Error reading app settings")
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub ReadSetting(ByVal key As String)
        Try
            Dim appSettings = ConfigurationManager.AppSettings
            Dim result As String = If(appSettings(key), "Not Found")
            Console.WriteLine(result)
        Catch ex As ConfigurationErrorsException
            Console.WriteLine("Error reading app settings")
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub AddUpdateAppSettings(ByVal key As String, ByVal value As String)
        Try
            Dim configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            If settings(key) Is Nothing Then
                settings.Add(key, value)
            Else
                settings(key).Value = value
            End If

            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch ex As ConfigurationErrorsException
            Console.WriteLine("Error writing app settings")
            Console.WriteLine(ex.Message)
        End Try
    End Sub
End Module
