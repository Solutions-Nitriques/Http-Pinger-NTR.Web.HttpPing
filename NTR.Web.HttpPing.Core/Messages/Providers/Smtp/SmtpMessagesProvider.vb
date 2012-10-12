Imports System.Net.Mail
Imports NTR.Web.HttpPing.Config

Namespace Messages

    Public Class SmtpMessagesProvider : Inherits AbstractMessagesProvider

#Region "Public Ctor"

        Public Sub New(ByVal messagesWriter As AbstractMessagesWriter, ByVal smtpMessagesToSelector As ISmtpMessagesToSelector)
            MyBase.New(messagesWriter)
            _smtpMessagesToSelector = smtpMessagesToSelector
        End Sub

#End Region

#Region "Public methodes"

        Friend Overrides Sub Init(ByVal config As IConfigModel)

            _smtpClient = New SmtpClient(config.MailServerAddr, config.MailServerPort)
            _smtpMessagesToSelector.Init(config)

        End Sub

#End Region

#Region "Private Section"

        Private _smtpMessagesToSelector As ISmtpMessagesToSelector

        Private _smtpClient As SmtpClient

#Region "Functions"

        Friend Overrides Function SendMessage(ByVal message As IMessage, ByVal async As Boolean) As Boolean

            ''Result for sync call
            Dim result As Boolean = False

            ''Keep a message instance for dispose on sync call
            Dim msg As Net.Mail.MailMessage = Nothing

            ''Do the job
            Try
                ''Apply the SmtpMessagesToSelector
                Dim smtpMessage As ISmtpMessage = _smtpMessagesToSelector.CreateSmtpMessage(message)

                ''Create the message envelope
                msg = CreateMailMessage(smtpMessage)

                ''Send the message
                If (async) Then
                    _smtpClient.SendAsync(msg, smtpMessage)
                Else
                    _smtpClient.Send(msg)
                End If

                result = True
            Catch ex As Exception
                result = False

                ''if assync, raise event for keeping uniformity of usability
                If (async) Then
                    ''todo: raise event

                End If

            Finally
                ''For sync call, dispose the message,
                ''For async call, the message will be disposed on the result event
                If Not async And msg IsNot Nothing Then
                    msg.Dispose()
                End If
            End Try

            Return result
        End Function

        Private Function CreateMailMessage(ByVal message As ISmtpMessage) As MailMessage
            Return New MailMessage("server@nitriques.com", message.To, message.Subject, message.Body)
        End Function

#End Region

#End Region

    End Class

End Namespace