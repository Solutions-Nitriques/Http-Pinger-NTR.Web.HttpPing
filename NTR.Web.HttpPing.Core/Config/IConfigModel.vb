Imports NTR.Web.HttpPing.Workers

Namespace Config

    Public Interface IConfigModel : Inherits IPingBatchWork

        ReadOnly Property ProcessInterval As Long

        ReadOnly Property AdminsEmail As String

        ReadOnly Property MailServerAddr As String

        ReadOnly Property MailServerPort As Integer

        ReadOnly Property UrlTimeLimit As Integer

        ReadOnly Property MaxHoursNoEmail As Integer


    End Interface

End Namespace
