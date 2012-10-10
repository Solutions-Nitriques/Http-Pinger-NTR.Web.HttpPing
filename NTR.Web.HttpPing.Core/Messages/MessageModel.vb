Public Class MessageModel : Implements IMessage

    Public Sub New([to] As String, subject As String, body As String)
        _to = [to]
        _subject = subject
        _body = body
    End Sub

    Private _body As String
    Public ReadOnly Property body As String Implements IMessage.body
        Get
            Return _body
        End Get
    End Property

    Private _subject As String
    Public ReadOnly Property subject As String Implements IMessage.subject
        Get
            Return _subject
        End Get
    End Property

    Private _to As String
    Public ReadOnly Property [to] As String Implements IMessage.to
        Get
            Return _to
        End Get
    End Property

End Class
