Public Class PingBatchWorker : Implements IPingBatchWorker

    Private _currentWork As IPingBatchWork

    Private _pinger As IPinger
    Private _messagesProvider As IMessagesProvider(Of IPingBatchWork)
    Private _messagesCreator As IMessagesCreator

    Public Sub New(pinger As IPinger, messagesProvider As IMessagesProvider(Of IPingBatchWork), messagesCreator As IMessagesCreator)
        _pinger = pinger
        _messagesProvider = messagesProvider
    End Sub

    Public ReadOnly Property IsRunning As Boolean Implements IPingBatchWorker.IsRunning
        Get
            Return _currentWork IsNot Nothing
        End Get
    End Property

    Public Sub RunBatch(work As IPingBatchWork) Implements IPingBatchWorker.RunBatch
        If (Not IsRunning) Then
            ''Assign work
            _currentWork = work

            ''Do The job
            For Each url As Uri In work.Urls
                ProcessOne(url, work.Timeout, work.MaxRetry, work.MaxEmailSend)
            Next

            ''Release worker
            _currentWork = Nothing
        End If
    End Sub

    Private Sub ProcessOne(ByVal url As Uri, timeout As Integer, maxRetry As Integer, maxEmailSend As Integer)
        Try
            ' Process the request
            Dim ret As Boolean = _pinger.PingUrl(url, timeout, maxRetry)

            Dim _errors As New Dictionary(Of String, PingError)

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
                    ''_mailer.SendSiteUp(_config.AdminsEmail, url, err.Count)
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
                If err.Count <= maxEmailSend Then

                    ''_mailer.SendSiteDown(_config.AdminsEmail, url, _errors.Count)

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Class PingError
        Public Property Url As Uri
        Public Property Count As Integer
    End Class

End Class
