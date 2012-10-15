Imports NTR.Web.HttpPing.Pinger

Namespace Workers

    Public Interface IPingUrlResult

        ReadOnly Property Url As Uri
        ReadOnly Property PingerResult As IPingerResult
        ReadOnly Property LastErrorTime As Nullable(Of DateTime)
        ReadOnly Property Succeed As Boolean

    End Interface

    Public Class PingUrlResult : Implements IPingUrlResult

        Sub New(ByVal pingerResult As IPingerResult, ByVal url As Uri)
            _pingerResult = pingerResult
            _url = url
            saveLastErrorTime()
        End Sub

        Public ReadOnly Property Url As Uri Implements IPingUrlResult.Url
            Get
                Return _url
            End Get
        End Property

        Public ReadOnly Property Succeed As Boolean Implements IPingUrlResult.Succeed
            Get
                Return _pingerResult.IsOk
            End Get
        End Property

        Public ReadOnly Property LastErrorTime As Nullable(Of DateTime) Implements IPingUrlResult.LastErrorTime
            Get
                Return _lastErrorTime
            End Get
        End Property

        Public ReadOnly Property PingerResult As IPingerResult Implements IPingUrlResult.PingerResult
            Get
                Return _pingerResult
            End Get
        End Property

        Private _url As Uri
        Private _lastErrorTime As Nullable(Of DateTime)
        Private _pingerResult As IPingerResult

        Private Sub saveLastErrorTime()
            If (Not _pingerResult.IsOk) Then
                _lastErrorTime = New Nullable(Of DateTime)(Now)
            Else
                _lastErrorTime = New Nullable(Of DateTime)()
            End If

        End Sub

    End Class

End Namespace