
Namespace Config

    Public Class XmlConfigLoader : Implements IConfigLoader

        Private configFilePath As String

        Public Sub New(Optional ByVal configFileName As String = "")
            Me.configFilePath = GetDirectory() + configFileName
        End Sub

#Region "Public Function"

        Public Function LoadConfigs() As IConfigModel Implements IConfigLoader.LoadConfigs

            If (String.IsNullOrWhiteSpace(configFilePath)) Then
                Throw New ApplicationException("The config file path is invalid")
            End If

            Dim doc As New Xml.XmlDocument
            Dim node As Xml.XmlNode

            ''Load defaul Value
            Dim _processInterval As Long = 180000
            Dim _pingTimeout As Integer = 500
            Dim _pingRetryInterval As Integer = 800
            Dim _pingMaxRetry As Integer = 3
            Dim _mailServerAddr As String = Nothing
            Dim _mailServerPort As Integer = 25
            Dim _adminsEmail As String = Nothing
            Dim _urlTimeLimit As Integer = 5  'Minutes
            Dim _Urls As New Collections.ObjectModel.Collection(Of Uri)

            ''Load file
            doc.Load(configFilePath)

            ''Merge file with default value
            node = doc.SelectSingleNode("/config/processInterval")
            If node IsNot Nothing Then
                Long.TryParse(node.InnerText, _processInterval)
            End If

            ''Merge file with default value
            node = doc.SelectSingleNode("/config/urlTimeLimit")
            If node IsNot Nothing Then
                Integer.TryParse(node.InnerText, _urlTimeLimit)
            End If

            node = doc.SelectSingleNode("/config/pingTimeout")
            If node IsNot Nothing Then
                Integer.TryParse(node.InnerText, _pingTimeout)
            End If

            node = doc.SelectSingleNode("/config/pingRetryInterval")
            If node IsNot Nothing Then
                Integer.TryParse(node.InnerText, _pingRetryInterval)
            End If

            node = doc.SelectSingleNode("/config/pingMaxRetry")
            If node IsNot Nothing Then
                Integer.TryParse(node.InnerText, _pingMaxRetry)
            End If

            node = doc.SelectSingleNode("/config/mailServer/addr")
            If node IsNot Nothing Then
                _mailServerAddr = node.InnerText
            End If

            node = doc.SelectSingleNode("/config/mailServer/port")
            If node IsNot Nothing Then
                Integer.TryParse(node.InnerText, _mailServerPort)
            End If

            node = doc.SelectSingleNode("/config/emails")
            If node IsNot Nothing Then
                _adminsEmail = node.InnerText
            End If

            Dim urls As Xml.XmlNodeList = doc.SelectNodes("/config/urls/url")
            For Each url As Xml.XmlNode In urls
                _Urls.Add(New Uri(url.InnerText))
            Next

            ''Create the new model and return it
            Return ConfigModel.createModel(_processInterval, _Urls, _urlTimeLimit, _adminsEmail, _pingTimeout, _pingRetryInterval, _pingMaxRetry, _mailServerAddr, _mailServerPort)
        End Function

#End Region

#Region "Private function"

        Private Function GetDirectory() As String
            Dim loc As String = System.Reflection.Assembly.GetAssembly(GetType(XmlConfigLoader)).Location
            Dim directory As New Text.StringBuilder()
            Dim split As String() = loc.Split("\"c)
            For x As Integer = 0 To split.Length - 2
                directory.AppendFormat("{0}\", split(x))
            Next
            ''We are in the Bin / Debug directory
            Return directory.ToString
        End Function

#End Region

    End Class

End Namespace