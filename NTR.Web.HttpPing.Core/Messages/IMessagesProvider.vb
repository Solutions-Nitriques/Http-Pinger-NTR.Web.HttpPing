Public Interface IMessagesProvider(Of T)

    Function Init(config As T) As Boolean
    Function SendMessage(message As IMessage) As Boolean
    Sub SendMessageAsync(message As IMessage)


End Interface
