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

        Protected Overrides Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage
            Dim goodsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.GoodUrlResults
            Dim badsUrl As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) = workResult.BadUrlResults

            Dim subject As String = String.Format("ERROR : {0} site DOWN", badsUrl.Count)
            Dim Body As New StringBuilder()

            Body.AppendFormat("Current State : {1} DOWN, {0} UP", goodsUrl.Count, badsUrl.Count).AppendLine()

            If (badsUrl.Count > 0) Then
                Body.AppendLine.Append("ACTUAL SITE DOWN").AppendLine()
                WriteSiteListWithStatus(Body, badsUrl)
            End If

            Body.AppendFormat("Current time  : {0:dd-MM-yyyy HH:mm:ss}", Now).AppendLine.AppendLine()
            Return New MessageModel(subject, Body.ToString)
        End Function

    End Class

End Namespace
