Imports NTR.Web.HttpPing.Process
Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Config
Imports NTR.Web.HttpPing.Workers
Imports System.ComponentModel

Public Class PingService

    Private WithEvents _backgroundWorker As BackgroundWorker
    Private _process As PingProcess

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        Dim _xmlConfigLoader As IConfigLoader = New XmlConfigLoader("httpping.config")
        Dim _messageProvider As AbstractMessagesProvider = New SmtpMessagesProvider(New SmtpTextMessagesWriter_TimeLimit(New SmtpMessagesFilter_TimeLimit()), New SimpleSmtpMessagesToSelector())
        _process = New PingProcess(_xmlConfigLoader, _messageProvider)
        _backgroundWorker = New BackgroundWorker()
        _backgroundWorker.RunWorkerAsync()
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        _process.StopPingProcess()
    End Sub

    Private Sub _backgroundWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles _backgroundWorker.DoWork
        _process.StartPingProcess()
    End Sub

End Class
