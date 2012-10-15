Namespace Workers

    Public Interface IPingUrlResult

        ReadOnly Property Url As Uri
        ReadOnly Property Succeed As Boolean
        ReadOnly Property LastErrorTime As Nullable(Of DateTime)

    End Interface

    Public Class PingUrlResult : Implements IPingUrlResult

        Sub New(ByVal succeed As Boolean, ByVal url As Uri)
            _succeed = succeed
            _url = url
            If (Not _succeed) Then
                _lastErrorTime = New Nullable(Of DateTime)(Now)
            Else
                _lastErrorTime = New Nullable(Of DateTime)()
            End If
        End Sub

        Private _url As Uri

        Public ReadOnly Property Url As Uri Implements IPingUrlResult.Url
            Get
                Return _url
            End Get
        End Property

        Private _succeed As Boolean
        Public ReadOnly Property Succeed As Boolean Implements IPingUrlResult.Succeed
            Get
                Return _succeed
            End Get
        End Property

        Private _lastErrorTime As Nullable(Of DateTime)
        Public ReadOnly Property LastErrorTime As Nullable(Of DateTime) Implements IPingUrlResult.LastErrorTime
            Get
                Return _lastErrorTime
            End Get
        End Property

    End Class

End Namespace