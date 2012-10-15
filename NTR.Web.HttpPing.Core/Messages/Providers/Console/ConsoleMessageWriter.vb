Imports NTR.Web.HttpPing.Config
Imports System.Text
Imports NTR.Web.HttpPing.Workers

Namespace Messages

    Public Class ConsoleMessageWriter : Inherits AbstractMessagesWriter

        Public Sub New()

        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            MyBase.New(messagesfilter)
        End Sub

        Protected Overrides Function CreateProcessMessage(ByVal messageType As ProcessMessageType, ByVal config As IConfigModel) As IMessage
            Dim body As New StringBuilder()
            Select Case messageType
                Case ProcessMessageType.Starting

                    body.AppendFormat("Started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName).AppendLine()
                    body.Append("---------------------------------").AppendLine()
                    body.Append("Configuration").AppendLine()
                    body.Append("- - - - - - - -").AppendLine()
                    body.AppendFormat("ProcessInterval...: {0}", config.ProcessInterval).AppendLine()
                    body.AppendFormat("Urls..............: {0}", config.Urls.Count).AppendLine()
                    For c As Integer = 0 To config.Urls.Count - 1
                        body.AppendFormat("  {0} : {1}", c + 1, config.Urls.ElementAt(c).ToString()).AppendLine()
                    Next
                    body.AppendFormat("AdminsEmail.......: {0}", config.AdminsEmail).AppendLine()
                    body.AppendFormat("PingMaxRetry......: {0}", config.PingMaxRetry).AppendLine()
                    body.AppendFormat("PingRetryInterval.: {0}", config.PingRetryInterval).AppendLine()
                    body.AppendFormat("MailServerAddr....: {0}", config.MailServerAddr).AppendLine()
                    body.AppendFormat("MailServerPort....: {0}", config.MailServerPort).AppendLine()

                Case ProcessMessageType.Stopping
                    body.AppendFormat("Stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
            End Select

            Return New MessageModel("", body.ToString)
        End Function

        Protected Overrides Function CreateUrlStatusMessage(ByVal urlResult As IPingUrlResult) As IMessage
            Dim body As String = String.Empty
            If (urlResult.Succeed) Then
                body = String.Format("UP   : {0}  ({1:dd-MM-yyyy HH:mm:ss})", urlResult.Url, Now)
            Else
                body = String.Format("DOWN : {0}  ({1:dd-MM-yyyy HH:mm:ss})", urlResult.Url, Now)
            End If
            Return New MessageModel("", body)
        End Function

        Protected Overrides Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage
            Dim body As StringBuilder = New StringBuilder()

            Dim badsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.BadUrlResults
            Dim goodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.GoodUrlResults

            body.AppendFormat("Batch completed at {0:dd-MM-yyyy HH:mm:ss}", Now).AppendLine()

            If badsUrl.Count > 0 Then
                body.AppendFormat("DOWN : {0}", badsUrl.Count).AppendLine()
                For Each it In badsUrl
                    body.AppendFormat("  - {0}", it.Value.Url)
                Next
            End If

            If goodsUrl.Count > 0 Then
                body.AppendFormat("UP : {0}", goodsUrl.Count).AppendLine()
                For Each it In goodsUrl
                    body.AppendFormat("  - {0}", it.Value.Url)
                Next
            End If

            Return New MessageModel("", body.ToString())
        End Function

    End Class

End Namespace
