Friend Class Config

    Public Sub New()

    End Sub

    Private _Urls As Collections.ObjectModel.Collection(Of Uri)
    Public ReadOnly Property Urls As ICollection(Of Uri)
        Get
            If _Urls Is Nothing Then
                _Urls = New Collections.ObjectModel.Collection(Of Uri)
            End If
            Return _Urls
        End Get
    End Property

    Private _Timeout As Integer = 1000
    Public ReadOnly Property Timeout As Integer
        Get
            Return _Timeout
        End Get
    End Property

    Private _AdminsEmail As String
    Public ReadOnly Property AdminsEmail As String
        Get
            Return _AdminsEmail
        End Get
    End Property

    Private _Interval As Double
    Public ReadOnly Property Interval As Double
        Get
            Return _Interval
        End Get
    End Property

    Private _IsLoaded As Boolean
    Public ReadOnly Property IsLoaded As Boolean
        Get
            Return _IsLoaded
        End Get
    End Property

    Private _MaxRetry As Integer
    Public ReadOnly Property MaxRetry As Integer
        Get
            Return _MaxRetry
        End Get
    End Property

    Private _MaxEmailSend As Integer
    Public ReadOnly Property MaxEmailSend As Integer
        Get
            Return _MaxEmailSend
        End Get
    End Property

    Private _MailServerAddr As String
    Public ReadOnly Property MailServerAddr As String
        Get
            Return _MailServerAddr
        End Get
    End Property

    Private _MailServerPort As Integer
    Public ReadOnly Property MailServerPort As Integer
        Get
            Return _MailServerPort
        End Get
    End Property

    Friend Function LoadConfigs(ByVal configFile As String) As Boolean

        If _IsLoaded Then
            Return True
        End If

        Dim ret As Boolean
        'Try
        Dim doc As New Xml.XmlDocument
        Dim node As Xml.XmlNode

        doc.Load(configFile)

        node = doc.SelectSingleNode("/config/timeout")
        If node IsNot Nothing Then
            Integer.TryParse(node.InnerText, _Timeout)
        End If

        node = doc.SelectSingleNode("/config/interval")
        If node IsNot Nothing Then
            Double.TryParse(node.InnerText, _Interval)
        End If

        node = doc.SelectSingleNode("/config/maxRetry")
        If node IsNot Nothing Then
            Integer.TryParse(node.InnerText, _MaxRetry)
        End If

        node = doc.SelectSingleNode("/config/maxEmailSend")
        If node IsNot Nothing Then
            Integer.TryParse(node.InnerText, _MaxEmailSend)
        End If

        node = doc.SelectSingleNode("/config/mailServer/addr")
        If node IsNot Nothing Then
            _MailServerAddr = node.InnerText
        End If

        node = doc.SelectSingleNode("/config/mailServer/port")
        If node IsNot Nothing Then
            Integer.TryParse(node.InnerText, _MailServerPort)
        End If


        Dim urls As Xml.XmlNodeList = doc.SelectNodes("/config/urls/url")
        For Each url As Xml.XmlNode In urls
            Me.Urls.Add(New Uri(url.InnerText))
        Next

        node = doc.SelectSingleNode("/config/emails")
        If node IsNot Nothing Then
            _AdminsEmail = node.InnerText
        End If

        _IsLoaded = True

        ret = True

        'Catch ex As Exception
        '    ret = False
        'End Try


        Return ret
    End Function

End Class