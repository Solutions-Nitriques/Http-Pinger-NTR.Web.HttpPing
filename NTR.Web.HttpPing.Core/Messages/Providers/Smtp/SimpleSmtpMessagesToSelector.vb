
Namespace Messages

    Public Class SimpleSmtpMessagesToSelector : Implements ISmtpMessagesToSelector

        Private _adminsEmail As String

        Public Sub Init(ByVal config As IConfigModel) Implements ISmtpMessagesToSelector.Init
            _adminsEmail = config.AdminsEmail
        End Sub

        Public Function CreateSmtpMessage(ByVal message As IMessage) As ISmtpMessage Implements ISmtpMessagesToSelector.CreateSmtpMessage
            Return New SmtpMessageModel(_adminsEmail, message)
        End Function

    End Class

End Namespace
