Imports NTR.Web.HttpPing.Workers
Imports NTR.Web.HttpPing.Config

Public Class SmtpMessagesFilter_Differential : Implements IMessagesFilter

    Public Function FilterProcessStatusMessage(ByVal messageType As Messages.ProcessMessageType, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterProcessStatusMessage
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

    Private _lastSendMessageDate As DateTime

    Private Sub updateLastSendMessageDate(ByVal result As Boolean)
        If (result) Then
            _lastSendMessageDate = Now
        End If
    End Sub

    Public Function FilterProcessStatusMessage(ByVal messageType As Messages.ProcessMessageType, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterProcessStatusMessage
        Dim result As Boolean = True
        If (messageType = Messages.ProcessMessageType.Running) Then
            result = _lastSendMessageDate.AddHours(config.MaxHoursNoEmail) < Now
        End If
        updateLastSendMessageDate(result)
        Return result
    End Function

    Private _lastErrorEmailSend As DateTime = DateTime.MinValue
    Private _lastWorkResultErrorEmailSend As IWorkResult

    Public Function FilterWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterWorkStatusMessage
        'Dim result As Boolean = False
        Dim hasSiteGoingUp As Boolean = False
        Dim hasErrorEmail As Boolean = False
        Dim timeLimitSubset As IWorkResult = workResult.CreateTimeLimitSubsetWorkResult(lastWorkResult, config.UrlTimeLimit)
        Dim hasUrlInErrorOverTimeLimit As Boolean = timeLimitSubset.UrlResults.Count > 0

        ''Check if we have site going up from last error send
        If (_lastWorkResultErrorEmailSend IsNot Nothing) Then
            'we have sended an email with error
            ''Check if we have site going up

            If _lastWorkResultErrorEmailSend.BadUrlResults.Intersect(workResult.GoodUrlResults).Count > 0 Then
                'We have site going up
                hasSiteGoingUp = True

                'update the list
                _lastWorkResultErrorEmailSend = workResult
            End If
        End If

        ''Check TimeLimit
        If (hasUrlInErrorOverTimeLimit) Then
            ''we have url over the time limit

            ''Check if we need to send an email
            If (_lastErrorEmailSend = DateTime.MinValue) Then
                ''We did not already send an email
                ''we need to send an email
                hasErrorEmail = True
            End If
            If (_lastErrorEmailSend.AddHours(6) < Now) Then
                'We sended an email 6 hours ago and still have to send a new one
                hasErrorEmail = True
            End If
            If (_lastWorkResultErrorEmailSend IsNot Nothing AndAlso _lastWorkResultErrorEmailSend.CreateTimeLimitSubsetWorkResult(lastWorkResult, config.UrlTimeLimit).BadUrlResults.Count <> timeLimitSubset.BadUrlResults.Count) Then
                'the count is different
                hasErrorEmail = True
            End If

            If (hasErrorEmail) Then
                _lastErrorEmailSend = Now
                _lastWorkResultErrorEmailSend = workResult
            End If
        Else
            ''No url are over the timelimite
            ''Reset error
            _lastErrorEmailSend = DateTime.MinValue
            _lastWorkResultErrorEmailSend = Nothing
        End If
        Dim result As Boolean = (hasSiteGoingUp OrElse hasErrorEmail)
        updateLastSendMessageDate(result)
        Return result
    End Function

    Public Function FilterUrlStatusMessage(ByVal urlResult As IPingUrlResult) As Boolean Implements IMessagesFilter.FilterUrlStatusMessage
        Return False
    End Function

End Class
