Friend Class HttpPinger

    Public Function PingUrl(ByVal url As Uri, ByVal timeout As Integer, ByVal retries As Integer) As Boolean
        Dim ret As Boolean
        Dim tryCount As Integer

        Do While Not ret AndAlso tryCount < retries
            Try
                Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.CreateDefault(url), Net.HttpWebRequest)
                request.Timeout = timeout
                request.Method = "HEAD"

                Dim response As Net.HttpWebResponse = DirectCast(request.GetResponse, Net.HttpWebResponse)

                ret = (response.StatusCode = Net.HttpStatusCode.OK)

                response.Close()

            Catch ex As Exception
                ret = False
            Finally
                tryCount += 1

                If Not ret Then
                    System.Threading.Thread.Sleep(800)
                End If
            End Try
        Loop

        Return ret
    End Function

End Class
