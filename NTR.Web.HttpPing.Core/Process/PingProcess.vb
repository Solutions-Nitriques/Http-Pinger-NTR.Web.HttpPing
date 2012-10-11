Namespace Process

    Public Class PingProcess

#Region "Public Ctor"

        Public Sub New(configLoader As IConfigLoader, sendMailProvider As IMessagesProvider(Of IConfigModel))
            _configLoader = configLoader
            _messagesProvider = sendMailProvider
        End Sub

#End Region

#Region "Public methode"

        Public Sub StartPingProcess()
            If _configModel Is Nothing Then
                _configModel = _configLoader.LoadConfigs()
            End If

            StartRunningPing()

        End Sub

        Public Sub StopPingProcess()
            If _timer.Enabled Then
                _timer.Stop()
                ''_sendMailProvider.SendProcessStopping(_config.AdminsEmail)
            End If
        End Sub

#End Region

#Region "Protected property"

        Protected ReadOnly Property Config As IConfigModel
            Get
                Return _configModel
            End Get
        End Property

#End Region

#Region "Private member"

        ''Dependance for loading the config model
        Private _configLoader As IConfigLoader

        ''Dependance for sending email
        Private _messagesProvider As IMessagesProvider(Of IConfigModel)

        ''The config model for the process, only accessible from descendant class if we want other behavior, will be instanciated by the config loader
        Private _configModel As IConfigModel

        ''The timer used for starting the next BatchWorker if available
        Private WithEvents _timer As New Timers.Timer

        ''The batch worker executing the job
        Private _batchWorker As PingBatchWorker

        Private Sub StartRunningPing()
            If Not _timer.Enabled Then
                _timer.Interval = _configModel.Interval
                _timer.Start()
                ''_sendMailProvider.SendProcessStarting(_config.AdminsEmail)
            End If

        End Sub

        Private Sub _timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _timer.Elapsed
            If (Not _batchWorker.IsRunning) Then
                ''Start a new batch
                _batchWorker.RunBatch(_configModel)
            Else
                ''skip this run, queue a new one with a new timer
                StartRunningPing()
            End If
        End Sub

#End Region

    End Class

End Namespace