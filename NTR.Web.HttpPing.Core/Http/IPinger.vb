Namespace Pinger


    Public Interface IPinger

        Function PingUrl(ByVal url As Uri, ByVal pingTimeout As Integer, ByVal retryInterval As Integer, ByVal maxRetries As Integer) As Boolean

    End Interface

End Namespace
