Namespace Process

    Public Class PingProcess

#Region "Public Ctor"

        Public Sub New(configLoader As IConfigLoader, sendMailProvider As SendMail)
            _configLoader = configLoader
            _sendMailProvider = sendMailProvider
        End Sub

#End Region

#Region "Public methode"

        Public Sub StartPingProcess()
            If _config Is Nothing Then
                _config = _configLoader.LoadConfigs()
            End If

            StartRunningPing()

        End Sub

        Public Sub StopPingProcess()
            If _timer.Enabled Then
                _timer.Stop()
                _sendMailProvider.SendProcessStopping(_config.AdminsEmail)
            End If
        End Sub

#End Region

#Region "Protected property"

        Protected ReadOnly Property Config As IConfigModel
            Get
                Return _config
            End Get
        End Property

#End Region


#Region "Private member"

        ''Dependance for loading the config model
        Private _configLoader As IConfigLoader

        ''Dependance for sending email
        Private _sendMailProvider As IMailProvider(Of IConfigModel)

        ''The config model for the process, only accessible from descendant class if we want other behavior, will be instanciated by the config loader
        Private _config As IConfigModel

        ''The timer used for starting the next BatchWorker if available
        Private WithEvents _timer As New Timers.Timer()

        ''The batch worker executing the job
        ''Private _batchWorker As BatchWorker

        Private Sub StartRunningPing()
            If Not _timer.Enabled Then
                _timer.Interval = _config.Interval
                _timer.Start()
                _sendMailProvider.SendProcessStarting(_config.AdminsEmail)
            End If

        End Sub

        Private Sub _timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _timer.Elapsed
            Me.ProcessPing()
        End Sub

#End Region

        ''Should be transfered to the BatchWorker
        Private _pinger As New HttpPinger



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




    End Class

End Namespace