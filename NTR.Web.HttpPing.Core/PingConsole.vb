Imports NTR.Web.HttpPing.Core.Process
Imports NTR.Web.HttpPing.Core.MessagesProvider

Module PingConsole


    Private _xmlConfigLoader As IConfigLoader = New XmlConfigLoader("../../httpping.config")
    Private _messageProvider As IMessagesProvider(Of IConfigModel) = New SmtpMessagesProvider()
    Private _process As PingProcess = New PingProcess(_xmlConfigLoader, _messageProvider)

    Sub Main()

        _process.StartPingProcess()

        Console.ReadLine()

        _process.StopPingProcess()

    End Sub

    'Private Sub process_SiteProcessed(ByVal sender As Object, ByVal e As PingProcess.SiteProcessedEventArgs) Handles process.SiteProcessed
    '    Console.WriteLine("Url {0} : {1}", e.Url, e.Success)
    'End Sub
End Module
