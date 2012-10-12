Imports NTR.Web.HttpPing.Config

Namespace Messages

    Public Class ConsoleMessagesProvider : Inherits AbstractMessagesProvider

        Public Sub New(ByVal messagesWriter As AbstractMessagesWriter)
            MyBase.New(messagesWriter)
        End Sub

        Friend Overrides Sub Init(ByVal config As IConfigModel)
            'nothing to do
        End Sub

        Friend Overrides Function SendMessage(ByVal message As IMessage, ByVal async As Boolean) As Boolean
            Console.WriteLine(message.Body)
            Return True
        End Function
    End Class

End Namespace
