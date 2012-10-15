Imports System.Net

Namespace Pinger
    Public Interface IPingerResult

        ReadOnly Property HttpResult As HttpStatusCode
        ReadOnly Property Exception As Exception
        ReadOnly Property IsOk As Boolean

    End Interface
End Namespace