
Namespace Workers

    Public Interface IPingBatchWork

        ReadOnly Property Urls As ICollection(Of Uri)
        ReadOnly Property Timeout As Integer
        ReadOnly Property MaxRetry As Integer
        ReadOnly Property MaxEmailSend As Integer

    End Interface

End Namespace