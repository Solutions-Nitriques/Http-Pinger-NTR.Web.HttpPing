﻿Imports NTR.Web.HttpPing.Config

Namespace Messages

    Public MustInherit Class AbstractMessagesProvider

        Private _messagesWriter As AbstractMessagesWriter

        Public Sub New(ByVal messagesWriter As AbstractMessagesWriter)
            _messagesWriter = messagesWriter

        End Sub

        Friend ReadOnly Property Writer As AbstractMessagesWriter
            Get
                Return _messagesWriter
            End Get
        End Property

        Friend Sub Init(ByVal Config As IConfigModel)
            Initialisation(Config)
            _messagesWriter.Init(Me, Config)
        End Sub


        Protected MustOverride Sub Initialisation(ByVal config As IConfigModel)

        Friend MustOverride Function SendMessage(ByVal message As IMessage, ByVal async As Boolean) As Boolean

    End Class

End Namespace


