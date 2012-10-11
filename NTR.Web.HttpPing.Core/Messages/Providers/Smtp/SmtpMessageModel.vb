Namespace Messages

    Public Class SmtpMessageModel : Implements IMessage, ISmtpMessage

#Region "Public Ctor"

        Public Sub New(ByVal [to] As String, ByVal message As IMessage)
            _to = [to]
            _message = message
        End Sub

#End Region

#Region "Public Property"

        Public ReadOnly Property [To] As String Implements ISmtpMessage.To
            Get
                Return _to
            End Get
        End Property

        Public ReadOnly Property body As String Implements IMessage.Body
            Get
                Return _message.Body
            End Get
        End Property

        Public ReadOnly Property subject As String Implements IMessage.Subject
            Get
                Return _message.Subject
            End Get
        End Property

#End Region

#Region "Private member"

        Private _to As String
        Private _message As IMessage

#End Region

    End Class

End Namespace
