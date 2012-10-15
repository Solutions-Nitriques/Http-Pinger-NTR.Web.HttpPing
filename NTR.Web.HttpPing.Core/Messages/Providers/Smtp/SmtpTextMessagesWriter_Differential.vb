Imports NTR.Web.HttpPing.Workers
Imports System.Text

Namespace Messages

    Public Class SmtpTextMessagesWriter_Differential : Inherits SmtpTextMessagesWriter_Base

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

End Namespace