Namespace Pinger

    Public Class HttpPinger : Implements IPinger

        Public Function PingUrl(ByVal url As Uri, ByVal pingTimeout As Integer, ByVal retryInterval As Integer, ByVal maxRetries As Integer) As IPingerResult Implements IPinger.PingUrl
            Dim lastStatusCode As Net.HttpStatusCode
            Dim tryCount As Integer
            Dim e As Exception = Nothing
            Dim retry As Boolean

            Do While retry AndAlso tryCount < maxRetries
                Try
                    ''Prepare request
                    Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.CreateDefault(url), Net.HttpWebRequest)
                    request.AllowAutoRedirect = True
                    request.Timeout = pingTimeout
                    request.Method = "HEAD"

                    ''Apply request
                    Dim response As Net.HttpWebResponse = DirectCast(request.GetResponse, Net.HttpWebResponse)

                    lastStatusCode = response.StatusCode
                    retry = response.StatusCode <> Net.HttpStatusCode.OK

                    response.Close()

                Catch ex As Exception
                    retry = True
                    e = ex
                Finally
                    tryCount += 1

                    If retry Then
                        System.Threading.Thread.Sleep(retryInterval)
                    End If
                End Try
            Loop

            Return New PingerResult(lastStatusCode, e)
        End Function

    End Class

End Namespace
