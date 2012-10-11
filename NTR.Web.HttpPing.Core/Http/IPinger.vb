
Public Interface IPinger

    Function PingUrl(ByVal url As Uri, ByVal timeout As Integer, ByVal retries As Integer) As Boolean

End Interface
