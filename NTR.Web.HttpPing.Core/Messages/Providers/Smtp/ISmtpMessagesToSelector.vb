
Namespace Messages

    Public Interface ISmtpMessagesToSelector

        Function CreateSmtpMessage(ByVal message As IMessage) As ISmtpMessage

        Sub Init(ByVal config As IConfigModel)

    End Interface

End Namespace
