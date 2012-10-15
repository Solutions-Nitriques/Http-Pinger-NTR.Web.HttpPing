Imports System.Net

Namespace Pinger

    Public Class PingerResult : Implements IPingerResult

        Friend Sub New(ByVal httpResult As HttpStatusCode, ByVal ex As Exception)
            _httpResult = httpResult
            _ex = ex
            checkStatus()
        End Sub

        Public ReadOnly Property Exception As Exception Implements IPingerResult.Exception
            Get
                Return _ex
            End Get
        End Property

        Public ReadOnly Property HttpResult As HttpStatusCode Implements IPingerResult.HttpResult
            Get
                Return _httpResult
            End Get
        End Property

        Public ReadOnly Property IsOk As Boolean Implements IPingerResult.IsOk
            Get
                Return _isOk
            End Get
        End Property

        Private _isOk As Boolean
        Private _httpResult As HttpStatusCode
        Private _ex As Exception

        Private Sub checkStatus()
            _isOk = _ex Is Nothing AndAlso (HttpStatusCode.OK = _httpResult)
        End Sub

    End Class

End Namespace
