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

        Private _Timeout As Integer = 1000
        Public ReadOnly Property Timeout As Integer Implements IConfigModel.Timeout
            Get
                Return _Timeout
            End Get
        End Property

        Private _AdminsEmail As String
        Public ReadOnly Property AdminsEmail As String Implements IConfigModel.AdminsEmail
            Get
                Return _AdminsEmail
            End Get
        End Property

        Private _Interval As Double
        Public ReadOnly Property Interval As Double Implements IConfigModel.Interval
            Get
                Return _Interval
            End Get
        End Property

        Private _MaxRetry As Integer
        Public ReadOnly Property MaxRetry As Integer Implements IConfigModel.MaxRetry
            Get
                Return _MaxRetry
            End Get
        End Property

        Private _MaxEmailSend As Integer
        Public ReadOnly Property MaxEmailSend As Integer Implements IConfigModel.MaxEmailSend
            Get
                Return _MaxEmailSend
            End Get
        End Property

        Private _MailServerAddr As String
        Public ReadOnly Property MailServerAddr As String Implements IConfigModel.MailServerAddr
            Get
                Return _MailServerAddr
            End Get
        End Property

        Private _MailServerPort As Integer
        Public ReadOnly Property MailServerPort As Integer Implements IConfigModel.MailServerPort
            Get
                Return _MailServerPort
            End Get
        End Property

        Friend Shared Function createModel( _
                                 urls As Collections.ObjectModel.Collection(Of Uri),
                                timeout As Integer,
                                adminsEmail As String,
                                interval As Double,
                                maxRetry As Integer,
                                maxEmailSend As Integer,
                                mailServerAddr As String,
                                mailServerPort As Integer
                                ) As IConfigModel

            Dim result = New ConfigModel()
            With result
                ._AdminsEmail = adminsEmail
                ._Interval = interval
                ._MailServerAddr = mailServerAddr
                ._MailServerPort = mailServerPort
                ._MaxEmailSend = maxEmailSend
                ._MaxRetry = maxRetry
                ._Timeout = timeout
                ._Urls = urls
            End With

            Return result
        End Function

    End Class

End Namespace
