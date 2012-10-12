Namespace Messages

    Public Class ConsoleMessageWriter : Inherits AbstractMessagesWriter

        Protected Overrides Function CreateProcessMessage(ByVal messageType As ProcessMessageType) As IMessage

            Dim body As String = String.Empty
            Select Case messageType
                Case ProcessMessageType.Starting
                    body = String.Format("HttpPing has started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
                Case ProcessMessageType.Stopping
                    body = String.Format("HttpPing has stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
            End Select

            Return New MessageModel("", body)
        End Function

        Protected Overrides Function CreateSiteStatusMessage(ByVal up As Boolean, ByVal url As System.Uri, ByVal retries As Integer) As IMessage
            Dim body As String = String.Empty
            If (up) Then
                body = String.Format("Web site {0} <{1}> is now responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)
            Else
                body = String.Format("Web site {0} <{1}> is NOT responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)
            End If
            Return New MessageModel("", body)
        End Function
    End Class

End Namespace
