Public Interface IPingBatchWorker

    ReadOnly Property IsRunning As Boolean
    Sub RunBatch(work As IPingBatchWork)

End Interface
