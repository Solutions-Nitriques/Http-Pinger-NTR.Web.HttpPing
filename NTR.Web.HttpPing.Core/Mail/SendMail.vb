Public Class SendMail

    Private _mailServer As String
    Private _mailServerPort As Integer

    Public Sub New(ByVal server As String, ByVal port As Integer)
        _mailServer = server
        _mailServerPort = port
    End Sub

    Public Function SendMail(ByVal [to] As String, ByVal subject As String, ByVal body As String, ByVal async As Boolean) As Boolean
        Dim ret As Boolean
        Dim client As Net.Mail.SmtpClient
        Dim msg As Net.Mail.MailMessage = Nothing

        Try
            client = New Net.Mail.SmtpClient(_mailServer, _mailServerPort)
            msg = New Net.Mail.MailMessage("server@nitriques.com", [to], subject, body)
            If async Then
                client.SendAsync(msg, Nothing)
            Else
                client.Send(msg)
            End If

            ret = True
        Catch ex As Exception
            ret = False
        Finally
            ' Do not dispose asycn message as this affects the async operation
            If Not async AndAlso msg IsNot Nothing Then
                msg.Dispose()
            End If
        End Try

        Return ret
    End Function

    Public Function SendSiteDown(ByVal [to] As String, ByVal url As Uri, ByVal retries As Integer) As Boolean
        Dim subject As String = String.Format("ERROR: {0} is DOWN", url.DnsSafeHost)
        Dim body As String = String.Format("Web site {0} <{1}> is NOT responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)

        Return SendMail([to], subject, body, True)
    End Function

    Public Function SendSiteUp(ByVal [to] As String, ByVal url As Uri, ByVal retries As Integer) As Boolean
        Dim subject As String = String.Format("ALERT: {0} is UP", url.DnsSafeHost)
        Dim body As String = String.Format("Web site {0} <{1}> is now responding with a 200 OK status. Failure occured {2} times on {4}. {3:dd-MM-yyyy HH:mm:ss}", url.DnsSafeHost, url, retries, Now, Environment.MachineName)

        Return SendMail([to], subject, body, True)
    End Function

    Public Function SendProcessStopping(ByVal [to] As String) As Boolean
        Dim subject As String = String.Format("ERROR: HttpPing is DOWN")
        Dim body As String = String.Format("HttpPing has stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)

        Return SendMail([to], subject, body, False)
    End Function

    Public Function SendProcessStarting(ByVal [to] As String) As Boolean
        Dim subject As String = "ALERT: HttpPing is UP"
        Dim body As String = String.Format("HttpPing has started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)

        Return SendMail([to], subject, body, True)
    End Function

End Class
