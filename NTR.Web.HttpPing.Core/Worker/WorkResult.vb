Imports NTR.Web.HttpPing.Workers

Public Interface IWorkResult

    ReadOnly Property UrlResults As IDictionary(Of Uri, IPingUrlResult)
    ReadOnly Property GoodUrlResults As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult))
    ReadOnly Property BadUrlResults As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult))

    Function CreateDifferentialWorkResult(ByVal lastWorkResult As IWorkResult) As WorkResult
    Function CreateTimeLimitSubsetWorkResult(ByVal lastWorkResult As IWorkResult, ByVal timeLimit As Integer) As WorkResult

End Interface

Public Class WorkResult : Implements IWorkResult

    Private _workResultList As IDictionary(Of Uri, IPingUrlResult)

    Friend Sub New(ByVal workResultList As IDictionary(Of Uri, IPingUrlResult))
        _workResultList = workResultList
    End Sub

    Public ReadOnly Property UrlResults As IDictionary(Of Uri, IPingUrlResult) Implements IWorkResult.UrlResults
        Get
            Return _workResultList
        End Get
    End Property

    Public ReadOnly Property BadUrlResults As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) Implements IWorkResult.BadUrlResults
        Get
            Return _workResultList.AsQueryable.Where(hasError)
        End Get
    End Property

    Public ReadOnly Property GoodUrlResults As IEnumerable(Of KeyValuePair(Of Uri, IPingUrlResult)) Implements IWorkResult.GoodUrlResults
        Get
            Return _workResultList.AsQueryable.Where(hasNoError)
        End Get
    End Property

    Public Function CreateDifferentialWorkResult(ByVal lastWorkResult As IWorkResult) As WorkResult Implements IWorkResult.CreateDifferentialWorkResult
        Dim diffList As IDictionary(Of Uri, IPingUrlResult) = New Dictionary(Of Uri, IPingUrlResult)
        If lastWorkResult IsNot Nothing Then
            For Each w In _workResultList
                If (w.Value.Succeed <> lastWorkResult.UrlResults(w.Key).Succeed) Then
                    diffList.Add(w.Key, w.Value)
                End If
            Next
        Else
            diffList = _workResultList
        End If
        Return New WorkResult(diffList)
    End Function

    Public Function CreateTimeLimitSubsetWorkResult(ByVal lastWorkResult As IWorkResult, ByVal timeLimit As Integer) As WorkResult Implements IWorkResult.CreateTimeLimitSubsetWorkResult
        Dim diffList As IDictionary(Of Uri, IPingUrlResult) = New Dictionary(Of Uri, IPingUrlResult)
        If lastWorkResult IsNot Nothing Then
            For Each w In _workResultList
                If Not w.Value.Succeed AndAlso lastWorkResult.UrlResults(w.Key).LastErrorTime.HasValue AndAlso lastWorkResult.UrlResults(w.Key).LastErrorTime.Value.AddMinutes(timeLimit) < Now Then
                    diffList.Add(w.Key, w.Value)
                End If
            Next
        End If
        Return New WorkResult(diffList)
    End Function

    Private hasError As Func(Of KeyValuePair(Of Uri, IPingUrlResult), Boolean) = Function(item As KeyValuePair(Of Uri, IPingUrlResult)) As Boolean
                                                                                     Return Not item.Value.Succeed
                                                                                 End Function

    Private hasNoError As Func(Of KeyValuePair(Of Uri, IPingUrlResult), Boolean) = Function(item As KeyValuePair(Of Uri, IPingUrlResult)) As Boolean
                                                                                       Return item.Value.Succeed
                                                                                   End Function




End Class
