Public Interface IConfigModel : Inherits IPingBatchWork

    ReadOnly Property AdminsEmail As String

    ReadOnly Property Interval As Double

    ReadOnly Property MailServerAddr As String

    ReadOnly Property MailServerPort As Integer


    '' IPingBatchWork
    'ReadOnly Property Urls As ICollection(Of Uri)

    'ReadOnly Property Timeout As Integer

    'ReadOnly Property MaxRetry As Integer

    'ReadOnly Property MaxEmailSend As Integer

End Interface
