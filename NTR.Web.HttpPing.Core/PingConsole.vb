Module PingConsole


    Private xmlConfigLoader As IConfigLoader = New XmlConfigLoader("../../httpping.config")
    Private _sendMailProvider As SendMail = New SendMail(_config.MailServerAddr, _config.MailServerPort)
    Private WithEvents process As PingProcess = New PingProcess(xmlConfigLoader)

    Sub Main()


        Console.WriteLine("Config file in {0} loaded", file)

        process.LaunchPing()

        Console.ReadLine()

        process.StopPing()
    End Sub

    Private Sub process_SiteProcessed(ByVal sender As Object, ByVal e As PingProcess.SiteProcessedEventArgs) Handles Process.SiteProcessed
        Console.WriteLine("Url {0} : {1}", e.Url, e.Success)
    End Sub
End Module
