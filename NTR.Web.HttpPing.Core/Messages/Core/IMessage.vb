
Namespace Messages

    Public Interface IMessage

        ReadOnly Property Subject As String
        ReadOnly Property Body As String
        ''ReadOnly Property Category As String

    End Interface

    Public Interface ISmtpMessage : Inherits IMessage

        ReadOnly Property [To] As String

    End Interface

End Namespace
