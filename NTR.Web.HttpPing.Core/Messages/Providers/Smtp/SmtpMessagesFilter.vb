Imports NTR.Web.HttpPing.Workers
Imports NTR.Web.HttpPing.Config

Public Class SmtpMessagesFilter_Differential : Implements IMessagesFilter

    Public Function FilterProcessStatusMessage(ByVal messageType As Messages.ProcessMessageType) As Boolean Implements IMessagesFilter.FilterProcessStatusMessage
        Return True
    End Function

    Public Function FilterWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterWorkStatusMessage
        Return workResult.CreateDifferentialWorkResult(lastWorkResult).UrlResults.Count > 0
    End Function

    Public Function FilterUrlStatusMessage(ByVal urlResult As IPingUrlResult) As Boolean Implements IMessagesFilter.FilterUrlStatusMessage
        Return False
    End Function

End Class

Public Class SmtpMessagesFilter_TimeLimit : Implements IMessagesFilter

    Public Function FilterProcessStatusMessage(ByVal messageType As Messages.ProcessMessageType) As Boolean Implements IMessagesFilter.FilterProcessStatusMessage
        Return True
    End Function

    Private _lastEmailSend As DateTime = DateTime.MinValue
    Private _lastEmailSendCount As Integer

    Public Function FilterWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterWorkStatusMessage
        Dim result As Boolean = False
        Dim filterResult As Boolean = False
        Dim timeLimitSubset As IWorkResult = workResult.CreateTimeLimitSubsetWorkResult(lastWorkResult, config.UrlTimeLimit)

        If (lastWorkResult IsNot Nothing) Then
            filterResult = timeLimitSubset.UrlResults.Count > 0
        End If

        If (filterResult) Then
            If (_lastEmailSend = DateTime.MinValue OrElse _lastEmailSendCount <> timeLimitSubset.UrlResults.Count) Then
                _lastEmailSend = Now
                result = filterResult
                _lastEmailSendCount = timeLimitSubset.UrlResults.Count
            End If
        Else
            _lastEmailSend = DateTime.MinValue
            _lastEmailSendCount = 0
        End If
        Return result
    End Function

    Public Function FilterUrlStatusMessage(ByVal urlResult As IPingUrlResult) As Boolean Implements IMessagesFilter.FilterUrlStatusMessage
        Return True
    End Function

End Class
