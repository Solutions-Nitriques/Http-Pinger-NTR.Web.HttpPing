
Namespace Workers

    Public Interface IPingBatchWork

        ReadOnly Property Urls As ICollection(Of Uri)
        ReadOnly Property PingTimeout As Integer
        ReadOnly Property PingRetryInterval As Integer
        ReadOnly Property PingMaxRetry As Integer

    End Interface

End Namespace