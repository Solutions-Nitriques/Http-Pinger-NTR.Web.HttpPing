Namespace Config

    Public Class ConfigModel : Implements IConfigModel

        Private _Urls As Collections.ObjectModel.Collection(Of Uri)
        Public ReadOnly Property Urls As ICollection(Of Uri) Implements IConfigModel.Urls
            Get
                If _Urls Is Nothing Then
                    _Urls = New Collections.ObjectModel.Collection(Of Uri)
                End If
                Return _Urls
            End Get
        End Property

        Private _AdminsEmail As String
        Public ReadOnly Property AdminsEmail As String Implements IConfigModel.AdminsEmail
            Get
                Return _AdminsEmail
            End Get
        End Property

        Private _pingRetryInterval As Integer
        Public ReadOnly Property PingRetryInterval As Integer Implements IConfigModel.PingRetryInterval
            Get
                Return _pingRetryInterval
            End Get
        End Property

        Private _pingMaxRetry As Integer
        Public ReadOnly Property PingMaxRetry As Integer Implements IConfigModel.PingMaxRetry
            Get
                Return _pingMaxRetry
            End Get
        End Property

        Private _mailServerAddress As String
        Public ReadOnly Property MailServerAddress As String Implements IConfigModel.MailServerAddr
            Get
                Return _mailServerAddress
            End Get
        End Property

        Private _mailServerPort As Integer
        Public ReadOnly Property MailServerPort As Integer Implements IConfigModel.MailServerPort
            Get
                Return _mailServerPort
            End Get
        End Property

        Private _processInterval As Long
        Public ReadOnly Property ProcessInterval As Long Implements IConfigModel.ProcessInterval
            Get
                Return _processInterval
            End Get
        End Property

        Private _pingTimeout As Integer
        Public ReadOnly Property PingTimeout As Integer Implements Workers.IPingBatchWork.PingTimeout
            Get
                Return _pingTimeout
            End Get
        End Property

        Private _UrlTimeLimit As Integer
        Public ReadOnly Property UrlTimeLimit As Integer Implements IConfigModel.UrlTimeLimit
            Get
                Return _UrlTimeLimit
            End Get
        End Property

        Public Shared Function createModel( _
                                ByVal processInterval As Long,
                                 ByVal urls As Collections.ObjectModel.Collection(Of Uri),
                                 ByVal urlTimeLimit As Integer,
                                ByVal adminsEmail As String,
                                ByVal pingTimeout As Integer,
                                ByVal pingRetryInterval As Integer,
                                ByVal pingMaxRetry As Integer,
                                ByVal mailServerAddress As String,
                                ByVal mailServerPort As Integer
                                ) As IConfigModel

            Dim result = New ConfigModel()
            With result
                ._processInterval = processInterval
                ._Urls = urls
                ._UrlTimeLimit = urlTimeLimit
                ._AdminsEmail = adminsEmail
                ._pingRetryInterval = pingRetryInterval
                ._pingMaxRetry = pingMaxRetry
                ._pingTimeout = pingTimeout
                ._mailServerAddress = mailServerAddress
                ._mailServerPort = mailServerPort
            End With

            Return result
        End Function

    End Class

End Namespace
