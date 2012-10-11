Imports NTR.Web.HttpPing.Core.Process
Imports NTR.Web.HttpPing.Core.Messages

Module PingConsole


    Private _xmlConfigLoader As IConfigLoader = New XmlConfigLoader("../../httpping.config")
    Private _messageProvider As New ConsoleMessagesProvider(New SimpleMessagesWriter())
    Private _process As PingProcess = New PingProcess(_xmlConfigLoader, _messageProvider)

    Sub Main()

        _process.StartPingProcess()

        Console.ReadLine()

        _process.StopPingProcess()

    End Sub

End Module
