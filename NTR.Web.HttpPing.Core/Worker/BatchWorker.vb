Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Pinger

Namespace Workers

    Public Class PingBatchWorker : Implements IPingBatchWorker

#Region "Public"

        Public Sub New(ByVal pinger As IPinger)
            _pinger = pinger
        End Sub

        Public ReadOnly Property IsRunning As Boolean Implements IPingBatchWorker.IsRunning
            Get
                Return _isRunning
            End Get
        End Property

        Public Function Run(ByVal work As IPingBatchWork) As IWorkResult Implements IPingBatchWorker.Run
            Return StartRunningBatch(work)
        End Function

#End Region

#Region "Private Section"

        Private _isRunning As Boolean

        Private _currentWorkResult As Boolean

        Private _pinger As IPinger

        Private Function StartRunningBatch(ByVal work As IPingBatchWork) As IWorkResult
            Dim result As IWorkResult = Nothing

            If (Not IsRunning) Then

                _isRunning = True

                result = RunWork(work)

                _isRunning = False

            End If
            Return result
        End Function

        Private Function RunWork(ByVal work As IPingBatchWork) As IWorkResult
            Dim listResult As New Dictionary(Of Uri, IPingUrlResult)

            ''Do The job
            For Each url As Uri In work.Urls
                listResult.Add(url, New PingUrlResult(_pinger.PingUrl(url, work.PingTimeout, work.PingRetryInterval, work.PingMaxRetry), url))
            Next

            Return New WorkResult(listResult)
        End Function

#End Region

    End Class

End Namespace