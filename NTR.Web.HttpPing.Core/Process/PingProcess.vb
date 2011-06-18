Public Class PingProcess

    Private _pinger As New HttpPinger

    Private _mailer As SendMail

    Private _config As New Config

    Private WithEvents _timer As New Timers.Timer()

    Private _errors As New Dictionary(Of String, PingError)

    Public Event SiteProcessed(ByVal sender As Object, ByVal e As SiteProcessedEventArgs)

    Public Class SiteProcessedEventArgs : Inherits EventArgs
        Public Property Success As Boolean
        Public Property Url As Uri
    End Class

    Private Class PingError
        Public Property Url As Uri
        Public Property Count As Integer
    End Class

    Protected Sub OnSiteProcessedEventArgs(ByVal success As Boolean, ByVal url As Uri)
        Dim e As New SiteProcessedEventArgs
        e.Success = success
        e.Url = url
        RaiseEvent SiteProcessed(Me, e)
    End Sub

    Public Sub LaunchPing(ByVal configFile As String)
        If _config.LoadConfigs(configFile) Then

            _mailer = New SendMail(_config.MailServerAddr, _config.MailServerPort)

            If Not _timer.Enabled Then
                _timer.Interval = _config.Interval
                _timer.Start()
                _mailer.SendProcessStarting(_config.AdminsEmail)
            End If
        End If
    End Sub

    Public Sub StopPing()
        If _timer.Enabled Then
            _timer.Stop()
            _mailer.SendProcessStopping(_config.AdminsEmail)
        End If
    End Sub

    Private Sub ProcessPing()
        _timer.Stop()
        For Each url As Uri In _config.Urls
            Try
                ProcessOne(url)
            Catch ex As Exception
                OnSiteProcessedEventArgs(False, url)
            End Try
        Next
        _timer.Start()
    End Sub

    Private Sub ProcessOne(ByVal url As Uri)
        ' Process the request
        Dim ret As Boolean = _pinger.PingUrl(url, _config.Timeout, _config.MaxRetry)

        ' Raise event
        OnSiteProcessedEventArgs(ret, url)

        ' Check if url was in error
        Dim err As PingError = Nothing
        _errors.TryGetValue(url.ToString, err)

        ' If successful
        If ret Then
            ' If error was in error
            If err IsNot Nothing Then
                ' Remove from stack
                _errors.Remove(url.ToString)
                ' Send a site up alert
                _mailer.SendSiteUp(_config.AdminsEmail, url, err.Count)
            End If
            ' Not in error, everything is OK
        Else
            ' If not already in error
            If err Is Nothing Then
                ' Create a new error object 
                err = New PingError
                err.Count = 1
                err.Url = url
                ' Add it to the stack
                _errors.Add(url.ToString, err)
            Else
                ' Increment error count
                err.Count += 1
            End If

            ' Send a site down alert if requiered
            If err.Count <= _config.MaxEmailSend Then
                _mailer.SendSiteDown(_config.AdminsEmail, url, _errors.Count)
            End If
        End If
    End Sub

    Private Sub _timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _timer.Elapsed
        Me.ProcessPing()
    End Sub

    Public Function GetDirectory() As String
        Dim loc As String = System.Reflection.Assembly.GetAssembly(GetType(PingProcess)).Location
        Dim directory As New Text.StringBuilder()
        Dim split As String() = loc.Split("\"c)
        For x As Integer = 0 To split.Length - 2
            directory.AppendFormat("{0}\", split(x))
        Next
        Return directory.ToString
    End Function
End Class
