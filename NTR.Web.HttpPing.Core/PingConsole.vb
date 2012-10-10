Module PingConsole

    Private WithEvents process As New PingProcess

    Sub Main()

        Dim file As String = process.GetDirectory + "../../httpping.config"
        Console.WriteLine("Config file in {0} loaded", file)

        process.LaunchPing(file)

        Console.ReadLine()

        process.StopPing()
    End Sub

    Private Sub process_SiteProcessed(ByVal sender As Object, ByVal e As PingProcess.SiteProcessedEventArgs) Handles Process.SiteProcessed
        Console.WriteLine("Url {0} : {1}", e.Url, e.Success)
    End Sub
End Module
