Imports NTR.Web.HttpPing.Process
Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Config

Module PingConsole

    Private _process As PingProcess

    Sub Main()

        Init()

        _process.StartPingProcess()

        System.Console.ReadLine()

        _process.StopPingProcess()

    End Sub

    Private Sub Init()

        '' Url to Test
        Dim urls As New Collections.ObjectModel.Collection(Of Uri)
        urls.Add(New Uri("http://www.google.com"))

        '---- ConfigLoader
        Dim _configLoader As IConfigLoader = New XmlConfigLoader("../../httppingExpress.config")
        'Dim _configLoader As IConfigLoader = New MockConfigLoader(ConfigModel.createModel(30000, urls, 3, "pascal@nitriques.com", 500, 800, 3, "smtp.isp.novavision.ca", 25))

        '---- MessagesFilter
        'Dim _messagesFilter As IMessagesFilter = New SmtpMessagesFilter_Differential
        Dim _messagesFilter As IMessagesFilter = New SmtpMessagesFilter_TimeLimit

        '---- MessagesWriter with no Filter
        'Dim _messagesWriter As AbstractMessagesWriter = New SmtpTextMessagesWriter()
        'Dim _messagesWriter As AbstractMessagesWriter = New ConsoleMessageWriter()

        '---- MessageWriter with filter
        'Dim _messagesWriter As AbstractMessagesWriter = New SmtpTextMessagesWriter_Differential(_messagesFilter)
        Dim _messagesWriter As AbstractMessagesWriter = New SmtpTextMessagesWriter_TimeLimit(_messagesFilter)
        'Dim _messagesWriter As AbstractMessagesWriter = New ConsoleMessageWriter(_messagesFilter)

        '---- Destinataire selector
        Dim _messagesToSelector As ISmtpMessagesToSelector = New SimpleSmtpMessagesToSelector()

        '---- MessageProvider
        Dim _messageProvider As AbstractMessagesProvider = New ConsoleMessagesProvider(_messagesWriter)
        'Dim _messageProvider As AbstractMessagesProvider = New SmtpMessagesProvider(_messagesWriter, _messagesToSelector)

        '---- The real process
        _process = New PingProcess(_configLoader, _messageProvider)
    End Sub

End Module
