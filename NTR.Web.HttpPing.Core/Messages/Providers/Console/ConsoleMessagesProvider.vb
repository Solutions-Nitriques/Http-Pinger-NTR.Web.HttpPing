Namespace Messages

    Public Class ConsoleMessagesProvider : Inherits AbstractMessagesProvider

        Public Sub New(ByVal messagesWriter As AbstractMessagesWriter)
            MyBase.New(messagesWriter)
        End Sub

        Public Overrides Sub Init(ByVal config As IConfigModel)
            'nothing to do
        End Sub

        Protected Friend Overrides Function SendMessage(ByVal message As IMessage, ByVal async As Boolean) As Boolean
            Console.WriteLine(message.Body)
            Return True
        End Function
    End Class

End Namespace
