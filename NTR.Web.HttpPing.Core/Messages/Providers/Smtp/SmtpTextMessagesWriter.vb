Imports NTR.Web.HttpPing.Config
Imports NTR.Web.HttpPing.Workers
Imports System.Text

Namespace Messages

    Public MustInherit Class SmtpTextMessagesWriter : Inherits AbstractMessagesWriter

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            MyBase.New(messagesfilter)
        End Sub

        Protected Overrides Function CreateProcessMessage(ByVal messageType As ProcessMessageType, ByVal config As IConfigModel) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            Select Case messageType
                Case ProcessMessageType.Starting
                    subject = "INFO : HttpPing is UP"
                    body = String.Format("HttpPing has started at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
                Case ProcessMessageType.Stopping
                    subject = String.Format("WARN: HttpPing is DOWN")
                    body = String.Format("HttpPing has stopped  at {0:dd-MM-yyyy HH:mm:ss} on {1}", Now, Environment.MachineName)
            End Select

            Return New MessageModel(subject, body)
        End Function

        Protected Overrides Function CreateUrlStatusMessage(ByVal urlResult As IPingUrlResult) As IMessage
            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            If (urlResult.Succeed) Then
                subject = String.Format("WARN : {0} is UP", urlResult.Url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is now responding with a 200 OK status. {2:dd-MM-yyyy HH:mm:ss}", urlResult.Url.DnsSafeHost, urlResult.Url, Now)
            Else
                subject = String.Format("ERROR : {0} is DOWN", urlResult.Url.DnsSafeHost)
                body = String.Format("Web site {0} <{1}> is NOT responding with a 200 OK status. {2:dd-MM-yyyy HH:mm:ss}", urlResult.Url.DnsSafeHost, urlResult.Url, Now)
            End If
            Return New MessageModel(subject, body)
        End Function

    End Class

    Public Class SmtpTextMessagesWriter_Differential : Inherits SmtpTextMessagesWriter

        Protected Overrides Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage
            Dim goodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.GoodUrlResults
            Dim badsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.BadUrlResults

            Dim diffWorkResult As WorkResult = workResult.CreateDifferentialWorkResult(lastWorkResult)
            Dim diffGoodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = diffWorkResult.GoodUrlResults
            Dim diffBadsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = diffWorkResult.BadUrlResults

            Dim subject As String = ""
            Dim Body As New StringBuilder()

            Body.AppendFormat("Current State : {0} UP, {1} DOWN", goodsUrl.Count, badsUrl.Count).AppendLine()
            Body.AppendFormat("Current time  : {0:dd-MM-yyyy HH:mm:ss}", Now).AppendLine.AppendLine()

            If (diffBadsUrl.Count > 0 And diffGoodsUrl.Count > 0) Then
                subject = String.Format("ERROR : {1} DOWN:{0} more, {2} come alive", diffBadsUrl.Count, badsUrl.Count, goodsUrl.Count)
                Body.AppendFormat("New Site Down : ").AppendLine()
                WriteSiteList(Body, diffBadsUrl)
                Body.AppendFormat("New Site Up : ").AppendLine()
                WriteSiteList(Body, diffGoodsUrl)
            ElseIf diffBadsUrl.Count > 0 Then
                subject = String.Format("ERROR : {1} DOWN:{0} more", diffBadsUrl.Count, badsUrl.Count)
                Body.AppendFormat("New Site Down : ").AppendLine()
                WriteSiteList(Body, diffBadsUrl)
            ElseIf diffGoodsUrl.Count > 0 Then
                subject = String.Format("INFO : {0} come alive, {0} still DOWN", goodsUrl.Count)
                Body.AppendFormat("New Site Up : ").AppendLine()
                WriteSiteList(Body, diffGoodsUrl)
            End If

            If (badsUrl.Count > 0) Then
                Body.AppendLine.Append("ACTUAL SITE DOWN").AppendLine()
                WriteSiteList(Body, badsUrl)
            End If

            Return New MessageModel(subject, Body.ToString)
        End Function

    End Class

    Public Class SmtpTextMessagesWriter_TimeLimit : Inherits SmtpTextMessagesWriter

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            MyBase.New(messagesfilter)
        End Sub

        Protected Overrides Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage
            Dim goodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.GoodUrlResults
            Dim badsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.BadUrlResults

            Dim subject As String = String.Format("Error : {0} site DOWN", badsUrl.Count)
            Dim Body As New StringBuilder()

            Body.AppendFormat("Current State : {0} UP, {1} DOWN", goodsUrl.Count, badsUrl.Count).AppendLine()

            If (badsUrl.Count > 0) Then
                Body.AppendLine.Append("ACTUAL SITE DOWN").AppendLine()
                WriteSiteList(Body, badsUrl)
            End If

            Body.AppendFormat("Current time  : {0:dd-MM-yyyy HH:mm:ss}", Now).AppendLine.AppendLine()
            Return New MessageModel(subject, Body.ToString)
        End Function

    End Class

End Namespace