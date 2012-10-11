
Namespace Messages

    Public Class MessageModel : Implements IMessage

#Region "Public Ctor"

        Public Sub New(ByVal subject As String, ByVal body As String)
            _subject = subject
            _body = body
        End Sub

#End Region

#Region "Public Property"

        Private _body As String
        Public ReadOnly Property body As String Implements IMessage.Body
            Get
                Return _body
            End Get
        End Property

        Private _subject As String
        Public ReadOnly Property subject As String Implements IMessage.Subject
            Get
                Return _subject
            End Get
        End Property

#End Region

    End Class

End Namespace
