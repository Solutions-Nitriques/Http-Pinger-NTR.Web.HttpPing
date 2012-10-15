Namespace Workers

    Public Interface IPingBatchWorker

        ReadOnly Property IsRunning As Boolean
        Function Run(ByVal work As IPingBatchWork) As IWorkResult

    End Interface

End Namespace
