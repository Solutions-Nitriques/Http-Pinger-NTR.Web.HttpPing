Imports System.Net.Mail

Namespace Messages

    Public Class SimpleMessagesWriter : Inherits AbstractMessagesWriter

        Protected Overrides Function CreateProcessMessage(ByVal messageType As ProcessMessageType) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            Select Case messageType
                Case ProcessMessageType.Starting
                    subject = "ALERT: HttpPing is UP"
                    body = String.Format("HttpPing has started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
                Case ProcessMessageType.Stopping
                    subject = String.Format("ERROR: HttpPing is DOWN")
                    body = String.Format("HttpPing has stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
            End Select

            Return New MessageModel(subject, body)
        End Function

        Protected Overrides Function CreateSiteStatusMessage(ByVal up As Boolean, ByVal url As System.Uri, ByVal retries As Integer) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            If (up) Then
                subject = String.Format("ALERT: {0} is UP", url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is now responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)
            Else
                subject = String.Format("ERROR: {0} is DOWN", url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is NOT responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)
            End If
            Return New MessageModel(subject, body)
        End Function



    End Class

End Namespace