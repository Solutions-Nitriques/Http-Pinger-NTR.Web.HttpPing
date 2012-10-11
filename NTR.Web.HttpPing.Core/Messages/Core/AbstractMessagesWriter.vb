
Namespace Messages

    Public Enum ProcessMessageType
        Unknow = 0
        Starting = 1
        Stopping = 2
        Running = 3
    End Enum

    Public MustInherit Class AbstractMessagesWriter

        Private _messageProvider As AbstractMessagesProvider

        Friend Sub Init(ByVal messagesProvider As AbstractMessagesProvider)
            _messageProvider = messagesProvider
        End Sub

        Public Sub WriteSiteStatusMessage(ByVal up As Boolean, ByVal url As Uri, ByVal retries As Integer, ByVal async As Boolean)
            If _messageProvider IsNot Nothing Then
                _messageProvider.SendMessage(createSiteStatusMessage(up, url, retries), async)
            End If
        End Sub

        Public Sub WriteProcessMessage(ByVal messageType As ProcessMessageType, ByVal async As Boolean)
            If _messageProvider IsNot Nothing Then
                _messageProvider.SendMessage(createProcessMessage(messageType), async)
            End If
        End Sub

        Protected MustOverride Function CreateSiteStatusMessage(ByVal up As Boolean, ByVal url As Uri, ByVal retries As Integer) As IMessage

        Protected MustOverride Function CreateProcessMessage(ByVal messageType As ProcessMessageType) As IMessage

    End Class

End Namespace

