Imports NTR.Web.HttpPing.Workers
Imports NTR.Web.HttpPing.Config
Imports System.Text

Namespace Messages

    Public Enum ProcessMessageType
        Unknow = 0
        Starting = 1
        Stopping = 2
        Running = 3
    End Enum

    Public MustInherit Class AbstractMessagesWriter

#Region "Public"

#Region "Ctor"

        Public Sub New()
            Me.New(New NoFilterMessage())
        End Sub

        Public Sub New(ByVal messagesfilter As IMessagesFilter)
            _messagesFilter = messagesfilter
        End Sub

#End Region

#Region "Methodes"

        Public Sub WriteProcessMessage(ByVal messageType As ProcessMessageType, ByVal config As IConfigModel, ByVal async As Boolean)
            If _messagesProvider IsNot Nothing Then
                ''Send the message
                If (_messagesFilter.FilterProcessStatusMessage(messageType)) Then

                    _messagesProvider.SendMessage(CreateProcessMessage(messageType, config), async)
                End If
            End If
        End Sub

        Public Sub WriteWorkStatusMessage(ByVal workResult As IWorkResult, ByVal async As Boolean)
            If _messagesProvider IsNot Nothing Then
                ''Send Individual message
                For Each item In workResult.UrlResults
                    If (_messagesFilter.FilterUrlStatusMessage(item.Value)) Then
                        _messagesProvider.SendMessage(CreateUrlStatusMessage(item.Value), async)
                    End If
                Next

                ''Send the message
                If (_messagesFilter Is Nothing OrElse _messagesFilter.FilterWorkStatusMessage(workResult, _lastWorkResult, Config)) Then
                    _messagesProvider.SendMessage(CreateWorkStatusMessage(workResult, _lastWorkResult), async)
                End If
            End If
            mergeWorkResult(workResult)
        End Sub

        Public Sub WriteUrlStatusMessage(ByVal urlResult As IPingUrlResult, ByVal async As Boolean)
            If _messagesProvider IsNot Nothing Then
                If (_messagesFilter.FilterUrlStatusMessage(urlResult)) Then
                    _messagesProvider.SendMessage(CreateUrlStatusMessage(urlResult), async)
                End If
            End If
        End Sub

#End Region

#End Region

#Region "Friend Init"

        Friend Sub Init(ByVal messagesProvider As AbstractMessagesProvider, ByVal config As IConfigModel)
            _messagesProvider = messagesProvider
            _config = config
        End Sub

#End Region

#Region "Protected"

        Protected Sub WriteSiteList(ByVal body As StringBuilder, ByVal list As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)))
            For Each it As KeyValuePair(Of Uri, IPingUrlResult) In list
                body.AppendFormat("  {0}", it.Value.Url).AppendLine()
            Next
        End Sub

        Protected Sub WriteSiteListWithStatus(ByVal body As StringBuilder, ByVal list As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)))
            For Each it As KeyValuePair(Of Uri, IPingUrlResult) In list
                body.AppendFormat("  {0}", it.Value.Url).AppendLine()
                body.AppendFormat("  --HttpStatus : {0}", it.Value.PingerResult.HttpResult).AppendLine()
                If (it.Value.PingerResult.Exception IsNot Nothing) Then
                    body.AppendFormat("--Exception : {0}", it.Value.PingerResult.Exception.Message).AppendLine()
                End If
            Next
        End Sub

        Protected ReadOnly Property Config As IConfigModel
            Get
                Return _config
            End Get
        End Property

#Region "MustOverride"

        Protected MustOverride Function CreateProcessMessage(ByVal messageType As ProcessMessageType, ByVal config As IConfigModel) As IMessage

        Protected MustOverride Function CreateWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult) As IMessage

        Protected MustOverride Function CreateUrlStatusMessage(ByVal UrlResult As IPingUrlResult) As IMessage

#End Region

#End Region

#Region "Private section"

        Private _messagesProvider As AbstractMessagesProvider
        Private _messagesFilter As IMessagesFilter
        Private _initedTime As DateTime = Now
        Private _lastWorkResult As IWorkResult = Nothing
        Private _config As IConfigModel

        Private Sub mergeWorkResult(ByVal workResult As IWorkResult)

            If (_lastWorkResult Is Nothing) Then
                _lastWorkResult = workResult
            Else
                Dim newLastWork As IDictionary(Of Uri, IPingUrlResult) = New Dictionary(Of Uri, IPingUrlResult)
                For Each it In workResult.UrlResults
                    If (it.Value.Succeed) Then
                        newLastWork.Add(it.Key, it.Value)
                    Else
                        If (_lastWorkResult.UrlResults(it.Key).LastErrorTime.HasValue) Then
                            newLastWork.Add(it.Key, _lastWorkResult.UrlResults(it.Key))
                        Else
                            newLastWork.Add(it.Key, it.Value)
                        End If
                    End If
                Next
                _lastWorkResult = New WorkResult(newLastWork)
            End If
        End Sub

#End Region

#Region "Private Inner Class"

        Private Class NoFilterMessage : Implements IMessagesFilter

            Public Function FilterProcessStatusMessage(ByVal messageType As ProcessMessageType) As Boolean Implements IMessagesFilter.FilterProcessStatusMessage
                Return True
            End Function

            Public Function FilterUrlStatusMessage(ByVal urlResult As Workers.IPingUrlResult) As Boolean Implements IMessagesFilter.FilterUrlStatusMessage
                Return True
            End Function

            Public Function FilterWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult, ByVal config As IConfigModel) As Boolean Implements IMessagesFilter.FilterWorkStatusMessage
                Return True
            End Function
        End Class

#End Region

    End Class



End Namespace

