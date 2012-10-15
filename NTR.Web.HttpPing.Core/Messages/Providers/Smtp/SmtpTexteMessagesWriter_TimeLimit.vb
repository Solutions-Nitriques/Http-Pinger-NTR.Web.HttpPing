Imports NTR.Web.HttpPing.Workers
Imports System.Text

Namespace Messages

    Public Class SmtpTextMessagesWriter_TimeLimit : Inherits SmtpTextMessagesWriter_Base

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            MyBase.New(messagesfilter)
        End Sub

        Private _lastSendWorkResultEmail As IWorkResult

        Protected Overrides Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage
            Dim goodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.GoodUrlResults
            Dim badsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.BadUrlResults
            Dim subject As String = ""

            If (badsUrl.Count = 0) Then
                subject = "INFO : No more site down"
            Else
                subject = String.Format("ERROR : {0} DOWN, {1} UP", badsUrl.Count, goodsUrl.Count)
            End If

            Dim Body As New StringBuilder()

            ''Body.AppendFormat("Current State : {1} DOWN, {0} UP", goodsUrl.Count, badsUrl.Count).AppendLine()

            If (badsUrl.Count > 0) Then
                Body.AppendLine.Append("ACTUAL SITE DOWN").AppendLine()
                WriteSiteListWithStatus(Body, badsUrl)
            End If

            If (_lastSendWorkResultEmail IsNot Nothing) Then
                Dim returnedSite As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = _lastSendWorkResultEmail.BadUrlResults.Intersect(workResult.GoodUrlResults)

                If (badsUrl.Count > 0) Then
                    Body.AppendLine.Append("Comming back site :").AppendLine()
                    WriteSiteList(Body, returnedSite)
                End If
            End If

            If (badsUrl.Count > 0) Then
                Body.AppendLine.Append("ACTUAL SITE UP").AppendLine()
                WriteSiteListWithStatus(Body, goodsUrl)
            End If

            _lastSendWorkResultEmail = workResult

            Body.AppendFormat("Current time  : {0:dd-MM-yyyy HH:mm:ss}", Now).AppendLine.AppendLine()
            Return New MessageModel(subject, Body.ToString)
        End Function

    End Class

End Namespace
