Imports NTR.Web.HttpPing.Config
Imports NTR.Web.HttpPing.Workers
Imports System.Text

Namespace Messages

    Public MustInherit Class SmtpTextMessagesWriter_Base : Inherits AbstractMessagesWriter

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            MyBase.New(messagesfilter)
        End Sub

        Protected Overrides Function CreateProcessMessage(ByVal messageType As ProcessMessageType, ByVal config As IConfigModel) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            Select Case messageType
                Case ProcessMessageType.Starting
                    subject = "INFO : HttpPing is UP"
                    body = String.Format("HttpPing has started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
                Case ProcessMessageType.Stopping
                    subject = String.Format("WARN: HttpPing is DOWN")
                    body = String.Format("HttpPing has stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
            End Select

            Return New MessageModel(subject, body)
        End Function

        Protected Overrides Function CreateUrlStatusMessage(ByVal urlResult As IPingUrlResult) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            If (urlResult.Succeed) Then
                subject = String.Format("WARN : {0} is UP", urlResult.Url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is now responding with a 200 OK status. {2:dd-MM-yyyy HH:mm:ss}", urlResult.Url.DnsSafeHost, urlResult.Url, Now)
            Else
                subject = String.Format("ERROR : {0} is DOWN", urlResult.Url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is NOT responding with a 200 OK status. {2:dd-MM-yyyy HH:mm:ss}", urlResult.Url.DnsSafeHost, urlResult.Url, Now)
            End If
            Return New MessageModel(subject, body)
        End Function

    End Class

End Namespace