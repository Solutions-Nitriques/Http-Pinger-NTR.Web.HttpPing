Imports NTR.Web.HttpPing.Config

Public Class MockConfigLoader : Implements IConfigLoader

    Private _config As ConfigModel

    Public Sub New(ByVal config As ConfigModel)
        _config = config
    End Sub

    Public Function LoadConfigs() As Config.IConfigModel Implements Config.IConfigLoader.LoadConfigs
        Return _config
    End Function
End Class
