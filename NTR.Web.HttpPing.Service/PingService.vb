Imports NTR.Web.HttpPing.Core

Public Class PingService

    Private WithEvents process As New PingProcess

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        If process IsNot Nothing Then
            Dim configFile As String = process.GetDirectory + "httpping.config"
            If args IsNot Nothing AndAlso args.Length > 0 Then
                configFile = args(0)
            End If
            process.LaunchPing(configFile)
        End If
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        If process IsNot Nothing Then
            process.StopPing()
        End If
    End Sub

End Class
