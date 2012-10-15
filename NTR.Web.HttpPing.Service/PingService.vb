﻿Imports NTR.Web.HttpPing.Process
Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Config
Imports NTR.Web.HttpPing.Workers

Public Class PingService

    Private _xmlConfigLoader As IConfigLoader = New XmlConfigLoader("httpping.config")
    Private _messageProvider As AbstractMessagesProvider = New SmtpMessagesProvider(New SmtpTextMessagesWriter_TimeLimit(New SmtpMessagesFilter_TimeLimit()), New SimpleSmtpMessagesToSelector())
    Private _process As New PingProcess(_xmlConfigLoader, _messageProvider)

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        _process.StartPingProcess()
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        _process.StopPingProcess()
    End Sub

End Class
