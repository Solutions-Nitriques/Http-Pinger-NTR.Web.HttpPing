Namespace Config

    Public Interface IConfigModel : Inherits IPingBatchWork

        ReadOnly Property AdminsEmail As String

        ReadOnly Property Interval As Double

        ReadOnly Property MailServerAddr As String

        ReadOnly Property MailServerPort As Integer

    End Interface

End Namespace
