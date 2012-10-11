Imports NTR.Web.HttpPing.Core
Imports NTR.Web.HttpPing.Core.Process
Imports NTR.Web.HttpPing.Core.MessagesProvider

Public Class PingService


    Private _xmlConfigLoader As IConfigLoader = New XmlConfigLoader("httpping.config")
    Private _messageProvider As IMessagesProvider(Of IConfigModel) = New SmtpMessagesProvider()
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
