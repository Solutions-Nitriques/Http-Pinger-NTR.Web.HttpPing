Public Interface IConfigModel

    ReadOnly Property Urls As ICollection(Of Uri)

    ReadOnly Property Timeout As Integer

    ReadOnly Property AdminsEmail As String

    ReadOnly Property Interval As Double

    ReadOnly Property MaxRetry As Integer

    ReadOnly Property MaxEmailSend As Integer

    ReadOnly Property MailServerAddr As String

    ReadOnly Property MailServerPort As Integer

End Interface
