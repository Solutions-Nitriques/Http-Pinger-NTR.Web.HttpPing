Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Config
Imports NTR.Web.HttpPing.Workers

Namespace Process

    Public Class PingProcess

#Region "Public Ctor"

        Public Sub New(ByVal configLoader As IConfigLoader, ByVal messagesProvider As AbstractMessagesProvider)
            _configLoader = configLoader
            _messagesProvider = messagesProvider
        End Sub

#End Region

#Region "Public methode"

        Public Sub StartPingProcess()

            Me.Init()

            Me.Run()

        End Sub

        Public Sub StopPingProcess()
            Me.Stop()
        End Sub

#End Region

#Region "Private member"

        ''Dependance for loading the config model
        Private _configLoader As IConfigLoader

        ''Dependance for sending email
        Private _messagesProvider As AbstractMessagesProvider

        Private _messagesWriter As AbstractMessagesWriter

        ''The config model for the process, only accessible from descendant class if we want other behavior, will be instanciated by the config loader
        Private _configModel As IConfigModel

        ''The timer used for starting the next BatchWorker if available
        Private WithEvents _timer As New Timers.Timer

        ''The batch worker executing the job 
        Private _batchWorker As IPingBatchWorker

        Private Sub Init()
            ''Init system
            _configModel = _configLoader.LoadConfigs()
            _messagesProvider.Init(_configModel)
        End Sub

        Private Sub Run()
            If Not _timer.Enabled Then
                _timer.Interval = _configModel.Interval
                _timer.Start()

                _messagesProvider.Writer.WriteProcessMessage(ProcessMessageType.Starting, True)
            End If
        End Sub

        Private Sub [Stop]()
            If _timer.Enabled Then
                _timer.Stop()
                _messagesProvider.Writer.WriteProcessMessage(ProcessMessageType.Stopping, False)
            End If
        End Sub

        Private Sub _timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _timer.Elapsed
            If (Not _batchWorker.IsRunning) Then
                ''Start a new batch
                _batchWorker.RunBatch(_configModel)
            Else
                ''skip this run, queue a new one with a new timer
                Run()
            End If
        End Sub

#End Region

    End Class

End Namespace